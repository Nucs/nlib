using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using nucs.Collections;

namespace BotSuite.ImageLibrary {
    /// <summary>
    /// serach for patterns or images in a image
    /// </summary>
    public static class Template {
        /// <summary>
        /// search for binary patterns
        /// </summary>
        /// <param name="Img">image to look in</param>
        /// <param name="Pattern">pattern to look for</param>
        /// <param name="Tolerance">tolerance (0,...,255)</param>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// ImageData Img = new ImageData(...);
        /// bool[,] Pattern = new bool[] {   
        ///             {1,1,0,0,0,0,0,0},
        ///             {0,0,1,0,0,0,0,0},
        ///             {0,0,1,0,0,0,0,0},
        ///             {0,1,0,0,0,0,0,0},
        ///             {1,0,0,0,0,0,0,0},
        ///             {1,1,1,0,0,0,0,0},
        ///             {0,0,0,0,0,0,0,0},
        ///             {0,0,0,0,0,0,0,0}
        ///         };
        /// Rectangle Location = Template.BinaryPattern(Img,Pattern,2);
        /// ]]>
        /// </code>
        /// </example>
        /// <returns></returns>
        public static Rectangle BinaryPattern(ImageData Img, bool[,] Pattern, uint Tolerance = 0) {
            // simple
            Point location = Point.Empty;

            Color ReferenceColor = Color.Wheat;
            bool first = true;
            for (Int32 OuterColumn = 0; OuterColumn < Img.Width - Pattern.GetLength(1); OuterColumn++) {
                for (Int32 OuterRow = 0; OuterRow < Img.Height - Pattern.GetLength(0); OuterRow++) {
                    for (Int32 InnerColumn = 0; InnerColumn < Pattern.GetLength(1); InnerColumn++) {
                        for (Int32 InnerRow = 0; InnerRow < Pattern.GetLength(0); InnerRow++) {
                            if (Pattern[InnerRow, InnerColumn] == true) {
                                if (first == true) {
                                    ReferenceColor = Img[OuterColumn, OuterRow];
                                    first = false;
                                } else {
                                    if (CommonFunctions.ColorsSimilar(ReferenceColor, Img[OuterColumn + InnerColumn, OuterRow + InnerRow], Tolerance)) {
                                        // ok
                                    } else {
                                        // schlecht passt nicht
                                        InnerColumn = Pattern.GetLength(1) + 10;
                                        InnerRow = Pattern.GetLength(0) + 10;
                                        first = true;
                                        break;
                                    }
                                }
                            } else {
                                if (first == false) {
                                    // darf nicht passen!
                                    if (CommonFunctions.ColorsSimilar(ReferenceColor, Img[OuterColumn + InnerColumn, OuterRow + InnerRow], Tolerance)) {
                                        // schlecht passt
                                        InnerColumn = Img.Width + 10;
                                        InnerRow = Img.Height + 10;
                                        first = true;
                                    }
                                }
                            }
                        }
                    }
                    if (first == false) {
                        //matched
                        location.X = OuterColumn;
                        location.Y = OuterRow;
                        return new Rectangle(location.X, location.Y, Pattern.GetLength(1), Pattern.GetLength(0));
                    }
                }
            }

            return Rectangle.Empty;
        }


        /// <summary>
        /// Used in the picture search. if the color is /DC/ then it is skipped and accepted as true.
        /// </summary>
        public static readonly SolidBrush DontCareBrush = new SolidBrush(Color.FromArgb(163, 73, 164));


