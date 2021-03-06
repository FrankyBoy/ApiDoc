﻿/*
 * Copyright 2008 Google Inc. All Rights Reserved.
 * Author: fraser@google.com (Neil Fraser)
 * Author: anteru@developer.shelter13.net (Matthaeus G. Chajdas)
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 * Diff Match and Patch
 * http://code.google.com/p/google-diff-match-patch/
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace ApiDoc.Utility.External
{
    internal static class CompatibilityExtensions
    {
        // JScript splice function
        public static List<T> Splice<T>(this List<T> input, int start, int count,
            params T[] objects)
        {
            List<T> deletedRange = input.GetRange(start, count);
            input.RemoveRange(start, count);
            input.InsertRange(start, objects);

            return deletedRange;
        }

        // Java substring function
        public static string JavaSubstring(this string s, int begin, int end)
        {
            return s.Substring(begin, end - begin);
        }
    }

    /**-
     * The data structure representing a diff is a List of Diff objects:
     * {Diff(Operation.DELETE, "Hello"), Diff(Operation.INSERT, "Goodbye"),
     *  Diff(Operation.EQUAL, " world.")}
     * which means: delete "Hello", add "Goodbye" and keep " world."
     */
    public enum Operation
    {
        // ReSharper disable InconsistentNaming
        DELETE, INSERT, EQUAL
        // ReSharper restore InconsistentNaming
    }


    /**
     * Class representing one diff operation.
     */
    public class Diff
    {
        public Operation Operation;
        // One of: INSERT, DELETE or EQUAL.
        public string Text;
        // The text associated with this diff operation.

        /**
         * Constructor.  Initializes the diff with the provided values.
         * @param operation One of INSERT, DELETE or EQUAL.
         * @param text The text being applied.
         */
        public Diff(Operation operation, string text)
        {
            // Construct a diff with the specified operation and text.
            Operation = operation;
            Text = text;
        }

        /**
         * Display a human-readable version of this Diff.
         * @return text version.
         */
        public override string ToString()
        {
            string prettyText = Text.Replace('\n', '\u00b6');
            return "Diff(" + Operation + ",\"" + prettyText + "\")";
        }

        /**
         * Is this Diff equivalent to another Diff?
         * @param d Another Diff to compare against.
         * @return true or false.
         */
        public override bool Equals(Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Diff return false.
            var p = obj as Diff;
            if (p == null)
            {
                return false;
            }

            // Return true if the fields match.
            return p.Operation == Operation && p.Text == Text;
        }

        public bool Equals(Diff obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // Return true if the fields match.
            return obj.Operation == Operation && obj.Text == Text;
        }

        public override int GetHashCode()
        {
// ReSharper disable NonReadonlyFieldInGetHashCode
            return Text.GetHashCode() ^ Operation.GetHashCode();
// ReSharper restore NonReadonlyFieldInGetHashCode
        }
    }


    /**
     * Class representing one patch operation.
     */
    public class Patch
    {
        public List<Diff> Diffs = new List<Diff>();
        public int Start1;
        public int Start2;
        public int Length1;
        public int Length2;

        /**
         * Emmulate GNU diff's format.
         * Header: @@ -382,8 +481,9 @@
         * Indicies are printed as 1-based, not 0-based.
         * @return The GNU diff string.
         */
        public override string ToString()
        {
            string coords1, coords2;
            if (Length1 == 0)
            {
                coords1 = Start1 + ",0";
            }
            else if (Length1 == 1)
            {
                coords1 = Convert.ToString(Start1 + 1);
            }
            else
            {
                coords1 = (Start1 + 1) + "," + Length1;
            }
            if (Length2 == 0)
            {
                coords2 = Start2 + ",0";
            }
            else if (Length2 == 1)
            {
                coords2 = Convert.ToString(Start2 + 1);
            }
            else
            {
                coords2 = (Start2 + 1) + "," + Length2;
            }
            var text = new StringBuilder();
            text.Append("@@ -").Append(coords1).Append(" +").Append(coords2)
                .Append(" @@\n");
            // Escape the body of the patch with %xx notation.
            foreach (Diff aDiff in Diffs)
            {
                switch (aDiff.Operation)
                {
                    case Operation.INSERT:
                        text.Append('+');
                        break;
                    case Operation.DELETE:
                        text.Append('-');
                        break;
                    case Operation.EQUAL:
                        text.Append(' ');
                        break;
                }

                var encoded = HttpUtility.UrlEncode(aDiff.Text, new UTF8Encoding()) ?? "";
                text.Append(encoded.Replace('+', ' ')).Append("\n");
            }

            return DiffMatchPatch.UnescapeForEncodeUriCompatability(
                text.ToString());
        }
    }


    /**
     * Class containing the diff, match and patch methods.
     * Also Contains the behaviour settings.
     */
    public class DiffMatchPatch
    {
        // Defaults.
        // Set these on your diff_match_patch instance to override the defaults.

        // Number of seconds to map a diff before giving up (0 for infinity).
        public float DiffTimeout = 1.0f;
        // Cost of an empty edit operation in terms of edit characters.
        public short DiffEditCost = 4;
        // At what point is no match declared (0.0 = perfection, 1.0 = very loose).
        public float MatchThreshold = 0.5f;
        // How far to search for a match (0 = exact location, 1000+ = broad match).
        // A match this many characters away from the expected location will add
        // 1.0 to the score (0.0 is a perfect match).
        public int MatchDistance = 1000;
        // When deleting a large block of text (over ~64 characters), how close
        // do the contents have to be to match the expected contents. (0.0 =
        // perfection, 1.0 = very loose).  Note that Match_Threshold controls
        // how closely the end points of a delete need to match.
        public float PatchDeleteThreshold = 0.5f;
        // Chunk size for context length.
        public short PatchMargin = 4;

        // The number of bits in an int.
        private const short MatchMaxBits = 32;


        //  DIFF FUNCTIONS


        /**
         * Find the differences between two texts.
         * Run a faster, slightly less optimal diff.
         * This method allows the 'checklines' of diff_main() to be optional.
         * Most of the time checklines is wanted, so default to true.
         * @param text1 Old string to be diffed.
         * @param text2 New string to be diffed.
         * @return List of Diff objects.
         */
        public List<Diff> diff_main(string text1, string text2)
        {
            return diff_main(text1, text2, true);
        }

        /**
         * Find the differences between two texts.
         * @param text1 Old string to be diffed.
         * @param text2 New string to be diffed.
         * @param checklines Speedup flag.  If false, then don't run a
         *     line-level diff first to identify the changed areas.
         *     If true, then run a faster slightly less optimal diff.
         * @return List of Diff objects.
         */
        public List<Diff> diff_main(string text1, string text2, bool checklines)
        {
            // Set a deadline by which time the diff must be complete.
            DateTime deadline;
            if (DiffTimeout <= 0)
            {
                deadline = DateTime.MaxValue;
            }
            else
            {
                deadline = DateTime.Now +
                    new TimeSpan(((long)(DiffTimeout * 1000)) * 10000);
            }
            return diff_main(text1, text2, checklines, deadline);
        }

        /**
         * Find the differences between two texts.  Simplifies the problem by
         * stripping any common prefix or suffix off the texts before diffing.
         * @param text1 Old string to be diffed.
         * @param text2 New string to be diffed.
         * @param checklines Speedup flag.  If false, then don't run a
         *     line-level diff first to identify the changed areas.
         *     If true, then run a faster slightly less optimal diff.
         * @param deadline Time when the diff should be complete by.  Used
         *     internally for recursive calls.  Users should set DiffTimeout
         *     instead.
         * @return List of Diff objects.
         */
        private List<Diff> diff_main(string text1, string text2, bool checklines,
            DateTime deadline)
        {
            // Check for null inputs not needed since null can't be passed in C#.

            // Check for equality (speedup).
            List<Diff> diffs;
            if (text1 == text2)
            {
                diffs = new List<Diff>();
                if (text1.Length != 0)
                {
                    diffs.Add(new Diff(Operation.EQUAL, text1));
                }
                return diffs;
            }

            // Trim off common prefix (speedup).
            int commonlength = diff_commonPrefix(text1, text2);
            string commonprefix = text1.Substring(0, commonlength);
            text1 = text1.Substring(commonlength);
            text2 = text2.Substring(commonlength);

            // Trim off common suffix (speedup).
            commonlength = diff_commonSuffix(text1, text2);
            string commonsuffix = text1.Substring(text1.Length - commonlength);
            text1 = text1.Substring(0, text1.Length - commonlength);
            text2 = text2.Substring(0, text2.Length - commonlength);

            // Compute the diff on the middle block.
            diffs = diff_compute(text1, text2, checklines, deadline);

            // Restore the prefix and suffix.
            if (commonprefix.Length != 0)
            {
                diffs.Insert(0, (new Diff(Operation.EQUAL, commonprefix)));
            }
            if (commonsuffix.Length != 0)
            {
                diffs.Add(new Diff(Operation.EQUAL, commonsuffix));
            }

            diff_cleanupMerge(diffs);
            return diffs;
        }

        /**
         * Find the differences between two texts.  Assumes that the texts do not
         * have any common prefix or suffix.
         * @param text1 Old string to be diffed.
         * @param text2 New string to be diffed.
         * @param checklines Speedup flag.  If false, then don't run a
         *     line-level diff first to identify the changed areas.
         *     If true, then run a faster slightly less optimal diff.
         * @param deadline Time when the diff should be complete by.
         * @return List of Diff objects.
         */
        private List<Diff> diff_compute(string text1, string text2,
                                        bool checklines, DateTime deadline)
        {
            var diffs = new List<Diff>();

            if (text1.Length == 0)
            {
                // Just add some text (speedup).
                diffs.Add(new Diff(Operation.INSERT, text2));
                return diffs;
            }

            if (text2.Length == 0)
            {
                // Just delete some text (speedup).
                diffs.Add(new Diff(Operation.DELETE, text1));
                return diffs;
            }

            string longtext = text1.Length > text2.Length ? text1 : text2;
            string shorttext = text1.Length > text2.Length ? text2 : text1;
            int i = longtext.IndexOf(shorttext, StringComparison.Ordinal);
            if (i != -1)
            {
                // Shorter text is inside the longer text (speedup).
                Operation op = (text1.Length > text2.Length) ?
                    Operation.DELETE : Operation.INSERT;
                diffs.Add(new Diff(op, longtext.Substring(0, i)));
                diffs.Add(new Diff(Operation.EQUAL, shorttext));
                diffs.Add(new Diff(op, longtext.Substring(i + shorttext.Length)));
                return diffs;
            }

            if (shorttext.Length == 1)
            {
                // Single character string.
                // After the previous speedup, the character can't be an equality.
                diffs.Add(new Diff(Operation.DELETE, text1));
                diffs.Add(new Diff(Operation.INSERT, text2));
                return diffs;
            }

            // Check to see if the problem can be split in two.
            string[] hm = diff_halfMatch(text1, text2);
            if (hm != null)
            {
                // A half-match was found, sort out the return data.
                string text1A = hm[0];
                string text1B = hm[1];
                string text2A = hm[2];
                string text2B = hm[3];
                string midCommon = hm[4];
                // Send both pairs off for separate processing.
                List<Diff> diffsA = diff_main(text1A, text2A, checklines, deadline);
                List<Diff> diffsB = diff_main(text1B, text2B, checklines, deadline);
                // Merge the results.
                diffs = diffsA;
                diffs.Add(new Diff(Operation.EQUAL, midCommon));
                diffs.AddRange(diffsB);
                return diffs;
            }

            if (checklines && text1.Length > 100 && text2.Length > 100)
            {
                return diff_lineMode(text1, text2, deadline);
            }

            return diff_bisect(text1, text2, deadline);
        }

        /**
         * Do a quick line-level diff on both strings, then rediff the parts for
         * greater accuracy.
         * This speedup can produce non-minimal diffs.
         * @param text1 Old string to be diffed.
         * @param text2 New string to be diffed.
         * @param deadline Time when the diff should be complete by.
         * @return List of Diff objects.
         */
        private List<Diff> diff_lineMode(string text1, string text2,
                                         DateTime deadline)
        {
            // Scan the text on a line-by-line basis first.
            Object[] b = diff_linesToChars(text1, text2);
            text1 = (string)b[0];
            text2 = (string)b[1];
            var linearray = (List<string>)b[2];

            List<Diff> diffs = diff_main(text1, text2, false, deadline);

            // Convert the diff back to original text.
            diff_charsToLines(diffs, linearray);
            // Eliminate freak matches (e.g. blank lines)
            diff_cleanupSemantic(diffs);

            // Rediff any replacement blocks, this time character-by-character.
            // Add a dummy entry at the end.
            diffs.Add(new Diff(Operation.EQUAL, string.Empty));
            int pointer = 0;
            int countDelete = 0;
            int countInsert = 0;
            string textDelete = string.Empty;
            string textInsert = string.Empty;
            while (pointer < diffs.Count)
            {
                switch (diffs[pointer].Operation)
                {
                    case Operation.INSERT:
                        countInsert++;
                        textInsert += diffs[pointer].Text;
                        break;
                    case Operation.DELETE:
                        countDelete++;
                        textDelete += diffs[pointer].Text;
                        break;
                    case Operation.EQUAL:
                        // Upon reaching an equality, check for prior redundancies.
                        if (countDelete >= 1 && countInsert >= 1)
                        {
                            // Delete the offending records and add the merged ones.
                            diffs.RemoveRange(pointer - countDelete - countInsert,
                                countDelete + countInsert);
                            pointer = pointer - countDelete - countInsert;
                            List<Diff> a =
                                diff_main(textDelete, textInsert, false, deadline);
                            diffs.InsertRange(pointer, a);
                            pointer = pointer + a.Count;
                        }
                        countInsert = 0;
                        countDelete = 0;
                        textDelete = string.Empty;
                        textInsert = string.Empty;
                        break;
                }
                pointer++;
            }
            diffs.RemoveAt(diffs.Count - 1);  // Remove the dummy entry at the end.

            return diffs;
        }

        /**
         * Find the 'middle snake' of a diff, split the problem in two
         * and return the recursively constructed diff.
         * See Myers 1986 paper: An O(ND) Difference Algorithm and Its Variations.
         * @param text1 Old string to be diffed.
         * @param text2 New string to be diffed.
         * @param deadline Time at which to bail if not yet complete.
         * @return List of Diff objects.
         */
        protected List<Diff> diff_bisect(string text1, string text2,
            DateTime deadline)
        {
            // Cache the text lengths to prevent multiple calls.
            int text1Length = text1.Length;
            int text2Length = text2.Length;
            int maxD = (text1Length + text2Length + 1) / 2;
            int vOffset = maxD;
            int vLength = 2 * maxD;
            var v1 = new int[vLength];
            var v2 = new int[vLength];
            for (int x = 0; x < vLength; x++)
            {
                v1[x] = -1;
                v2[x] = -1;
            }
            v1[vOffset + 1] = 0;
            v2[vOffset + 1] = 0;
            int delta = text1Length - text2Length;
            // If the total number of characters is odd, then the front path will
            // collide with the reverse path.
            bool front = (delta % 2 != 0);
            // Offsets for start and end of k loop.
            // Prevents mapping of space beyond the grid.
            int k1Start = 0;
            int k1End = 0;
            int k2Start = 0;
            int k2End = 0;
            for (int d = 0; d < maxD; d++)
            {
                // Bail out if deadline is reached.
                if (DateTime.Now > deadline)
                {
                    break;
                }

                // Walk the front path one step.
                for (int k1 = -d + k1Start; k1 <= d - k1End; k1 += 2)
                {
                    int k1Offset = vOffset + k1;
                    int x1;
                    if (k1 == -d || k1 != d && v1[k1Offset - 1] < v1[k1Offset + 1])
                    {
                        x1 = v1[k1Offset + 1];
                    }
                    else
                    {
                        x1 = v1[k1Offset - 1] + 1;
                    }
                    int y1 = x1 - k1;
                    while (x1 < text1Length && y1 < text2Length
                          && text1[x1] == text2[y1])
                    {
                        x1++;
                        y1++;
                    }
                    v1[k1Offset] = x1;
                    if (x1 > text1Length)
                    {
                        // Ran off the right of the graph.
                        k1End += 2;
                    }
                    else if (y1 > text2Length)
                    {
                        // Ran off the bottom of the graph.
                        k1Start += 2;
                    }
                    else if (front)
                    {
                        int k2Offset = vOffset + delta - k1;
                        if (k2Offset >= 0 && k2Offset < vLength && v2[k2Offset] != -1)
                        {
                            // Mirror x2 onto top-left coordinate system.
                            int x2 = text1Length - v2[k2Offset];
                            if (x1 >= x2)
                            {
                                // Overlap detected.
                                return diff_bisectSplit(text1, text2, x1, y1, deadline);
                            }
                        }
                    }
                }

                // Walk the reverse path one step.
                for (int k2 = -d + k2Start; k2 <= d - k2End; k2 += 2)
                {
                    int k2Offset = vOffset + k2;
                    int x2;
                    if (k2 == -d || k2 != d && v2[k2Offset - 1] < v2[k2Offset + 1])
                    {
                        x2 = v2[k2Offset + 1];
                    }
                    else
                    {
                        x2 = v2[k2Offset - 1] + 1;
                    }
                    int y2 = x2 - k2;
                    while (x2 < text1Length && y2 < text2Length
                        && text1[text1Length - x2 - 1]
                        == text2[text2Length - y2 - 1])
                    {
                        x2++;
                        y2++;
                    }
                    v2[k2Offset] = x2;
                    if (x2 > text1Length)
                    {
                        // Ran off the left of the graph.
                        k2End += 2;
                    }
                    else if (y2 > text2Length)
                    {
                        // Ran off the top of the graph.
                        k2Start += 2;
                    }
                    else if (!front)
                    {
                        int k1Offset = vOffset + delta - k2;
                        if (k1Offset >= 0 && k1Offset < vLength && v1[k1Offset] != -1)
                        {
                            int x1 = v1[k1Offset];
                            int y1 = vOffset + x1 - k1Offset;
                            // Mirror x2 onto top-left coordinate system.
                            x2 = text1Length - v2[k2Offset];
                            if (x1 >= x2)
                            {
                                // Overlap detected.
                                return diff_bisectSplit(text1, text2, x1, y1, deadline);
                            }
                        }
                    }
                }
            }
            // Diff took too long and hit the deadline or
            // number of diffs equals number of characters, no commonality at all.
            var diffs = new List<Diff> { new Diff(Operation.DELETE, text1), new Diff(Operation.INSERT, text2) };
            return diffs;
        }

        /**
         * Given the location of the 'middle snake', split the diff in two parts
         * and recurse.
         * @param text1 Old string to be diffed.
         * @param text2 New string to be diffed.
         * @param x Index of split point in text1.
         * @param y Index of split point in text2.
         * @param deadline Time at which to bail if not yet complete.
         * @return LinkedList of Diff objects.
         */
        private List<Diff> diff_bisectSplit(string text1, string text2,
            int x, int y, DateTime deadline)
        {
            string text1A = text1.Substring(0, x);
            string text2A = text2.Substring(0, y);
            string text1B = text1.Substring(x);
            string text2B = text2.Substring(y);

            // Compute both diffs serially.
            List<Diff> diffs = diff_main(text1A, text2A, false, deadline);
            List<Diff> diffsb = diff_main(text1B, text2B, false, deadline);

            diffs.AddRange(diffsb);
            return diffs;
        }

        /**
         * Split two texts into a list of strings.  Reduce the texts to a string of
         * hashes where each Unicode character represents one line.
         * @param text1 First string.
         * @param text2 Second string.
         * @return Three element Object array, containing the encoded text1, the
         *     encoded text2 and the List of unique strings.  The zeroth element
         *     of the List of unique strings is intentionally blank.
         */
        protected Object[] diff_linesToChars(string text1, string text2)
        {
            var lineArray = new List<string>();
            var lineHash = new Dictionary<string, int>();
            // e.g. linearray[4] == "Hello\n"
            // e.g. linehash.get("Hello\n") == 4

            // "\x00" is a valid character, but various debuggers don't like it.
            // So we'll insert a junk entry to avoid generating a null character.
            lineArray.Add(string.Empty);

            string chars1 = diff_linesToCharsMunge(text1, lineArray, lineHash);
            string chars2 = diff_linesToCharsMunge(text2, lineArray, lineHash);
            return new Object[] { chars1, chars2, lineArray };
        }

        /**
         * Split a text into a list of strings.  Reduce the texts to a string of
         * hashes where each Unicode character represents one line.
         * @param text String to encode.
         * @param lineArray List of unique strings.
         * @param lineHash Map of strings to indices.
         * @return Encoded string.
         */
        private static string diff_linesToCharsMunge(string text, List<string> lineArray,
                                              Dictionary<string, int> lineHash)
        {
            int lineStart = 0;
            int lineEnd = -1;
            var chars = new StringBuilder();
            // Walk the text, pulling out a Substring for each line.
            // text.split('\n') would would temporarily double our memory footprint.
            // Modifying text would create many large strings to garbage collect.
            while (lineEnd < text.Length - 1)
            {
                lineEnd = text.IndexOf('\n', lineStart);
                if (lineEnd == -1)
                {
                    lineEnd = text.Length - 1;
                }
                string line = text.JavaSubstring(lineStart, lineEnd + 1);
                lineStart = lineEnd + 1;

                if (lineHash.ContainsKey(line))
                {
                    chars.Append(((char)lineHash[line]));
                }
                else
                {
                    lineArray.Add(line);
                    lineHash.Add(line, lineArray.Count - 1);
                    chars.Append(((char)(lineArray.Count - 1)));
                }
            }
            return chars.ToString();
        }

        /**
         * Rehydrate the text in a diff from a string of line hashes to real lines
         * of text.
         * @param diffs List of Diff objects.
         * @param lineArray List of unique strings.
         */
        protected void diff_charsToLines(ICollection<Diff> diffs,
                        List<string> lineArray)
        {
            foreach (Diff diff in diffs)
            {
                var text = new StringBuilder();
                foreach (char t in diff.Text)
                {
                    text.Append(lineArray[t]);
                }
                diff.Text = text.ToString();
            }
        }

        /**
         * Determine the common prefix of two strings.
         * @param text1 First string.
         * @param text2 Second string.
         * @return The number of characters common to the start of each string.
         */
        public int diff_commonPrefix(string text1, string text2)
        {
            // Performance analysis: http://neil.fraser.name/news/2007/10/09/
            int n = Math.Min(text1.Length, text2.Length);
            for (int i = 0; i < n; i++)
            {
                if (text1[i] != text2[i])
                {
                    return i;
                }
            }
            return n;
        }

        /**
         * Determine the common suffix of two strings.
         * @param text1 First string.
         * @param text2 Second string.
         * @return The number of characters common to the end of each string.
         */
        public int diff_commonSuffix(string text1, string text2)
        {
            // Performance analysis: http://neil.fraser.name/news/2007/10/09/
            int text1Length = text1.Length;
            int text2Length = text2.Length;
            int n = Math.Min(text1.Length, text2.Length);
            for (int i = 1; i <= n; i++)
            {
                if (text1[text1Length - i] != text2[text2Length - i])
                {
                    return i - 1;
                }
            }
            return n;
        }

        /**
         * Determine if the suffix of one string is the prefix of another.
         * @param text1 First string.
         * @param text2 Second string.
         * @return The number of characters common to the end of the first
         *     string and the start of the second string.
         */
        protected int diff_commonOverlap(string text1, string text2)
        {
            // Cache the text lengths to prevent multiple calls.
            int text1Length = text1.Length;
            int text2Length = text2.Length;
            // Eliminate the null case.
            if (text1Length == 0 || text2Length == 0)
            {
                return 0;
            }
            // Truncate the longer string.
            if (text1Length > text2Length)
            {
                text1 = text1.Substring(text1Length - text2Length);
            }
            else if (text1Length < text2Length)
            {
                text2 = text2.Substring(0, text1Length);
            }
            int textLength = Math.Min(text1Length, text2Length);
            // Quick check for the worst case.
            if (text1 == text2)
            {
                return textLength;
            }

            // Start by looking for a single character match
            // and increase length until no match is found.
            // Performance analysis: http://neil.fraser.name/news/2010/11/04/
            int best = 0;
            int length = 1;
            while (true)
            {
                string pattern = text1.Substring(textLength - length);
                int found = text2.IndexOf(pattern, StringComparison.Ordinal);
                if (found == -1)
                {
                    return best;
                }
                length += found;
                if (found == 0 || text1.Substring(textLength - length) ==
                    text2.Substring(0, length))
                {
                    best = length;
                    length++;
                }
            }
        }

        /**
         * Do the two texts share a Substring which is at least half the length of
         * the longer text?
         * This speedup can produce non-minimal diffs.
         * @param text1 First string.
         * @param text2 Second string.
         * @return Five element String array, containing the prefix of text1, the
         *     suffix of text1, the prefix of text2, the suffix of text2 and the
         *     common middle.  Or null if there was no match.
         */

        protected string[] diff_halfMatch(string text1, string text2)
        {
            if (DiffTimeout <= 0)
            {
                // Don't risk returning a non-optimal diff if we have unlimited time.
                return null;
            }
            string longtext = text1.Length > text2.Length ? text1 : text2;
            string shorttext = text1.Length > text2.Length ? text2 : text1;
            if (longtext.Length < 4 || shorttext.Length * 2 < longtext.Length)
            {
                return null;  // Pointless.
            }

            // First check if the second quarter is the seed for a half-match.
            string[] hm1 = diff_halfMatchI(longtext, shorttext,
                                           (longtext.Length + 3) / 4);
            // Check again based on the third quarter.
            string[] hm2 = diff_halfMatchI(longtext, shorttext,
                                           (longtext.Length + 1) / 2);
            string[] hm;
            if (hm1 == null && hm2 == null)
            {
                return null;
            }
            if (hm2 == null)
            {
                hm = hm1;
            }
            else if (hm1 == null)
            {
                hm = hm2;
            }
            else
            {
                // Both matched.  Select the longest.
                hm = hm1[4].Length > hm2[4].Length ? hm1 : hm2;
            }

            // A half-match was found, sort out the return data.
            if (text1.Length > text2.Length)
            {
                return hm;
                //return new string[]{hm[0], hm[1], hm[2], hm[3], hm[4]};
            }
            return new[] { hm[2], hm[3], hm[0], hm[1], hm[4] };
        }

        /**
         * Does a Substring of shorttext exist within longtext such that the
         * Substring is at least half the length of longtext?
         * @param longtext Longer string.
         * @param shorttext Shorter string.
         * @param i Start index of quarter length Substring within longtext.
         * @return Five element string array, containing the prefix of longtext, the
         *     suffix of longtext, the prefix of shorttext, the suffix of shorttext
         *     and the common middle.  Or null if there was no match.
         */
        private string[] diff_halfMatchI(string longtext, string shorttext, int i)
        {
            // Start with a 1/4 length Substring at position i as a seed.
            string seed = longtext.Substring(i, longtext.Length / 4);
            int j = -1;
            string bestCommon = string.Empty;
            string bestLongtextA = string.Empty, bestLongtextB = string.Empty;
            string bestShorttextA = string.Empty, bestShorttextB = string.Empty;
            while (j < shorttext.Length && (j = shorttext.IndexOf(seed, j + 1,
                StringComparison.Ordinal)) != -1)
            {
                int prefixLength = diff_commonPrefix(longtext.Substring(i),
                                                     shorttext.Substring(j));
                int suffixLength = diff_commonSuffix(longtext.Substring(0, i),
                                                     shorttext.Substring(0, j));
                if (bestCommon.Length < suffixLength + prefixLength)
                {
                    bestCommon = shorttext.Substring(j - suffixLength, suffixLength)
                        + shorttext.Substring(j, prefixLength);
                    bestLongtextA = longtext.Substring(0, i - suffixLength);
                    bestLongtextB = longtext.Substring(i + prefixLength);
                    bestShorttextA = shorttext.Substring(0, j - suffixLength);
                    bestShorttextB = shorttext.Substring(j + prefixLength);
                }
            }
            if (bestCommon.Length * 2 >= longtext.Length)
            {
                return new[]{bestLongtextA, bestLongtextB,
            bestShorttextA, bestShorttextB, bestCommon};
            }
            return null;
        }

        /**
         * Reduce the number of edits by eliminating semantically trivial
         * equalities.
         * @param diffs List of Diff objects.
         */
        public void diff_cleanupSemantic(List<Diff> diffs)
        {
            bool changes = false;
            // Stack of indices where equalities are found.
            var equalities = new Stack<int>();
            // Always equal to equalities[equalitiesLength-1][1]
            string lastequality = null;
            int pointer = 0;  // Index of current position.
            // Number of characters that changed prior to the equality.
            int lengthInsertions1 = 0;
            int lengthDeletions1 = 0;
            // Number of characters that changed after the equality.
            int lengthInsertions2 = 0;
            int lengthDeletions2 = 0;
            while (pointer < diffs.Count)
            {
                if (diffs[pointer].Operation == Operation.EQUAL)
                {  // Equality found.
                    equalities.Push(pointer);
                    lengthInsertions1 = lengthInsertions2;
                    lengthDeletions1 = lengthDeletions2;
                    lengthInsertions2 = 0;
                    lengthDeletions2 = 0;
                    lastequality = diffs[pointer].Text;
                }
                else
                {  // an insertion or deletion
                    if (diffs[pointer].Operation == Operation.INSERT)
                    {
                        lengthInsertions2 += diffs[pointer].Text.Length;
                    }
                    else
                    {
                        lengthDeletions2 += diffs[pointer].Text.Length;
                    }
                    // Eliminate an equality that is smaller or equal to the edits on both
                    // sides of it.
                    if (lastequality != null && (lastequality.Length
                        <= Math.Max(lengthInsertions1, lengthDeletions1))
                        && (lastequality.Length
                            <= Math.Max(lengthInsertions2, lengthDeletions2)))
                    {
                        // Duplicate record.
                        diffs.Insert(equalities.Peek(),
                                     new Diff(Operation.DELETE, lastequality));
                        // Change second copy to insert.
                        diffs[equalities.Peek() + 1].Operation = Operation.INSERT;
                        // Throw away the equality we just deleted.
                        equalities.Pop();
                        if (equalities.Count > 0)
                        {
                            equalities.Pop();
                        }
                        pointer = equalities.Count > 0 ? equalities.Peek() : -1;
                        lengthInsertions1 = 0;  // Reset the counters.
                        lengthDeletions1 = 0;
                        lengthInsertions2 = 0;
                        lengthDeletions2 = 0;
                        lastequality = null;
                        changes = true;
                    }
                }
                pointer++;
            }

            // Normalize the diff.
            if (changes)
            {
                diff_cleanupMerge(diffs);
            }
            diff_cleanupSemanticLossless(diffs);

            // Find any overlaps between deletions and insertions.
            // e.g: <del>abcxxx</del><ins>xxxdef</ins>
            //   -> <del>abc</del>xxx<ins>def</ins>
            // e.g: <del>xxxabc</del><ins>defxxx</ins>
            //   -> <ins>def</ins>xxx<del>abc</del>
            // Only extract an overlap if it is as big as the edit ahead or behind it.
            pointer = 1;
            while (pointer < diffs.Count)
            {
                if (diffs[pointer - 1].Operation == Operation.DELETE &&
                    diffs[pointer].Operation == Operation.INSERT)
                {
                    string deletion = diffs[pointer - 1].Text;
                    string insertion = diffs[pointer].Text;
                    int overlapLength1 = diff_commonOverlap(deletion, insertion);
                    int overlapLength2 = diff_commonOverlap(insertion, deletion);
                    if (overlapLength1 >= overlapLength2)
                    {
                        if (overlapLength1 >= deletion.Length / 2.0 ||
                            overlapLength1 >= insertion.Length / 2.0)
                        {
                            // Overlap found.
                            // Insert an equality and trim the surrounding edits.
                            diffs.Insert(pointer, new Diff(Operation.EQUAL,
                                insertion.Substring(0, overlapLength1)));
                            diffs[pointer - 1].Text =
                                deletion.Substring(0, deletion.Length - overlapLength1);
                            diffs[pointer + 1].Text = insertion.Substring(overlapLength1);
                            pointer++;
                        }
                    }
                    else
                    {
                        if (overlapLength2 >= deletion.Length / 2.0 ||
                            overlapLength2 >= insertion.Length / 2.0)
                        {
                            // Reverse overlap found.
                            // Insert an equality and swap and trim the surrounding edits.
                            diffs.Insert(pointer, new Diff(Operation.EQUAL,
                                deletion.Substring(0, overlapLength2)));
                            diffs[pointer - 1].Operation = Operation.INSERT;
                            diffs[pointer - 1].Text =
                                insertion.Substring(0, insertion.Length - overlapLength2);
                            diffs[pointer + 1].Operation = Operation.DELETE;
                            diffs[pointer + 1].Text = deletion.Substring(overlapLength2);
                            pointer++;
                        }
                    }
                    pointer++;
                }
                pointer++;
            }
        }

        /**
         * Look for single edits surrounded on both sides by equalities
         * which can be shifted sideways to align the edit to a word boundary.
         * e.g: The c<ins>at c</ins>ame. -> The <ins>cat </ins>came.
         * @param diffs List of Diff objects.
         */
        public void diff_cleanupSemanticLossless(List<Diff> diffs)
        {
            int pointer = 1;
            // Intentionally ignore the first and last element (don't need checking).
            while (pointer < diffs.Count - 1)
            {
                if (diffs[pointer - 1].Operation == Operation.EQUAL &&
                  diffs[pointer + 1].Operation == Operation.EQUAL)
                {
                    // This is a single edit surrounded by equalities.
                    string equality1 = diffs[pointer - 1].Text;
                    string edit = diffs[pointer].Text;
                    string equality2 = diffs[pointer + 1].Text;

                    // First, shift the edit as far left as possible.
                    int commonOffset = diff_commonSuffix(equality1, edit);
                    if (commonOffset > 0)
                    {
                        string commonString = edit.Substring(edit.Length - commonOffset);
                        equality1 = equality1.Substring(0, equality1.Length - commonOffset);
                        edit = commonString + edit.Substring(0, edit.Length - commonOffset);
                        equality2 = commonString + equality2;
                    }

                    // Second, step character by character right,
                    // looking for the best fit.
                    string bestEquality1 = equality1;
                    string bestEdit = edit;
                    string bestEquality2 = equality2;
                    int bestScore = diff_cleanupSemanticScore(equality1, edit) +
                        diff_cleanupSemanticScore(edit, equality2);
                    while (edit.Length != 0 && equality2.Length != 0
                        && edit[0] == equality2[0])
                    {
                        equality1 += edit[0];
                        edit = edit.Substring(1) + equality2[0];
                        equality2 = equality2.Substring(1);
                        int score = diff_cleanupSemanticScore(equality1, edit) +
                            diff_cleanupSemanticScore(edit, equality2);
                        // The >= encourages trailing rather than leading whitespace on
                        // edits.
                        if (score >= bestScore)
                        {
                            bestScore = score;
                            bestEquality1 = equality1;
                            bestEdit = edit;
                            bestEquality2 = equality2;
                        }
                    }

                    if (diffs[pointer - 1].Text != bestEquality1)
                    {
                        // We have an improvement, save it back to the diff.
                        if (bestEquality1.Length != 0)
                        {
                            diffs[pointer - 1].Text = bestEquality1;
                        }
                        else
                        {
                            diffs.RemoveAt(pointer - 1);
                            pointer--;
                        }
                        diffs[pointer].Text = bestEdit;
                        if (bestEquality2.Length != 0)
                        {
                            diffs[pointer + 1].Text = bestEquality2;
                        }
                        else
                        {
                            diffs.RemoveAt(pointer + 1);
                            pointer--;
                        }
                    }
                }
                pointer++;
            }
        }

        /**
         * Given two strings, comAdde a score representing whether the internal
         * boundary falls on logical boundaries.
         * Scores range from 6 (best) to 0 (worst).
         * @param one First string.
         * @param two Second string.
         * @return The score.
         */
        private int diff_cleanupSemanticScore(string one, string two)
        {
            if (one.Length == 0 || two.Length == 0)
            {
                // Edges are the best.
                return 6;
            }

            // Each port of this function behaves slightly differently due to
            // subtle differences in each language's definition of things like
            // 'whitespace'.  Since this function's purpose is largely cosmetic,
            // the choice has been made to use each language's native features
            // rather than force total conformity.
            char char1 = one[one.Length - 1];
            char char2 = two[0];
            bool nonAlphaNumeric1 = !Char.IsLetterOrDigit(char1);
            bool nonAlphaNumeric2 = !Char.IsLetterOrDigit(char2);
            bool whitespace1 = nonAlphaNumeric1 && Char.IsWhiteSpace(char1);
            bool whitespace2 = nonAlphaNumeric2 && Char.IsWhiteSpace(char2);
            bool lineBreak1 = whitespace1 && Char.IsControl(char1);
            bool lineBreak2 = whitespace2 && Char.IsControl(char2);
            bool blankLine1 = lineBreak1 && _blankLineEnd.IsMatch(one);
            bool blankLine2 = lineBreak2 && _blankLineStart.IsMatch(two);

            if (blankLine1 || blankLine2)
            {
                // Five points for blank lines.
                return 5;
            }
            if (lineBreak1 || lineBreak2)
            {
                // Four points for line breaks.
                return 4;
            }
            if (nonAlphaNumeric1 && !whitespace1 && whitespace2)
            {
                // Three points for end of sentences.
                return 3;
            }
            if (whitespace1 || whitespace2)
            {
                // Two points for whitespace.
                return 2;
            }
            if (nonAlphaNumeric1 || nonAlphaNumeric2)
            {
                // One point for non-alphanumeric.
                return 1;
            }
            return 0;
        }

        // Define some regex patterns for matching boundaries.
        private readonly Regex _blankLineEnd = new Regex("\\n\\r?\\n\\Z");
        private readonly Regex _blankLineStart = new Regex("\\A\\r?\\n\\r?\\n");

        /**
         * Reduce the number of edits by eliminating operationally trivial
         * equalities.
         * @param diffs List of Diff objects.
         */
        public void diff_cleanupEfficiency(List<Diff> diffs)
        {
            bool changes = false;
            // Stack of indices where equalities are found.
            var equalities = new Stack<int>();
            // Always equal to equalities[equalitiesLength-1][1]
            string lastequality = string.Empty;
            int pointer = 0;  // Index of current position.
            // Is there an insertion operation before the last equality.
            bool preIns = false;
            // Is there a deletion operation before the last equality.
            bool preDel = false;
            // Is there an insertion operation after the last equality.
            bool postIns = false;
            // Is there a deletion operation after the last equality.
            bool postDel = false;
            while (pointer < diffs.Count)
            {
                if (diffs[pointer].Operation == Operation.EQUAL)
                {  // Equality found.
                    if (diffs[pointer].Text.Length < DiffEditCost
                        && (postIns || postDel))
                    {
                        // Candidate found.
                        equalities.Push(pointer);
                        preIns = postIns;
                        preDel = postDel;
                        lastequality = diffs[pointer].Text;
                    }
                    else
                    {
                        // Not a candidate, and can never become one.
                        equalities.Clear();
                        lastequality = string.Empty;
                    }
                    postIns = postDel = false;
                }
                else
                {  // An insertion or deletion.
                    if (diffs[pointer].Operation == Operation.DELETE)
                    {
                        postDel = true;
                    }
                    else
                    {
                        postIns = true;
                    }
                    /*
                     * Five types to be split:
                     * <ins>A</ins><del>B</del>XY<ins>C</ins><del>D</del>
                     * <ins>A</ins>X<ins>C</ins><del>D</del>
                     * <ins>A</ins><del>B</del>X<ins>C</ins>
                     * <ins>A</del>X<ins>C</ins><del>D</del>
                     * <ins>A</ins><del>B</del>X<del>C</del>
                     */
                    if ((lastequality.Length != 0)
                        && ((preIns && preDel && postIns && postDel)
                        || ((lastequality.Length < DiffEditCost / 2)
                        && ((preIns ? 1 : 0) + (preDel ? 1 : 0) + (postIns ? 1 : 0)
                        + (postDel ? 1 : 0)) == 3)))
                    {
                        // Duplicate record.
                        diffs.Insert(equalities.Peek(),
                                     new Diff(Operation.DELETE, lastequality));
                        // Change second copy to insert.
                        diffs[equalities.Peek() + 1].Operation = Operation.INSERT;
                        equalities.Pop();  // Throw away the equality we just deleted.
                        lastequality = string.Empty;
                        if (preIns && preDel)
                        {
                            // No changes made which could affect previous entry, keep going.
                            postIns = postDel = true;
                            equalities.Clear();
                        }
                        else
                        {
                            if (equalities.Count > 0)
                            {
                                equalities.Pop();
                            }

                            pointer = equalities.Count > 0 ? equalities.Peek() : -1;
                            postIns = postDel = false;
                        }
                        changes = true;
                    }
                }
                pointer++;
            }

            if (changes)
            {
                diff_cleanupMerge(diffs);
            }
        }

        /**
         * Reorder and merge like edit sections.  Merge equalities.
         * Any edit section can move as long as it doesn't cross an equality.
         * @param diffs List of Diff objects.
         */
        public void diff_cleanupMerge(List<Diff> diffs)
        {
            // Add a dummy entry at the end.
            diffs.Add(new Diff(Operation.EQUAL, string.Empty));
            int pointer = 0;
            int countDelete = 0;
            int countInsert = 0;
            string textDelete = string.Empty;
            string textInsert = string.Empty;
            while (pointer < diffs.Count)
            {
                switch (diffs[pointer].Operation)
                {
                    case Operation.INSERT:
                        countInsert++;
                        textInsert += diffs[pointer].Text;
                        pointer++;
                        break;
                    case Operation.DELETE:
                        countDelete++;
                        textDelete += diffs[pointer].Text;
                        pointer++;
                        break;
                    case Operation.EQUAL:
                        // Upon reaching an equality, check for prior redundancies.
                        if (countDelete + countInsert > 1)
                        {
                            if (countDelete != 0 && countInsert != 0)
                            {
                                // Factor out any common prefixies.
                                int commonlength = diff_commonPrefix(textInsert, textDelete);
                                if (commonlength != 0)
                                {
                                    if ((pointer - countDelete - countInsert) > 0 &&
                                      diffs[pointer - countDelete - countInsert - 1].Operation
                                          == Operation.EQUAL)
                                    {
                                        diffs[pointer - countDelete - countInsert - 1].Text
                                            += textInsert.Substring(0, commonlength);
                                    }
                                    else
                                    {
                                        diffs.Insert(0, new Diff(Operation.EQUAL,
                                            textInsert.Substring(0, commonlength)));
                                        pointer++;
                                    }
                                    textInsert = textInsert.Substring(commonlength);
                                    textDelete = textDelete.Substring(commonlength);
                                }
                                // Factor out any common suffixies.
                                commonlength = diff_commonSuffix(textInsert, textDelete);
                                if (commonlength != 0)
                                {
                                    diffs[pointer].Text = textInsert.Substring(textInsert.Length
                                        - commonlength) + diffs[pointer].Text;
                                    textInsert = textInsert.Substring(0, textInsert.Length
                                        - commonlength);
                                    textDelete = textDelete.Substring(0, textDelete.Length
                                        - commonlength);
                                }
                            }
                            // Delete the offending records and add the merged ones.
                            if (countDelete == 0)
                            {
                                diffs.Splice(pointer - countInsert,
                                    countDelete + countInsert,
                                    new Diff(Operation.INSERT, textInsert));
                            }
                            else if (countInsert == 0)
                            {
                                diffs.Splice(pointer - countDelete,
                                    countDelete + countInsert,
                                    new Diff(Operation.DELETE, textDelete));
                            }
                            else
                            {
                                diffs.Splice(pointer - countDelete - countInsert,
                                    countDelete + countInsert,
                                    new Diff(Operation.DELETE, textDelete),
                                    new Diff(Operation.INSERT, textInsert));
                            }
                            pointer = pointer - countDelete - countInsert +
                                (countDelete != 0 ? 1 : 0) + (countInsert != 0 ? 1 : 0) + 1;
                        }
                        else if (pointer != 0
                          && diffs[pointer - 1].Operation == Operation.EQUAL)
                        {
                            // Merge this equality with the previous one.
                            diffs[pointer - 1].Text += diffs[pointer].Text;
                            diffs.RemoveAt(pointer);
                        }
                        else
                        {
                            pointer++;
                        }
                        countInsert = 0;
                        countDelete = 0;
                        textDelete = string.Empty;
                        textInsert = string.Empty;
                        break;
                }
            }
            if (diffs[diffs.Count - 1].Text.Length == 0)
            {
                diffs.RemoveAt(diffs.Count - 1);  // Remove the dummy entry at the end.
            }

            // Second pass: look for single edits surrounded on both sides by
            // equalities which can be shifted sideways to eliminate an equality.
            // e.g: A<ins>BA</ins>C -> <ins>AB</ins>AC
            bool changes = false;
            pointer = 1;
            // Intentionally ignore the first and last element (don't need checking).
            while (pointer < (diffs.Count - 1))
            {
                if (diffs[pointer - 1].Operation == Operation.EQUAL &&
                  diffs[pointer + 1].Operation == Operation.EQUAL)
                {
                    // This is a single edit surrounded by equalities.
                    if (diffs[pointer].Text.EndsWith(diffs[pointer - 1].Text,
                        StringComparison.Ordinal))
                    {
                        // Shift the edit over the previous equality.
                        diffs[pointer].Text = diffs[pointer - 1].Text +
                            diffs[pointer].Text.Substring(0, diffs[pointer].Text.Length -
                                                          diffs[pointer - 1].Text.Length);
                        diffs[pointer + 1].Text = diffs[pointer - 1].Text
                            + diffs[pointer + 1].Text;
                        diffs.Splice(pointer - 1, 1);
                        changes = true;
                    }
                    else if (diffs[pointer].Text.StartsWith(diffs[pointer + 1].Text,
                      StringComparison.Ordinal))
                    {
                        // Shift the edit over the next equality.
                        diffs[pointer - 1].Text += diffs[pointer + 1].Text;
                        diffs[pointer].Text =
                            diffs[pointer].Text.Substring(diffs[pointer + 1].Text.Length)
                            + diffs[pointer + 1].Text;
                        diffs.Splice(pointer + 1, 1);
                        changes = true;
                    }
                }
                pointer++;
            }
            // If shifts were made, the diff needs reordering and another shift sweep.
            if (changes)
            {
                diff_cleanupMerge(diffs);
            }
        }

        /**
         * loc is a location in text1, comAdde and return the equivalent location in
         * text2.
         * e.g. "The cat" vs "The big cat", 1->1, 5->8
         * @param diffs List of Diff objects.
         * @param loc Location within text1.
         * @return Location within text2.
         */
        public int diff_xIndex(List<Diff> diffs, int loc)
        {
            int chars1 = 0;
            int chars2 = 0;
            int lastChars1 = 0;
            int lastChars2 = 0;
            Diff lastDiff = null;
            foreach (Diff aDiff in diffs)
            {
                if (aDiff.Operation != Operation.INSERT)
                {
                    // Equality or deletion.
                    chars1 += aDiff.Text.Length;
                }
                if (aDiff.Operation != Operation.DELETE)
                {
                    // Equality or insertion.
                    chars2 += aDiff.Text.Length;
                }
                if (chars1 > loc)
                {
                    // Overshot the location.
                    lastDiff = aDiff;
                    break;
                }
                lastChars1 = chars1;
                lastChars2 = chars2;
            }
            if (lastDiff != null && lastDiff.Operation == Operation.DELETE)
            {
                // The location was deleted.
                return lastChars2;
            }
            // Add the remaining character length.
            return lastChars2 + (loc - lastChars1);
        }

        /**
         * Convert a Diff list into a pretty HTML report.
         * @param diffs List of Diff objects.
         * @return HTML representation.
         */
        public string diff_prettyHtml(List<Diff> diffs)
        {
            var html = new StringBuilder();
            foreach (Diff aDiff in diffs)
            {
                string text = aDiff.Text.Replace("&", "&amp;").Replace("<", "&lt;")
                  .Replace(">", "&gt;");
                switch (aDiff.Operation)
                {
                    case Operation.INSERT:
                        html.Append("<ins>").Append(text)
                            .Append("</ins>");
                        break;
                    case Operation.DELETE:
                        html.Append("<del>").Append(text)
                            .Append("</del>");
                        break;
                    case Operation.EQUAL:
                        html.Append("<span>").Append(text).Append("</span>");
                        break;
                }
            }
            return html.ToString();
        }

        /**
         * Compute and return the source text (all equalities and deletions).
         * @param diffs List of Diff objects.
         * @return Source text.
         */
        public string diff_text1(List<Diff> diffs)
        {
            var text = new StringBuilder();
            foreach (Diff aDiff in diffs)
            {
                if (aDiff.Operation != Operation.INSERT)
                {
                    text.Append(aDiff.Text);
                }
            }
            return text.ToString();
        }

        /**
         * Compute and return the destination text (all equalities and insertions).
         * @param diffs List of Diff objects.
         * @return Destination text.
         */
        public string diff_text2(List<Diff> diffs)
        {
            var text = new StringBuilder();
            foreach (Diff aDiff in diffs)
            {
                if (aDiff.Operation != Operation.DELETE)
                {
                    text.Append(aDiff.Text);
                }
            }
            return text.ToString();
        }

        /**
         * Compute the Levenshtein distance; the number of inserted, deleted or
         * substituted characters.
         * @param diffs List of Diff objects.
         * @return Number of changes.
         */
        public int diff_levenshtein(List<Diff> diffs)
        {
            int levenshtein = 0;
            int insertions = 0;
            int deletions = 0;
            foreach (Diff aDiff in diffs)
            {
                switch (aDiff.Operation)
                {
                    case Operation.INSERT:
                        insertions += aDiff.Text.Length;
                        break;
                    case Operation.DELETE:
                        deletions += aDiff.Text.Length;
                        break;
                    case Operation.EQUAL:
                        // A deletion and an insertion is one substitution.
                        levenshtein += Math.Max(insertions, deletions);
                        insertions = 0;
                        deletions = 0;
                        break;
                }
            }
            levenshtein += Math.Max(insertions, deletions);
            return levenshtein;
        }

        /**
         * Crush the diff into an encoded string which describes the operations
         * required to transform text1 into text2.
         * E.g. =3\t-2\t+ing  -> Keep 3 chars, delete 2 chars, insert 'ing'.
         * Operations are tab-separated.  Inserted text is escaped using %xx
         * notation.
         * @param diffs Array of Diff objects.
         * @return Delta text.
         */
        public string diff_toDelta(List<Diff> diffs)
        {
            var text = new StringBuilder();
            foreach (Diff aDiff in diffs)
            {
                switch (aDiff.Operation)
                {
                    case Operation.INSERT:
                        text.Append("+")
                            .Append(HttpUtility.UrlEncode(aDiff.Text, new UTF8Encoding())
                            .Replace('+', ' ')).Append("\t");
                        break;
                    case Operation.DELETE:
                        text.Append("-").Append(aDiff.Text.Length).Append("\t");
                        break;
                    case Operation.EQUAL:
                        text.Append("=").Append(aDiff.Text.Length).Append("\t");
                        break;
                }
            }
            string delta = text.ToString();
            if (delta.Length != 0)
            {
                // Strip off trailing tab character.
                delta = delta.Substring(0, delta.Length - 1);
                delta = UnescapeForEncodeUriCompatability(delta);
            }
            return delta;
        }

        /**
         * Given the original text1, and an encoded string which describes the
         * operations required to transform text1 into text2, comAdde the full diff.
         * @param text1 Source string for the diff.
         * @param delta Delta text.
         * @return Array of Diff objects or null if invalid.
         * @throws ArgumentException If invalid input.
         */
        public List<Diff> diff_fromDelta(string text1, string delta)
        {
            var diffs = new List<Diff>();
            int pointer = 0;  // Cursor in text1
            string[] tokens = delta.Split(new[] { "\t" },
                StringSplitOptions.None);
            foreach (string token in tokens)
            {
                if (token.Length == 0)
                {
                    // Blank tokens are ok (from a trailing \t).
                    continue;
                }
                // Each token begins with a one character parameter which specifies the
                // operation of this token (delete, insert, equality).
                string param = token.Substring(1);
                switch (token[0])
                {
                    case '+':
                        // decode would change all "+" to " "
                        param = param.Replace("+", "%2b");

                        param = HttpUtility.UrlDecode(param, new UTF8Encoding(false, true));
                        diffs.Add(new Diff(Operation.INSERT, param));
                        break;
                    case '-':
                    // Fall through.
                    case '=':
                        int n;
                        try
                        {
                            n = Convert.ToInt32(param);
                        }
                        catch (FormatException e)
                        {
                            throw new ArgumentException(
                                "Invalid number in diff_fromDelta: " + param, e);
                        }
                        if (n < 0)
                        {
                            throw new ArgumentException(
                                "Negative number in diff_fromDelta: " + param);
                        }
                        string text;
                        try
                        {
                            text = text1.Substring(pointer, n);
                            pointer += n;
                        }
                        catch (ArgumentOutOfRangeException e)
                        {
                            throw new ArgumentException("Delta length (" + pointer
                                + ") larger than source text length (" + text1.Length
                                + ").", e);
                        }
                        diffs.Add(token[0] == '=' ? new Diff(Operation.EQUAL, text) : new Diff(Operation.DELETE, text));
                        break;
                    default:
                        // Anything else is an error.
                        throw new ArgumentException(
                            "Invalid diff operation in diff_fromDelta: " + token[0]);
                }
            }
            if (pointer != text1.Length)
            {
                throw new ArgumentException("Delta length (" + pointer
                    + ") smaller than source text length (" + text1.Length + ").");
            }
            return diffs;
        }


        //  MATCH FUNCTIONS


        /**
         * Locate the best instance of 'pattern' in 'text' near 'loc'.
         * Returns -1 if no match found.
         * @param text The text to search.
         * @param pattern The pattern to search for.
         * @param loc The location to search around.
         * @return Best match index or -1.
         */
        public int match_main(string text, string pattern, int loc)
        {
            // Check for null inputs not needed since null can't be passed in C#.

            loc = Math.Max(0, Math.Min(loc, text.Length));
            if (text == pattern)
            {
                // Shortcut (potentially not guaranteed by the algorithm)
                return 0;
            }
            if (text.Length == 0)
            {
                // Nothing to match.
                return -1;
            }
            if (loc + pattern.Length <= text.Length
                && text.Substring(loc, pattern.Length) == pattern)
            {
                // Perfect match at the perfect spot!  (Includes case of null pattern)
                return loc;
            }
            // Do a fuzzy compare.
            return match_bitap(text, pattern, loc);
        }

        /**
         * Locate the best instance of 'pattern' in 'text' near 'loc' using the
         * Bitap algorithm.  Returns -1 if no match found.
         * @param text The text to search.
         * @param pattern The pattern to search for.
         * @param loc The location to search around.
         * @return Best match index or -1.
         */
        protected int match_bitap(string text, string pattern, int loc)
        {
            // assert (Match_MaxBits == 0 || pattern.Length <= Match_MaxBits)
            //    : "Pattern too long for this application.";

            // Initialise the alphabet.
            Dictionary<char, int> s = match_alphabet(pattern);

            // Highest score beyond which we give up.
            double scoreThreshold = MatchThreshold;
            // Is there a nearby exact match? (speedup)
            int bestLoc = text.IndexOf(pattern, loc, StringComparison.Ordinal);
            if (bestLoc != -1)
            {
                scoreThreshold = Math.Min(match_bitapScore(0, bestLoc, loc,
                    pattern), scoreThreshold);
                // What about in the other direction? (speedup)
                bestLoc = text.LastIndexOf(pattern,
                    Math.Min(loc + pattern.Length, text.Length),
                    StringComparison.Ordinal);
                if (bestLoc != -1)
                {
                    scoreThreshold = Math.Min(match_bitapScore(0, bestLoc, loc,
                        pattern), scoreThreshold);
                }
            }

            // Initialise the bit arrays.
            int matchmask = 1 << (pattern.Length - 1);
            bestLoc = -1;

            int binMax = pattern.Length + text.Length;
            // Empty initialization added to appease C# compiler.
            var lastRd = new int[0];
            for (int d = 0; d < pattern.Length; d++)
            {
                // Scan for the best match; each iteration allows for one more error.
                // Run a binary search to determine how far from 'loc' we can stray at
                // this error level.
                int binMin = 0;
                int binMid = binMax;
                while (binMin < binMid)
                {
                    if (match_bitapScore(d, loc + binMid, loc, pattern)
                        <= scoreThreshold)
                    {
                        binMin = binMid;
                    }
                    else
                    {
                        binMax = binMid;
                    }
                    binMid = (binMax - binMin) / 2 + binMin;
                }
                // Use the result from this iteration as the maximum for the next.
                binMax = binMid;
                int start = Math.Max(1, loc - binMid + 1);
                int finish = Math.Min(loc + binMid, text.Length) + pattern.Length;

                var rd = new int[finish + 2];
                rd[finish + 1] = (1 << d) - 1;
                for (int j = finish; j >= start; j--)
                {
                    int charMatch;
                    if (text.Length <= j - 1 || !s.ContainsKey(text[j - 1]))
                    {
                        // Out of range.
                        charMatch = 0;
                    }
                    else
                    {
                        charMatch = s[text[j - 1]];
                    }
                    if (d == 0)
                    {
                        // First pass: exact match.
                        rd[j] = ((rd[j + 1] << 1) | 1) & charMatch;
                    }
                    else
                    {
                        // Subsequent passes: fuzzy match.
                        rd[j] = ((rd[j + 1] << 1) | 1) & charMatch
                            | (((lastRd[j + 1] | lastRd[j]) << 1) | 1) | lastRd[j + 1];
                    }
                    if ((rd[j] & matchmask) != 0)
                    {
                        double score = match_bitapScore(d, j - 1, loc, pattern);
                        // This match will almost certainly be better than any existing
                        // match.  But check anyway.
                        if (score <= scoreThreshold)
                        {
                            // Told you so.
                            scoreThreshold = score;
                            bestLoc = j - 1;
                            if (bestLoc > loc)
                            {
                                // When passing loc, don't exceed our current distance from loc.
                                start = Math.Max(1, 2 * loc - bestLoc);
                            }
                            else
                            {
                                // Already passed loc, downhill from here on in.
                                break;
                            }
                        }
                    }
                }
                if (match_bitapScore(d + 1, loc, loc, pattern) > scoreThreshold)
                {
                    // No hope for a (better) match at greater error levels.
                    break;
                }
                lastRd = rd;
            }
            return bestLoc;
        }

        /**
         * Compute and return the score for a match with e errors and x location.
         * @param e Number of errors in match.
         * @param x Location of match.
         * @param loc Expected location of match.
         * @param pattern Pattern being sought.
         * @return Overall score for match (0.0 = good, 1.0 = bad).
         */
        private double match_bitapScore(int e, int x, int loc, string pattern)
        {
            float accuracy = (float)e / pattern.Length;
            int proximity = Math.Abs(loc - x);
            if (MatchDistance == 0)
            {
                // Dodge divide by zero error.
                return proximity == 0 ? accuracy : 1.0;
            }
            return accuracy + (proximity / (float)MatchDistance);
        }

        /**
         * Initialise the alphabet for the Bitap algorithm.
         * @param pattern The text to encode.
         * @return Hash of character locations.
         */
        protected Dictionary<char, int> match_alphabet(string pattern)
        {
            var s = new Dictionary<char, int>();
            char[] charPattern = pattern.ToCharArray();
            foreach (char c in charPattern)
            {
                if (!s.ContainsKey(c))
                {
                    s.Add(c, 0);
                }
            }
            int i = 0;
            foreach (char c in charPattern)
            {
                int value = s[c] | (1 << (pattern.Length - i - 1));
                s[c] = value;
                i++;
            }
            return s;
        }


        //  PATCH FUNCTIONS


        /**
         * Increase the context until it is unique,
         * but don't let the pattern expand beyond Match_MaxBits.
         * @param patch The patch to grow.
         * @param text Source text.
         */
        protected void patch_addContext(Patch patch, string text)
        {
            if (text.Length == 0)
            {
                return;
            }
            string pattern = text.Substring(patch.Start2, patch.Length1);
            int padding = 0;

            // Look for the first and last matches of pattern in text.  If two
            // different matches are found, increase the pattern length.
            while (text.IndexOf(pattern, StringComparison.Ordinal)
                != text.LastIndexOf(pattern, StringComparison.Ordinal)
                && pattern.Length < MatchMaxBits - PatchMargin - PatchMargin)
            {
                padding += PatchMargin;
                pattern = text.JavaSubstring(Math.Max(0, patch.Start2 - padding),
                    Math.Min(text.Length, patch.Start2 + patch.Length1 + padding));
            }
            // Add one chunk for good luck.
            padding += PatchMargin;

            // Add the prefix.
            string prefix = text.JavaSubstring(Math.Max(0, patch.Start2 - padding),
              patch.Start2);
            if (prefix.Length != 0)
            {
                patch.Diffs.Insert(0, new Diff(Operation.EQUAL, prefix));
            }
            // Add the suffix.
            string suffix = text.JavaSubstring(patch.Start2 + patch.Length1,
                Math.Min(text.Length, patch.Start2 + patch.Length1 + padding));
            if (suffix.Length != 0)
            {
                patch.Diffs.Add(new Diff(Operation.EQUAL, suffix));
            }

            // Roll back the start points.
            patch.Start1 -= prefix.Length;
            patch.Start2 -= prefix.Length;
            // Extend the lengths.
            patch.Length1 += prefix.Length + suffix.Length;
            patch.Length2 += prefix.Length + suffix.Length;
        }

        /**
         * Compute a list of patches to turn text1 into text2.
         * A set of diffs will be computed.
         * @param text1 Old text.
         * @param text2 New text.
         * @return List of Patch objects.
         */
        public List<Patch> patch_make(string text1, string text2)
        {
            // Check for null inputs not needed since null can't be passed in C#.
            // No diffs provided, comAdde our own.
            List<Diff> diffs = diff_main(text1, text2, true);
            if (diffs.Count > 2)
            {
                diff_cleanupSemantic(diffs);
                diff_cleanupEfficiency(diffs);
            }
            return patch_make(text1, diffs);
        }

        /**
         * Compute a list of patches to turn text1 into text2.
         * text1 will be derived from the provided diffs.
         * @param diffs Array of Diff objects for text1 to text2.
         * @return List of Patch objects.
         */
        public List<Patch> patch_make(List<Diff> diffs)
        {
            // Check for null inputs not needed since null can't be passed in C#.
            // No origin string provided, comAdde our own.
            string text1 = diff_text1(diffs);
            return patch_make(text1, diffs);
        }

        /**
         * Compute a list of patches to turn text1 into text2.
         * text2 is ignored, diffs are the delta between text1 and text2.
         * @param text1 Old text
         * @param text2 Ignored.
         * @param diffs Array of Diff objects for text1 to text2.
         * @return List of Patch objects.
         * @deprecated Prefer patch_make(string text1, List<Diff> diffs).
         */
        public List<Patch> patch_make(string text1, string text2,
            List<Diff> diffs)
        {
            return patch_make(text1, diffs);
        }

        /**
         * Compute a list of patches to turn text1 into text2.
         * text2 is not provided, diffs are the delta between text1 and text2.
         * @param text1 Old text.
         * @param diffs Array of Diff objects for text1 to text2.
         * @return List of Patch objects.
         */
        public List<Patch> patch_make(string text1, List<Diff> diffs)
        {
            // Check for null inputs not needed since null can't be passed in C#.
            var patches = new List<Patch>();
            if (diffs.Count == 0)
            {
                return patches;  // Get rid of the null case.
            }
            var patch = new Patch();
            int charCount1 = 0;  // Number of characters into the text1 string.
            int charCount2 = 0;  // Number of characters into the text2 string.
            // Start with text1 (prepatch_text) and apply the diffs until we arrive at
            // text2 (postpatch_text). We recreate the patches one by one to determine
            // context info.
            string prepatchText = text1;
            string postpatchText = text1;
            foreach (Diff aDiff in diffs)
            {
                if (patch.Diffs.Count == 0 && aDiff.Operation != Operation.EQUAL)
                {
                    // A new patch starts here.
                    patch.Start1 = charCount1;
                    patch.Start2 = charCount2;
                }

                switch (aDiff.Operation)
                {
                    case Operation.INSERT:
                        patch.Diffs.Add(aDiff);
                        patch.Length2 += aDiff.Text.Length;
                        postpatchText = postpatchText.Insert(charCount2, aDiff.Text);
                        break;
                    case Operation.DELETE:
                        patch.Length1 += aDiff.Text.Length;
                        patch.Diffs.Add(aDiff);
                        postpatchText = postpatchText.Remove(charCount2,
                            aDiff.Text.Length);
                        break;
                    case Operation.EQUAL:
                        if (aDiff.Text.Length <= 2 * PatchMargin
                            && patch.Diffs.Count() != 0 && aDiff != diffs.Last())
                        {
                            // Small equality inside a patch.
                            patch.Diffs.Add(aDiff);
                            patch.Length1 += aDiff.Text.Length;
                            patch.Length2 += aDiff.Text.Length;
                        }

                        if (aDiff.Text.Length >= 2 * PatchMargin)
                        {
                            // Time for a new patch.
                            if (patch.Diffs.Count != 0)
                            {
                                patch_addContext(patch, prepatchText);
                                patches.Add(patch);
                                patch = new Patch();
                                // Unlike Unidiff, our patch lists have a rolling context.
                                // http://code.google.com/p/google-diff-match-patch/wiki/Unidiff
                                // Update prepatch text & pos to reflect the application of the
                                // just completed patch.
                                prepatchText = postpatchText;
                                charCount1 = charCount2;
                            }
                        }
                        break;
                }

                // Update the current character count.
                if (aDiff.Operation != Operation.INSERT)
                {
                    charCount1 += aDiff.Text.Length;
                }
                if (aDiff.Operation != Operation.DELETE)
                {
                    charCount2 += aDiff.Text.Length;
                }
            }
            // Pick up the leftover patch if not empty.
            if (patch.Diffs.Count != 0)
            {
                patch_addContext(patch, prepatchText);
                patches.Add(patch);
            }

            return patches;
        }

        /**
         * Given an array of patches, return another array that is identical.
         * @param patches Array of Patch objects.
         * @return Array of Patch objects.
         */
        public List<Patch> patch_deepCopy(List<Patch> patches)
        {
            var patchesCopy = new List<Patch>();
            foreach (Patch aPatch in patches)
            {
                var patchCopy = new Patch();
                foreach (Diff aDiff in aPatch.Diffs)
                {
                    var diffCopy = new Diff(aDiff.Operation, aDiff.Text);
                    patchCopy.Diffs.Add(diffCopy);
                }
                patchCopy.Start1 = aPatch.Start1;
                patchCopy.Start2 = aPatch.Start2;
                patchCopy.Length1 = aPatch.Length1;
                patchCopy.Length2 = aPatch.Length2;
                patchesCopy.Add(patchCopy);
            }
            return patchesCopy;
        }

        /**
         * Merge a set of patches onto the text.  Return a patched text, as well
         * as an array of true/false values indicating which patches were applied.
         * @param patches Array of Patch objects
         * @param text Old text.
         * @return Two element Object array, containing the new text and an array of
         *      bool values.
         */
        public Object[] patch_apply(List<Patch> patches, string text)
        {
            if (patches.Count == 0)
            {
                return new Object[] { text, new bool[0] };
            }

            // Deep copy the patches so that no changes are made to originals.
            patches = patch_deepCopy(patches);

            string nullPadding = patch_addPadding(patches);
            text = nullPadding + text + nullPadding;
            patch_splitMax(patches);

            int x = 0;
            // delta keeps track of the offset between the expected and actual
            // location of the previous patch.  If there are patches expected at
            // positions 10 and 20, but the first patch was found at 12, delta is 2
            // and the second patch has an effective expected position of 22.
            int delta = 0;
            var results = new bool[patches.Count];
            foreach (Patch aPatch in patches)
            {
                int expectedLoc = aPatch.Start2 + delta;
                string text1 = diff_text1(aPatch.Diffs);
                int startLoc;
                int endLoc = -1;
                if (text1.Length > MatchMaxBits)
                {
                    // patch_splitMax will only provide an oversized pattern
                    // in the case of a monster delete.
                    startLoc = match_main(text,
                        text1.Substring(0, MatchMaxBits), expectedLoc);
                    if (startLoc != -1)
                    {
                        endLoc = match_main(text,
                            text1.Substring(text1.Length - MatchMaxBits),
                            expectedLoc + text1.Length - MatchMaxBits);
                        if (endLoc == -1 || startLoc >= endLoc)
                        {
                            // Can't find valid trailing context.  Drop this patch.
                            startLoc = -1;
                        }
                    }
                }
                else
                {
                    startLoc = match_main(text, text1, expectedLoc);
                }
                if (startLoc == -1)
                {
                    // No match found.  :(
                    results[x] = false;
                    // Subtract the delta for this failed patch from subsequent patches.
                    delta -= aPatch.Length2 - aPatch.Length1;
                }
                else
                {
                    // Found a match.  :)
                    results[x] = true;
                    delta = startLoc - expectedLoc;
                    string text2 =
                        text.JavaSubstring(startLoc, endLoc == -1
                        ? Math.Min(startLoc + text1.Length, text.Length)
                        : Math.Min(endLoc + MatchMaxBits, text.Length));
                    if (text1 == text2)
                    {
                        // Perfect match, just shove the Replacement text in.
                        text = text.Substring(0, startLoc) + diff_text2(aPatch.Diffs)
                            + text.Substring(startLoc + text1.Length);
                    }
                    else
                    {
                        // Imperfect match.  Run a diff to get a framework of equivalent
                        // indices.
                        List<Diff> diffs = diff_main(text1, text2, false);
                        if (text1.Length > MatchMaxBits
                            && diff_levenshtein(diffs) / (float)text1.Length
                            > PatchDeleteThreshold)
                        {
                            // The end points match, but the content is unacceptably bad.
                            results[x] = false;
                        }
                        else
                        {
                            diff_cleanupSemanticLossless(diffs);
                            int index1 = 0;
                            foreach (Diff aDiff in aPatch.Diffs)
                            {
                                if (aDiff.Operation != Operation.EQUAL)
                                {
                                    int index2 = diff_xIndex(diffs, index1);
                                    if (aDiff.Operation == Operation.INSERT)
                                    {
                                        // Insertion
                                        text = text.Insert(startLoc + index2, aDiff.Text);
                                    }
                                    else if (aDiff.Operation == Operation.DELETE)
                                    {
                                        // Deletion
                                        text = text.Remove(startLoc + index2, diff_xIndex(diffs,
                                            index1 + aDiff.Text.Length) - index2);
                                    }
                                }
                                if (aDiff.Operation != Operation.DELETE)
                                {
                                    index1 += aDiff.Text.Length;
                                }
                            }
                        }
                    }
                }
                x++;
            }
            // Strip the padding off.
            text = text.Substring(nullPadding.Length, text.Length
                - 2 * nullPadding.Length);
            return new Object[] { text, results };
        }

        /**
         * Add some padding on text start and end so that edges can match something.
         * Intended to be called only from within patch_apply.
         * @param patches Array of Patch objects.
         * @return The padding string added to each side.
         */
        public string patch_addPadding(List<Patch> patches)
        {
            short paddingLength = PatchMargin;
            string nullPadding = string.Empty;
            for (short x = 1; x <= paddingLength; x++)
            {
                nullPadding += (char)x;
            }

            // Bump all the patches forward.
            foreach (Patch aPatch in patches)
            {
                aPatch.Start1 += paddingLength;
                aPatch.Start2 += paddingLength;
            }

            // Add some padding on start of first diff.
            Patch patch = patches.First();
            List<Diff> diffs = patch.Diffs;
            if (diffs.Count == 0 || diffs.First().Operation != Operation.EQUAL)
            {
                // Add nullPadding equality.
                diffs.Insert(0, new Diff(Operation.EQUAL, nullPadding));
                patch.Start1 -= paddingLength;  // Should be 0.
                patch.Start2 -= paddingLength;  // Should be 0.
                patch.Length1 += paddingLength;
                patch.Length2 += paddingLength;
            }
            else if (paddingLength > diffs.First().Text.Length)
            {
                // Grow first equality.
                Diff firstDiff = diffs.First();
                int extraLength = paddingLength - firstDiff.Text.Length;
                firstDiff.Text = nullPadding.Substring(firstDiff.Text.Length)
                    + firstDiff.Text;
                patch.Start1 -= extraLength;
                patch.Start2 -= extraLength;
                patch.Length1 += extraLength;
                patch.Length2 += extraLength;
            }

            // Add some padding on end of last diff.
            patch = patches.Last();
            diffs = patch.Diffs;
            if (diffs.Count == 0 || diffs.Last().Operation != Operation.EQUAL)
            {
                // Add nullPadding equality.
                diffs.Add(new Diff(Operation.EQUAL, nullPadding));
                patch.Length1 += paddingLength;
                patch.Length2 += paddingLength;
            }
            else if (paddingLength > diffs.Last().Text.Length)
            {
                // Grow last equality.
                Diff lastDiff = diffs.Last();
                int extraLength = paddingLength - lastDiff.Text.Length;
                lastDiff.Text += nullPadding.Substring(0, extraLength);
                patch.Length1 += extraLength;
                patch.Length2 += extraLength;
            }

            return nullPadding;
        }

        /**
         * Look through the patches and break up any which are longer than the
         * maximum limit of the match algorithm.
         * Intended to be called only from within patch_apply.
         * @param patches List of Patch objects.
         */
        public void patch_splitMax(List<Patch> patches)
        {
            const short patchSize = MatchMaxBits;
            for (int x = 0; x < patches.Count; x++)
            {
                if (patches[x].Length1 <= patchSize)
                {
                    continue;
                }
                Patch bigpatch = patches[x];
                // Remove the big old patch.
                patches.Splice(x--, 1);
                int start1 = bigpatch.Start1;
                int start2 = bigpatch.Start2;
                string precontext = string.Empty;
                while (bigpatch.Diffs.Count != 0)
                {
                    // Create one of several smaller patches.
                    var patch = new Patch();
                    bool empty = true;
                    patch.Start1 = start1 - precontext.Length;
                    patch.Start2 = start2 - precontext.Length;
                    if (precontext.Length != 0)
                    {
                        patch.Length1 = patch.Length2 = precontext.Length;
                        patch.Diffs.Add(new Diff(Operation.EQUAL, precontext));
                    }
                    while (bigpatch.Diffs.Count != 0
                        && patch.Length1 < patchSize - PatchMargin)
                    {
                        Operation diffType = bigpatch.Diffs[0].Operation;
                        string diffText = bigpatch.Diffs[0].Text;
                        if (diffType == Operation.INSERT)
                        {
                            // Insertions are harmless.
                            patch.Length2 += diffText.Length;
                            start2 += diffText.Length;
                            patch.Diffs.Add(bigpatch.Diffs.First());
                            bigpatch.Diffs.RemoveAt(0);
                            empty = false;
                        }
                        else if (diffType == Operation.DELETE && patch.Diffs.Count == 1
                          && patch.Diffs.First().Operation == Operation.EQUAL
                          && diffText.Length > 2 * patchSize)
                        {
                            // This is a large deletion.  Let it pass in one chunk.
                            patch.Length1 += diffText.Length;
                            start1 += diffText.Length;
                            empty = false;
                            patch.Diffs.Add(new Diff(diffType, diffText));
                            bigpatch.Diffs.RemoveAt(0);
                        }
                        else
                        {
                            // Deletion or equality.  Only take as much as we can stomach.
                            diffText = diffText.Substring(0, Math.Min(diffText.Length,
                                patchSize - patch.Length1 - PatchMargin));
                            patch.Length1 += diffText.Length;
                            start1 += diffText.Length;
                            if (diffType == Operation.EQUAL)
                            {
                                patch.Length2 += diffText.Length;
                                start2 += diffText.Length;
                            }
                            else
                            {
                                empty = false;
                            }
                            patch.Diffs.Add(new Diff(diffType, diffText));
                            if (diffText == bigpatch.Diffs[0].Text)
                            {
                                bigpatch.Diffs.RemoveAt(0);
                            }
                            else
                            {
                                bigpatch.Diffs[0].Text =
                                    bigpatch.Diffs[0].Text.Substring(diffText.Length);
                            }
                        }
                    }
                    // Compute the head context for the next patch.
                    precontext = diff_text2(patch.Diffs);
                    precontext = precontext.Substring(Math.Max(0,
                        precontext.Length - PatchMargin));

                    string postcontext;
                    // Append the end context for this patch.
                    if (diff_text1(bigpatch.Diffs).Length > PatchMargin)
                    {
                        postcontext = diff_text1(bigpatch.Diffs)
                            .Substring(0, PatchMargin);
                    }
                    else
                    {
                        postcontext = diff_text1(bigpatch.Diffs);
                    }

                    if (postcontext.Length != 0)
                    {
                        patch.Length1 += postcontext.Length;
                        patch.Length2 += postcontext.Length;
                        if (patch.Diffs.Count != 0
                            && patch.Diffs[patch.Diffs.Count - 1].Operation
                            == Operation.EQUAL)
                        {
                            patch.Diffs[patch.Diffs.Count - 1].Text += postcontext;
                        }
                        else
                        {
                            patch.Diffs.Add(new Diff(Operation.EQUAL, postcontext));
                        }
                    }
                    if (!empty)
                    {
                        patches.Splice(++x, 0, patch);
                    }
                }
            }
        }

        /**
       * Take a list of patches and return a textual representation.
       * @param patches List of Patch objects.
       * @return Text representation of patches.
       */
        public string patch_toText(List<Patch> patches)
        {
            var text = new StringBuilder();
            foreach (Patch aPatch in patches)
            {
                text.Append(aPatch);
            }
            return text.ToString();
        }

        /**
         * Parse a textual representation of patches and return a List of Patch
         * objects.
         * @param textline Text representation of patches.
         * @return List of Patch objects.
         * @throws ArgumentException If invalid input.
         */
        public List<Patch> patch_fromText(string textline)
        {
            var patches = new List<Patch>();
            if (textline.Length == 0)
            {
                return patches;
            }
            string[] text = textline.Split('\n');
            int textPointer = 0;
            var patchHeader
              = new Regex("^@@ -(\\d+),?(\\d*) \\+(\\d+),?(\\d*) @@$");
            while (textPointer < text.Length)
            {
                Match m = patchHeader.Match(text[textPointer]);
                if (!m.Success)
                {
                    throw new ArgumentException("Invalid patch string: "
                        + text[textPointer]);
                }
                var patch = new Patch();
                patches.Add(patch);
                patch.Start1 = Convert.ToInt32(m.Groups[1].Value);
                if (m.Groups[2].Length == 0)
                {
                    patch.Start1--;
                    patch.Length1 = 1;
                }
                else if (m.Groups[2].Value == "0")
                {
                    patch.Length1 = 0;
                }
                else
                {
                    patch.Start1--;
                    patch.Length1 = Convert.ToInt32(m.Groups[2].Value);
                }

                patch.Start2 = Convert.ToInt32(m.Groups[3].Value);
                if (m.Groups[4].Length == 0)
                {
                    patch.Start2--;
                    patch.Length2 = 1;
                }
                else if (m.Groups[4].Value == "0")
                {
                    patch.Length2 = 0;
                }
                else
                {
                    patch.Start2--;
                    patch.Length2 = Convert.ToInt32(m.Groups[4].Value);
                }
                textPointer++;

                while (textPointer < text.Length)
                {
                    char sign;
                    try
                    {
                        sign = text[textPointer][0];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        // Blank line?  Whatever.
                        textPointer++;
                        continue;
                    }
                    string line = text[textPointer].Substring(1);
                    line = line.Replace("+", "%2b");
                    line = HttpUtility.UrlDecode(line, new UTF8Encoding(false, true));
                    if (sign == '-')
                    {
                        // Deletion.
                        patch.Diffs.Add(new Diff(Operation.DELETE, line));
                    }
                    else if (sign == '+')
                    {
                        // Insertion.
                        patch.Diffs.Add(new Diff(Operation.INSERT, line));
                    }
                    else if (sign == ' ')
                    {
                        // Minor equality.
                        patch.Diffs.Add(new Diff(Operation.EQUAL, line));
                    }
                    else if (sign == '@')
                    {
                        // Start of next patch.
                        break;
                    }
                    else
                    {
                        // WTF?
                        throw new ArgumentException(
                            "Invalid patch mode '" + sign + "' in: " + line);
                    }
                    textPointer++;
                }
            }
            return patches;
        }

        /**
         * Unescape selected chars for compatability with JavaScript's encodeURI.
         * In speed critical applications this could be dropped since the
         * receiving application will certainly decode these fine.
         * Note that this function is case-sensitive.  Thus "%3F" would not be
         * unescaped.  But this is ok because it is only called with the output of
         * HttpUtility.UrlEncode which returns lowercase hex.
         *
         * Example: "%3f" -> "?", "%24" -> "$", etc.
         *
         * @param str The string to escape.
         * @return The escaped string.
         */
        public static string UnescapeForEncodeUriCompatability(string str)
        {
            return str.Replace("%21", "!").Replace("%7e", "~")
                .Replace("%27", "'").Replace("%28", "(").Replace("%29", ")")
                .Replace("%3b", ";").Replace("%2f", "/").Replace("%3f", "?")
                .Replace("%3a", ":").Replace("%40", "@").Replace("%26", "&")
                .Replace("%3d", "=").Replace("%2b", "+").Replace("%24", "$")
                .Replace("%2c", ",").Replace("%23", "#");
        }
    }
}
