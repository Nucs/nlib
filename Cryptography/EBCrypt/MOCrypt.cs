using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nucs.Collections;
using nucs.SystemCore;



////////////////////////////////////////////////
/*                   DESIGN
 *        Each line represents two bits 'bb', which give 4 options
 *        00 - decrement -
 *        01 - increment + 
 *        10 - jump back 
 *        11 - jump forward
 *        n - Provided with a number that represents number of jumps
 *        Inc and Dec are in range of 0 to 9, if it hits 9, jumps back to 0
 *        If jump is longer than calculated, say size is 50, calc = 60, jumps to 10 and so is the opposite
 *     
 * 
 * 
 *
 * 1 //1
 * 1 //2
 * 1 //3
 * 3 //JMP-BGN-N0 || << PUSH 3 //Jump begins in next 3, takes last push
 * 2 //3 HOLD 3*1 -> 3
 * 0 //1 HOLD 3*1 -> 3
 * 3 //4 HOLD-JMP 3*4 -> 12 //Jumps to 81
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 *
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 */
/////////////////////////////////////////////////

namespace nucs.Cryptography.EBCrypt {
    /// <summary>
    /// Stands for Multiple Options cryptography.
    /// </summary>
    public static class MOCrypt {

        public class MOObject {

            readonly bool StepsByPush = true;
            public MOObject(bool StepsByPushes = true) {
                StepsByPush = StepsByPushes;
            }

            #region Decrypt values

            /// <summary>
            /// Collection of the lines
            /// </summary>
            ImprovedList<Line> l; 
            /// <summary>
            /// Collection of the results
            /// </summary>
            List<byte> s; 
            /// <summary>
            /// Human count of the lines
            /// </summary>
            int l_c;
            /// <summary>
            /// Zero based count of the lines
            /// </summary>
            int l_ind;
            /// <summary>
            /// Steps done
            /// </summary>
            ulong steps;
            /// <summary>
            /// Key - Maximum human-based number of steps
            /// </summary>
            ulong n;
            /// <summary>
            /// current evaluation of increment/decrement from method `EVAL = v==1`
            /// </summary>
            byte eval;
            /// <summary>
            /// Key - start index
            /// </summary>
            long st;
            /// <summary>
            /// Current line that is being inspected
            /// </summary>
            long i;
            /// <summary>
            /// Current `i` state
            /// </summary>
            State state = State.None;
            /// <summary>
            /// Last `i` state
            /// </summary>
            State state_last = State.None;

            #endregion