        /// <summary>
        /// search for an image in another image
        /// return the best matching position
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// ImageData Img = new ImageData(...);
        /// ImageData Search= new ImageData(...);
        /// // search with tolerance 25
        /// Rectangle Position = Template.Image(Img,Search,25);
        /// ]]>
        /// </code>
        /// </example>
        /// <param name="Img">image to look in</param>
        /// <param name="Ref">image to look for</param>
        /// <param name="Tolerance">tolerance of similarity (0,...,255)</param>
        /// <returns>best matching position as rectangle</returns>
        public static Rectangle Image(ImageData Img, ImageData Ref, uint Tolerance = 0) {
            Double bestScore = (Math.Abs(Byte.MaxValue - Byte.MinValue)*3);

            Int32 currentScore = 0;
            Color CurrentInnerPictureColor;
            Color CurrentOuterPictureColor;
            Boolean allSimilar = true;
            Point location = Point.Empty;
            Boolean Found = false;

            for (Int32 originalX = 0; originalX < Img.Width - Ref.Width; originalX++) {
                for (Int32 originalY = 0; originalY < Img.Height - Ref.Height; originalY++) {
                    CurrentInnerPictureColor = Ref[0, 0];
                    CurrentOuterPictureColor = Img[originalX, originalY];

                    if (CommonFunctions.ColorsSimilar(CurrentInnerPictureColor, CurrentOuterPictureColor, Tolerance)) {
                        currentScore = 0;
                        allSimilar = true;
                        for (Int32 referenceX = 0; referenceX < Ref.Width; referenceX++) {
                            if (!allSimilar)
                                break;
                            for (Int32 referenceY = 0; referenceY < Ref.Height; referenceY++) {
                                if (!allSimilar)
                                    break;
                                CurrentInnerPictureColor = Ref[referenceX, referenceY];
                                if (CurrentInnerPictureColor.Equals(DontCareBrush.Color)) //checks for /DC/ pixel
                                    continue;
                                CurrentOuterPictureColor = Img[originalX + referenceX, originalY + referenceY];

                                if (!CommonFunctions.ColorsSimilar(CurrentInnerPictureColor, CurrentOuterPictureColor, Tolerance))
                                    allSimilar = false;

                                currentScore += (Math.Abs(CurrentInnerPictureColor.R - CurrentOuterPictureColor.R) + Math.Abs(CurrentInnerPictureColor.G - CurrentOuterPictureColor.G) + Math.Abs(CurrentInnerPictureColor.B - CurrentOuterPictureColor.B));
                            }
                        }
                        if (allSimilar) {
                            if (((Double) currentScore/(Double) (Ref.Width*Ref.Height)) < bestScore) {
                                location.X = originalX;
                                location.Y = originalY;
                                bestScore = ((Double) currentScore/(Double) (Ref.Width*Ref.Height));
                                Found = true;
                            }
                        }
                    }
                }
            }
            if (Found)
                return new Rectangle(location.X, location.Y, Ref.Width, Ref.Height);
            else
                return Rectangle.Empty;
        }

        /// <summary>
        /// search for an image in another image
        /// return the best matching position
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// ImageData Img = new ImageData(...);
        /// ImageData Search= new ImageData(...);
        /// // search with tolerance 25
        /// Rectangle Position = Template.Image(Img,Search,25);
        /// ]]>
        /// </code>
        /// </example>
        /// <param name="Img">image to look in</param>
        /// <param name="Ref">image to look for</param>
        /// <param name="Tolerance">tolerance of similarity (0,...,255)</param>
        /// <returns>best matching position as rectangle</returns>
        public static Task<Rectangle> ImageAsync(ImageData Img, ImageData Ref, uint Tolerance = 0) {
            return Task.Run(() => Image(Img, Ref, Tolerance));
        }


