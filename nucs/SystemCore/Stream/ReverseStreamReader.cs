using System;
using System.IO;
using System.Text;

/*
* Class: ReverseStreamReader
* Description: StreamReader to read a (ASCII) flat file in reverse (Bottom Up) mode
* Supported Methods: Read, Read (to buffer), ReadLine, ReadToEnd
* Author: Shyam K. Arjarapu
* Copyright: (c) Shyam K. Arjarapu. All rights reserved.
* 
*/

namespace Arjarapu.IO {
    public class ReverseStreamReader
        : StreamReader {
        protected long _position;
        public long Position { get { return _position;} }
        public long Length { get; private set; }
        public ReverseStreamReader(Stream stream) : base(stream) {
            // start reading from end of file and save the current position. 
            this.BaseStream.Seek(-1, SeekOrigin.End);
            _position = this.BaseStream.Position;
            Length = BaseStream.Length;
        }

        private void DecrementPosition() {
            // since we are reading the file is reverse over
            // the logic is more like Peek at current Char
            // and move the read position to char before the
            // current one. Making use of Peek produced wierd
            // errors. So I used Read and moved 2 positions

            if (_position > -1) {
                _position--;
                if (this.BaseStream.Position > 1)
                    this.BaseStream.Seek(-2, SeekOrigin.Current);
                else if (this.BaseStream.Position == 1)
                    this.BaseStream.Seek(-1, SeekOrigin.Current);
            }
        }

        public override int Read() {
            int charValue;
            //read the current character and move the 
            //current read position to char before it.
            //if we reached begining of stream return -1

            if (_position == -1)
                charValue = -1;
            else {
                charValue = base.BaseStream.ReadByte();
                DecrementPosition();
            }
            return charValue;
        }

        public override int Read(char[] buffer, int index, int count) {
            int charVal;
            int readCount = 0;
            //read count chars from current stream and 
            //insert into the buffer starting from index
            while (readCount < count) {
                charVal = this.Read();
                if (charVal != -1) {
                    buffer[index + readCount] = (Char) charVal;
                    readCount++;
                }
                else
                    break;
            }

            return readCount;
        }

        public override string ReadLine() {
            if (_position > -1) {
                StringBuilder osb = new StringBuilder();
                int charVal;
                // \r\n or just \n is line feed.
                // \r = 13 and \n = 10
                // since the reading done in reverse order
                // check for \n then followed by optional \r
                while ((charVal = this.Read()) != -1)
                    if (charVal == 10) {
                        //line break found; check for carriage return
                        charVal = this.Read();
                        if (charVal == 13)
                            // current charVal is 13. So, discard and 
                            // move the cursor back to where it was.
                            break;
                        else {
                            _position++;
                            this.BaseStream.Seek(1, SeekOrigin.Current);
                        }
                        break;
                    }
                    else
                        osb.Append((Char) charVal);

                return osb.ToString();
            }
            else
                return null;
        }

        public override String ReadToEnd() {
            String sline;
            StringBuilder oSB = new StringBuilder();
            while ((sline = this.ReadLine()) != null)
                oSB.AppendLine(sline);

            // replace \n\r with \r\n
            // \r = 13 and \n = 10
            Char cr = (Char) 13;
            Char nl = (Char) 10;
            String crnl = new String(new char[] {cr, nl});
            String nlcr = new String(new char[] {nl, cr});

            oSB.Replace(nlcr, crnl);

            return oSB.ToString();
        }
    }
}