            /// <summary>
            /// Decrypts a buffer of bytes, steps is the special key to get a specific value.
            /// </summary>
            /// <param name="buffer"></param>
            /// <param name="_n">n is aka steps till return value</param>
            /// <param name="_st">is the start index</param>
            /// <returns></returns>
            public List<byte> Decrypt(IEnumerable<bit> buffer, ulong _n, long _st) {
                st = _st;
                n = _n;                 //nunber of steps
                steps = 0;              //current step num
                l = ToLines(buffer);    //lines - set of two bits
                s = new List<byte>();   //results
                l_c = l.Count;          //count of lines
                l_ind = l.Count-1;      //index of last line
                eval = 0;               //collector of evaluation
                Jump(0, st, ref i);
                #region States

                //figure out first
                FigureState(l[i].toByte());
                #endregion



                for ( /*i is set above*/; i < l_c; Controller(ref i)) {
                    if (StepsByPush == false && steps == n) { break; } //once done doing steps //todo consider making this on value pushing, not actual steps
                    var v = l[i].toByte(); //value 0-3.
                    FigureState(v);
#if DEBUG
                    if (state != state_last)
                    Console.WriteLine("FIGURED: " + state);
#endif
                    #region React To New State

                    if (state_last == State.Evaluating && state != state_last)
                        PUSH();
                    
                    #endregion


                _refresh: switch (state) {
                        case State.Evaluating:
                            if (v > 1) { //todo shouldn't happen, remove if never indeed happens
                                FigureState(v);
                                goto _refresh;
                            }
                            EVAL = v == 1;
                            break;
                        case State.CollectingJumpBackward:
                            if (_JMP_BGN_Started == false) {
                                JMP_BGN();
                                break;
                            }
                            if (_JMP_BGN_Holder.Count != _JMP_BGN_Count)
                                HOLD = v;
                            else
                                HOLD_JMP = v;
                            break;
                        case State.CollectingJumpForward:
                            if (_JMP_BGN_Started == false) {
                                JMP_BGN();
                                break;
                            }
                            if (_JMP_BGN_Holder.Count != _JMP_BGN_Count)
                                HOLD = v;
                            else
                                HOLD_JMP = v;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    state_last = state;
                }

#if DEBUG
                Console.WriteLine("RESULT: ");
                foreach (var b in s) 
                    Console.Write(b);
#endif
                return s;
            }


            internal bool EVAL {
                set {
#if DEBUG
                    Console.WriteLine(eval + "->" + (eval + (value ? 1 : -1)));
#endif
                    Evalue(ref eval, value);
                    EVAL_Began = true;
                }
            }

            internal bool EVAL_Began = false;
            internal byte LASTPUSH { get { return s.Count > 0 ? s[s.Count-1] : (byte)0; } }


            internal void PUSH() {
                s.Add(eval);
                eval = 0;
                EVAL_Began = false;
                if (StepsByPush)
                    steps++;
#if DEBUG
                Console.WriteLine("PUSH " + s.Last());
#endif
            }

            internal long JMP {
                set {
                    Jump(i, value, ref i);
                    _JMP_BGN_Started = false;
                    _JMP_BGN_Count = -1;
                    _JMP_BGN_Holder.Clear();
                    state = State.None; //todo consider this
#if DEBUG
                    Console.WriteLine("JMP "+value);
                    Console.WriteLine("I: "+i);
#endif
                }
            }

            internal void JMP_BGN() { _JMP_BGN_Started = true; _JMP_BGN_Holder.Clear(); _JMP_BGN_Count = LASTPUSH; }

            internal byte HOLD {
                set {
                    if (_JMP_BGN_Started == false) return;
                    _JMP_BGN_Holder.Add(value);
#if DEBUG
                    Console.WriteLine("HOLD "+value);
#endif
                }
            }

            internal byte HOLD_JMP {
                set {
                    if (_JMP_BGN_Started == false) return;
                    HOLD = value;
                    JMP = JMP_Holders_Sum();
                }
            }

            internal bool _JMP_BGN_Started = false;
            internal short _JMP_BGN_Count = -1;
            internal readonly List<short> _JMP_BGN_Holder = new List<short>(); 
            /*switch (v) {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
            }*/

            private enum State : byte {
                None = 0,
                Evaluating = 1,
                CollectingJumpBackward = 2,
                CollectingJumpForward = 3
            }


            /// <summary>
            /// Figures I by N and l_count, figures if to quit loop 
            /// </summary>
            /// <param name="_i"></param>
            /// <returns></returns>
            private void Controller(ref long _i) {
                if (StepsByPush == false) {
                    if (++steps == n) _i = l_c; //force somewhere to quit
                } else {
                    if (steps == n) _i = l_c; //force somewhere to quit
                }
                if (_i++ == l_ind)
                    _i = 0;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="_atm"></param>
            /// <param name="_toJump"></param>
            /// <param name="newIndex"></param>
            /// <returns></returns>
            private void Jump(long _atm, long _toJump, ref long newIndex) {
                if (_toJump > 0) {
                    newIndex = (_atm + _toJump)%l_c;
                }

                else if (_toJump < 0) {
                    while (_toJump++ != 0) {
                        if (_atm == 0) {
                            _atm = l_ind; //loop to top
                            continue;
                        }
                        _atm--;
                    }
                    newIndex = _atm;
                }

                else if (_toJump == 0) {
                    newIndex = _atm;
                }
            }

            private void Evalue(ref byte _byte, bool direction) {
                if (direction) {
                    if (_byte + 1 == 10)
                        _byte = 0;
                    else
                        _byte++;
                } else {
                    if (_byte == 0)
                        _byte = 9;
                    else
                        _byte--;
                }
            }

            private long JMP_Holders_Sum() {
                var _s = _JMP_BGN_Holder.Select(b => b + 1L).ToArray();
                var _l = _s[0];
                for (int _i = 1; _i < _JMP_BGN_Holder.Count; _i++) {
                    _l = Convert.ToInt64(Math.Pow(_l, _s[_i]));
                }
                return _l;
            }

            private void FigureState(byte val) {
                switch (val) { 
                    case 0:
                        if (state == State.None)
                            state = State.Evaluating;
                        break;
                    case 1:
                        if (state == State.None)
                            state = State.Evaluating;
                        break;
                    case 2:
                        if (state != State.CollectingJumpForward)
                            state = State.CollectingJumpBackward;
                        break;
                    case 3:
                        if (state != State.CollectingJumpBackward)
                            state = State.CollectingJumpForward;
                        break;
                }
            }

        }


        
       

        #region Helping Tools
        /// <summary>
        /// breaks an array of bytes to IEnumerable of 'Line's
        /// </summary>
        /// <param name="buffer">Input stream</param>
        /// <returns>IEnumerable of Line</returns>
        public static ImprovedList<Line> ToLines(IEnumerable<bit> buffer) {
            var buff = buffer.ToArray();
            var l = new ImprovedList<Line>();
            for (var i = 0; i < buff.Length-1; i+=2) //*4 because it is 8bits per byte and they are groups of 2 so 8/2=4..
                l.Add(new Line() {a = buff[i], b = buff[i+1]});
            return l;
        } 

        /// <summary>
        /// Represents two bits, a - LSB (Right), b - MSB (Left)
        /// </summary>
        public struct Line {
            public bit a;
            public bit b;

            public byte toByte() {
                return (byte)(((byte)a) + ((byte)b * 2));
            }

            public override string ToString() {
                return (a ? "1" : "0") + (b ? "1" : "0");
            }
        }
        #endregion
    }
}