        /// <summary>
        /// search for an image in another image
        /// return all possible matchings
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// ImageData Img = new ImageData(...);
        /// ImageData Search= new ImageData(...);
        /// // search with tolerance 25
        /// List<Rectangle> Positions = Template.AllImages(Img,Search,25);
        /// ]]>
        /// </code>
        /// </example>
        /// <param name="Img">image to look in</param>
        /// <param name="Ref">image to look for</param>
        /// <param name="Tolerance">tolerance of similarity (0,...,255)</param>
        /// <returns>List of matching positions (datatype Rectange)</returns>
        public static List<Rectangle> AllImages(ImageData Img, ImageData Ref, uint Tolerance = 0) {
            Double bestScore = (Math.Abs(Byte.MaxValue - Byte.MinValue)*3);
            List<Rectangle> RetVal = new List<Rectangle>();
            Int32 currentScore = 0;
            Color CurrentInnerPictureColor;
            Color CurrentOuterPictureColor;
            Boolean allSimilar = true;
            //Point location = Point.Empty;
            Boolean Found = false;
            var ToSkip = new List<Rectangle>();
            for (Int32 originalX = 0; originalX < Img.Width - Ref.Width; originalX++) {
                for (Int32 originalY = 0; originalY < Img.Height - Ref.Height; originalY++) {
                    CurrentInnerPictureColor = Ref[0, 0];
                    CurrentOuterPictureColor = Img[originalX, originalY];

                    if (CommonFunctions.ColorsSimilar(CurrentInnerPictureColor, CurrentOuterPictureColor, Tolerance)) {
                        currentScore = 0;
                        allSimilar = true;
                        for (Int32 referenceX = 0; referenceX < Ref.Width; referenceX++) {
                            for (Int32 referenceY = 0; referenceY < Ref.Height; referenceY++) {

                                

                                CurrentInnerPictureColor = Ref[referenceX, referenceY];
                                if (CurrentInnerPictureColor.Equals(DontCareBrush.Color)) //checks for /DC/ pixel
                                    continue;
                                CurrentOuterPictureColor = Img[originalX + referenceX, originalY + referenceY];

                                if (!CommonFunctions.ColorsSimilar(CurrentInnerPictureColor, CurrentOuterPictureColor, Tolerance))
                                    goto _continue_nextpixel_y;

                                currentScore += (Math.Abs(CurrentInnerPictureColor.R - CurrentOuterPictureColor.R) + Math.Abs(CurrentInnerPictureColor.G - CurrentOuterPictureColor.G) + Math.Abs(CurrentInnerPictureColor.B - CurrentOuterPictureColor.B));
                            }
                        }

                        if (allSimilar) {
                            var rec = new Rectangle(originalX, originalY, Ref.Width, Ref.Height);
                            RetVal.Add(rec);
                            ToSkip.Add(rec);
                        
                        }
                        
                    }
                _continue_nextpixel_y:
                        if (false) ;
                }
                _continue_rectintersect_x:
                        if (false) ;
            }
            return RetVal;
        }

        /// <summary>
        /// Find the location of the the first found <paramref name="needle"/>s inside of <paramref name="haystack"/>
        /// </summary>
        /// <param name="haystack">The image that is suppose to contain <paramref name="needle"/></param>
        /// <param name="needle">The image that is suppose to be contained inside of <paramref name="haystack"/></param>
        /// <param name="tolerance">Tolerance about the difference from the supposed pixel color to the found. higher -> less accurate</param>
        /// <param name="dir">The method for seeking the <paramref name="needle"/></param>
        /// <param name="args">The arguments that are needed to be passed to perform the <paramref name="dir"/>(<see cref="SeekMethod"/>) 
        /// - see the requirement in the documentation for each <see cref="SeekMethod"/></param>
        public static Rectangle Pinpoint(ImageData haystack, ImageData needle, uint tolerance, SeekMethod dir, params object[] args) {
            return PinpointMany(haystack, needle, tolerance, dir, args).FirstOrDefault();
        }

        /// <summary>
        /// Finds the locations of the the <paramref name="needle"/>s inside of <paramref name="haystack"/>
        /// </summary>
        /// <param name="haystack">The image that is suppose to contain <paramref name="needle"/></param>
        /// <param name="needle">The image that is suppose to be contained inside of <paramref name="haystack"/></param>
        /// <param name="tolerance">Tolerance about the difference from the supposed pixel color to the found. higher -> less accurate</param>
        /// <param name="dir">The method for seeking the <paramref name="needle"/></param>
        /// <param name="args">The arguments that are needed to be passed to perform the <paramref name="dir"/>(<see cref="SeekMethod"/>) 
        /// - see the requirement in the documentation for each <see cref="SeekMethod"/></param>
        public static IEnumerable<Rectangle> PinpointMany(ImageData haystack, ImageData needle, uint tolerance, SeekMethod dir, params object[] args) {
            if (needle.Width > haystack.Width || needle.Height > haystack.Height)
                yield break;
            //var _enum = _populate(haystack.Size, needle.Size, dir, args);

                foreach (var o in _populate(haystack.Size, needle.Size, dir, args)) { //populates the pixel points using IEnumerable technique.
                    Color CurrentInnerPictureColor = needle[0, 0];
                    Color CurrentOuterPictureColor = haystack[o.X, o.Y];
                    if (CommonFunctions.ColorsSimilar(CurrentInnerPictureColor, CurrentOuterPictureColor, tolerance)) {
                            for (Int32 referenceX = 0; referenceX < needle.Width; referenceX++) {
                                for (Int32 referenceY = 0; referenceY < needle.Height; referenceY++) {
                                    CurrentInnerPictureColor = needle[referenceX, referenceY];
                                    if (CurrentInnerPictureColor.Equals(DontCareBrush.Color)) //checks for /DC/ pixel
                                        continue;
                                    CurrentOuterPictureColor = haystack[o.X + referenceX, o.Y + referenceY];
                                    if (!CommonFunctions.ColorsSimilar(CurrentInnerPictureColor, CurrentOuterPictureColor, tolerance))
                                        goto _next;
                                }
                            }
                            //all seem to fit
                            yield return new Rectangle(o.X, o.Y, needle.Width, needle.Height);
                        }
                    _next:
                    continue;
                }
            
        }

        private static IEnumerable<Point> _populate(Size frame, Size box, SeekMethod dir, params object[] args) {
            int x = 0, y = 0, expected = 0;
            Point start;
            switch (dir) {
            case SeekMethod.Spiral:

            start = args.Length == 1 && args[0] is Point 
                ? (Point)args[0] 
                : new Point(Convert.ToInt32(Math.Floor(frame.Width/2d)), Convert.ToInt32(Math.Floor(frame.Height/2d)));
                    
            foreach (var point in Spiral(frame, start)) {
                yield return point;
            }

            break;

            case SeekMethod.Regular:
            for (; x < frame.Width-box.Width; x++) {
                for (; y < frame.Height - box.Height; y++) {
                    yield return new Point(x,y);
                }
            }
            break;

            case SeekMethod.HalfscreenLeftFromPoint:
                throw new NotImplementedException();
            break;
            case SeekMethod.HalfscreenRightFromPoint:
                throw new NotImplementedException();
            break;
            
            case SeekMethod.Antenna:
                throw new NotImplementedException();
            break;
            default:
            throw new ArgumentOutOfRangeException("dir");
        }
        }

        /// <summary>
        /// Populates the order of search.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="startlocation"></param>
        /// <returns></returns>
        public static IEnumerable<Point> Spiral(Size size, Point startlocation) {
            var X = Math.Max(size.Width - startlocation.X, startlocation.X) * 2;
            var Y = Math.Max(startlocation.Y, size.Height - startlocation.Y) * 2;
            int x,y,dx,dy;
            var t = Math.Max(X, Y);
            var maxI = t * t;

            x = y = dx = 0;
            dy = -1;
            for(var i = 0; i < maxI; i++){
                if ((-X/2 <= x) && (x <= X/2) && (-Y/2 <= y) && (y <= Y/2)) {
                    if (startlocation.X - 1 + x >= 0 && startlocation.Y - 1 + y >= 0 && startlocation.X - 1 + x < size.Width && startlocation.Y - 1 + y < size.Height)
                        yield return new Point(startlocation.X + x,startlocation.Y + y);
                }
                if( (x == y) || ((x < 0) && (x == -y)) || ((x > 0) && (x == 1-y))){
                    t = dx;
                    dx = -dy;
                    dy = t;
                }
                x += dx;
                y += dy;
            }
            for (int i = size.Height; i >= 0; i--) 
                yield return new Point(0, i);
            for (int i = 1; i < size.Width; i++)
                yield return new Point(i, 0);
        }
    }

    public enum SeekMethod {
        /// <summary>
        /// Pass a <see cref="Point"/> to show the where to begin the spiral, otherwise it will calculate the center and begin there.
        /// </summary>
        Spiral,
        /// <summary>
        /// Pass a number (<see cref="Int32"/>) that will point the y that will make the split
        /// </summary>
        HalfscreenLeftFromPoint,
        /// <summary>
        /// Pass a number (<see cref="Int32"/>) that will point the y that will make the split
        /// </summary>
        HalfscreenRightFromPoint,
        /// <summary>
        /// From 0,0 to Right, Bottom
        /// </summary>
        Regular,
        /// <summary>
        /// Hard to explain, but see this: http://i.imgur.com/hF1lrsv.jpg , also pass a <see cref="Direction"/> objects to show where to go
        /// if <see cref="Direction.Up"/> is passed, then 1 will be sent up from center and 1 down.
        /// </summary>
        Antenna
    }

    public enum Direction {
        Up,
        Down,
        Left,
        Right
    }
}