/* <!--
 * Copyright (C) 2009 - 2009 by OpenGamma Inc. and other contributors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *     
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * -->
 */
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace FudgeMessage.Util
{
    /// <summary>
    /// Encoding to support working with <a href="http://en.wikipedia.org/wiki/UTF-8#Modified_UTF-8">Modified UTF-8</a> data.
    /// </summary>
    public class ModifiedUTF8Encoding : Encoding
    {
        // See the .net implementation of UTF8Encoding for what needs doing.

        /// <inheritdoc />
        public override int GetByteCount(char[] chars, int index, int count)
        {
            // REVIEW wyliekir 2009-08-17 -- This was taken almost verbatim from
            // DataOutputStream.
            int utflen = 0;
            int c = 0;

            for (int i = index; i < index + count; i++)
            {
                c = chars[i];
                if ((c >= 0x0001) && (c <= 0x007F))
                {
                    utflen++;
                }
                else if (c > 0x07FF)
                {
                    utflen += 3;
                }
                else
                {
                    utflen += 2;
                }
            }
            return utflen;
        }

        /// <inheritdoc />
        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            int pos = byteIndex;
            int i = charIndex;
            char c;
            for (i = charIndex; i < charIndex + charCount; i++)
            {
                c = chars[i];
                if (!((c >= 0x0001) && (c <= 0x007F)))
                    break;
                bytes[pos++] = (byte)c;
            }

            for (; i < charIndex + charCount; i++)
            {
                c = chars[i];
                if ((c >= 0x0001) && (c <= 0x007F))
                {
                    bytes[pos++] = (byte)c;

                }
                else if (c > 0x07FF)
                {
                    bytes[pos++] = (byte)(0xE0 | ((c >> 12) & 0x0F));
                    bytes[pos++] = (byte)(0x80 | ((c >> 6) & 0x3F));
                    bytes[pos++] = (byte)(0x80 | ((c >> 0) & 0x3F));
                }
                else
                {
                    bytes[pos++] = (byte)(0xC0 | ((c >> 6) & 0x1F));
                    bytes[pos++] = (byte)(0x80 | ((c >> 0) & 0x3F));
                }
            }
            return pos - byteIndex;
        }

        /// <inheritdoc />
        public override int GetCharCount(byte[] bytes, int index, int count)
        {
            // MUST KEEP IN SYNC WITH GetChars()

            int c;
            int pos = index;
            int chararr_pos = 0;
            int end = index + count;

            // REVIEW kirk 2009-08-18 -- This can be optimized. We're copying the data too many
            // times. Particularly since we expect that most of the time we're reading from
            // a byte array already, duplicating it doesn't make much sense.
            while (pos < end)
            {
                c = (int)bytes[pos] & 0xff;
                if (c > 127) break;
                pos++;
                chararr_pos++;

            }

            while (pos < end)
            {
                c = (int)bytes[pos] & 0xff;
                switch (c >> 4)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                        /* 0xxxxxxx*/
                        pos++;
                        chararr_pos++;
                        break;
                    case 12:
                    case 13:
                        /* 110x xxxx   10xx xxxx*/
                        pos += 2;
                        if (pos > end)
                            throw new UTFDataFormatException(
                                "malformed input: partial character at end");
                        chararr_pos++;
                        break;
                    case 14:
                        /* 1110 xxxx  10xx xxxx  10xx xxxx */
                        pos += 3;
                        if (pos > end)
                            throw new UTFDataFormatException(
                                "malformed input: partial character at end");
                        chararr_pos++;
                        break;
                    default:
                        /* 10xx xxxx,  1111 xxxx */
                        throw new UTFDataFormatException(
                            "malformed input around byte " + pos);
                }
            }
            return chararr_pos;
        }

        /// <inheritdoc />
        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            // MUST BE KEPT IN SYNC WITH GetCharCount()
            int c, char2, char3;
            int pos = byteIndex;
            int chararr_pos = charIndex;
            int end = byteIndex + byteCount;

            // REVIEW kirk 2009-08-18 -- This can be optimized. We're copying the data too many
            // times. Particularly since we expect that most of the time we're reading from
            // a byte array already, duplicating it doesn't make much sense.
            while (pos < end)
            {
                c = (int)bytes[pos] & 0xff;
                if (c > 127) break;
                pos++;
                chars[chararr_pos++] = (char)c;

            }

            while (pos < end)
            {
                c = (int)bytes[pos] & 0xff;
                switch (c >> 4)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                        /* 0xxxxxxx*/
                        pos++;
                        chars[chararr_pos++] = (char)c;
                        break;
                    case 12:
                    case 13:
                        /* 110x xxxx   10xx xxxx*/
                        pos += 2;
                        if (pos > end)
                            throw new UTFDataFormatException(
                                "malformed input: partial character at end");
                        char2 = (int)bytes[pos - 1];
                        if ((char2 & 0xC0) != 0x80)
                            throw new UTFDataFormatException(
                                "malformed input around byte " + pos);
                        chars[chararr_pos++] = (char)(((c & 0x1F) << 6) |
                                                        (char2 & 0x3F));
                        break;
                    case 14:
                        /* 1110 xxxx  10xx xxxx  10xx xxxx */
                        pos += 3;
                        if (pos > end)
                            throw new UTFDataFormatException(
                                "malformed input: partial character at end");
                        char2 = (int)bytes[pos - 2];
                        char3 = (int)bytes[pos - 1];
                        if (((char2 & 0xC0) != 0x80) || ((char3 & 0xC0) != 0x80))
                            throw new UTFDataFormatException(
                                "malformed input around byte " + (pos - 1));
                        chars[chararr_pos++] = (char)(((c & 0x0F) << 12) |
                                                        ((char2 & 0x3F) << 6) |
                                                        ((char3 & 0x3F) << 0));
                        break;
                    default:
                        /* 10xx xxxx,  1111 xxxx */
                        throw new UTFDataFormatException(
                            "malformed input around byte " + pos);
                }
            }
            // The number of chars produced may be less than utflen
            return chararr_pos - charIndex;
        }

        /// <inheritdoc />
        public override int GetMaxByteCount(int charCount)
        {
            return charCount * 3;
        }

        /// <inheritdoc />
        public override int GetMaxCharCount(int byteCount)
        {
            return byteCount;
        }

        //-------------------------------------------------------------------------
        /// <summary>
        /// Calculate the Length in bytes of a string.
        /// 
        /// </summary>
        /// <param Name="str"> the string to find the Length of, not null</param>
        /// <returns>number of bytes</returns>
        public static int GetLengthBytes(String str)
        {
            int bytes = str.Length;
            for (int i = bytes; --i >= 0;)
            {
                int c = str.CharAt(i);
                if (c >= 0x10000)
                {
                    bytes += 3;
                }
                else if (c >= 0x800)
                {
                    bytes += 2;
                }
                else if (c >= 0x80)
                {
                    bytes++;
                }
            }
            return bytes;
        }

        /// <summary>
        /// Calculate the Length in bytes of a string.
        /// 
        /// </summary>
        /// <param Name="str"> the string to find the Length of, not null</param>
        /// <returns>number of bytes</returns>
        public static int GetLengthBytes(char[] str)
        {
            int bytes = str.Length;
            for (int i = bytes; --i >= 0;)
            {
                int c = (int)str[i];
                if (c >= 0x10000)
                {
                    bytes += 3;
                }
                else if (c >= 0x800)
                {
                    bytes += 2;
                }
                else if (c >= 0x80)
                {
                    bytes++;
                }
            }
            return bytes;
        }

        //-------------------------------------------------------------------------
        /// <summary>
        /// Encodes a string into a supplied array.
        /// The array must be at least {@link #getLengthBytes(String)} long for this to succeed.
        /// 
        /// </summary>
        /// <param Name="str"> the string to encode, not null</param>
        /// <param Name="arr"> the array to encode into</param>
        /// <returns>number of bytes written to array</returns>
        /// <exception cref="ArrayIndexOutOfBoundsException">if the target array is not big enough </exception>
        public static int Encode(String str, byte[] arr)
        {
            int len = str.Length;
            int count = 0;
            for (int i = 0; i < len; i++)
            {
                int c = str.CharAt(i);
                if (c >= 0x10000)
                {
                    arr[count++] = (byte)(0xF0 | ((c >> 18) & 0x07));
                    arr[count++] = (byte)(0x80 | ((c >> 12) & 0x3F));
                    arr[count++] = (byte)(0x80 | ((c >> 6) & 0x3F));
                    arr[count++] = (byte)(0x80 | (c & 0x3F));
                }
                else if (c >= 0x800)
                {
                    arr[count++] = (byte)(0xE0 | ((c >> 12) & 0x0F));
                    arr[count++] = (byte)(0x80 | ((c >> 6) & 0x3F));
                    arr[count++] = (byte)(0x80 | (c & 0x3F));
                }
                else if (c >= 0x80)
                {
                    arr[count++] = (byte)(0xC0 | ((c >> 6) & 0x1F));
                    arr[count++] = (byte)(0x80 | (c & 0x3F));
                }
                else
                {
                    arr[count++] = (byte)c;
                }
            }
            return count;
        }

        /// <summary>
        /// Encodes a string into an array.
        /// 
        /// </summary>
        /// <param Name="str"> the string to encode, not null</param>
        /// <returns>byte  the encoding of the string</returns>
        public static byte[] Encode(String str)
        {
            byte[] buffer = new byte[GetLengthBytes(str)];
            Encode(str, buffer);
            return buffer;
        }

        //-------------------------------------------------------------------------
        /// <summary>
        /// Decodes a string from a byte array.
        /// 
        /// </summary>
        /// <param Name="arr"> the byte encoding of a string to convert, not null</param>
        /// <returns>the decoded string, not null</returns>
        /// <exception cref="UTFDataFormatException">if the source does not contain valid UTF-8 </exception>
        public static String Decode(byte[] arr)
        {
            return Decode(arr, 0, arr.Length);
        }

        /// <summary>
        /// Decodes a string from part of a byte array.
        /// 
        /// </summary>
        /// <param Name="arr"> the byte encoding of a string to convert, not null</param>
        /// <param Name="start"> the start index of the UTF-8 string encoding</param>
        /// <param Name="Length"> the number of bytes of UTF-8 data</param>
        /// <returns>the decoded string, not null</returns>
        /// <exception cref="UTFDataFormatException">if the array fragment does not contain valid UTF-8 </exception>
        public static String Decode(byte[] arr, int start, int Length)
        {
            char[]
            buffer = new char[Length];
            int count = 0;
            Length += start;
            for (int i = start; i < Length; i++)
            {
                int c = (int)arr[i] & 0xFF;
                int l = c >> 4;
                if (l < 8)
                {
                    // 1 byte encoding
                    buffer[count++] = (char)c;
                }
                else if (l < 12)
                {
                    // illegal
                    throw new UTFDataFormatException("invalid byte sequence at position " + i);
                }
                else if (l < 14)
                {
                    // 2 byte encoding
                    if (++i >= Length)
                    {
                        throw new UTFDataFormatException("unexpected end of data at position " + i);
                    }
                    int c2 = (int)arr[i] & 0xFF;
                    if ((c2 & 0xC0) != 0x80)
                    {
                        throw new UTFDataFormatException("invalid character in 2-byte sequence at position " + i);
                    }
                    buffer[count++] = (char)(((c & 0x1F) << 6) | (c2 & 0x3F));
                }
                else if (l < 15)
                {
                    // 3 byte encoding
                    if ((i += 2) >= Length)
                    {
                        throw new UTFDataFormatException("unexpected end of data at position " + i);
                    }
                    int c2 = (int)arr[i - 1] & 0xFF;
                    int c3 = (int)arr[i] & 0xFF;
                    if (((c2 & 0xC0) != 0x80) || ((c3 & 0xC0) != 0x80))
                    {
                        throw new UTFDataFormatException("unexpected end of data at position " + i);
                    }
                    buffer[count++] = (char)(((c & 0x0F) << 12) | ((c2 & 0x3F) << 6) | (c3 & 0x3F));
                }
                else
                {
                    // 4 byte encoding
                    if ((i += 3) >= Length)
                    {
                        throw new UTFDataFormatException("unexpected end of data at position " + i);
                    }
                    int c2 = (int)arr[i - 2] & 0xFF;
                    int c3 = (int)arr[i - 1] & 0xFF;
                    int c4 = (int)arr[i] & 0xFF;
                    if (((c2 & 0xC0) != 0x80) || ((c3 & 0xC0) != 0x80) || ((c4 & 0xC0) != 0x80))
                    {
                        throw new UTFDataFormatException("unexpected end of data at position " + i);
                    }
                    buffer[count++] = (char)(((c & 0x07) << 18) | ((c2 & 0x3F) << 12) | ((c3 & 0x3F) << 6) | (c4 & 0x3F));
                }
            }
            return new String(buffer, 0, count);
        }

        /// <summary>
        /// Decodes a string from a {@link DataInput} source.
        /// Note that the methods within {@link DataInput} are designed for <em>modified</em> UTF-8
        /// so can't be used directly with Fudge.
        /// 
        /// </summary>
        /// <param Name="is"> the data source</param>
        /// <param Name="utfLen"> the number of bytes of data to read</param>
        /// <returns>the decoded string, not null</returns>
        /// <exception cref="IOException">if the underlying source raises one or the data is malformed </exception>
        public static String ReadString(StreamReader sr, int utfLen)
        {
            // REVIEW kirk 2009-08-18 -- This can be optimizedd We're copying the data too many
            // timesd Particularly since we expect that most of the time we're reading from
            // a byte array already, duplicating it doesn't make much sense.
            byte[]
            bytearr = new byte[utfLen];
            using (var memstream = new MemoryStream())
            {
                sr.BaseStream.CopyTo(memstream);
                bytearr = memstream.ToArray();
            }

            return Decode(bytearr);
        }

        /// <summary>
        /// Encodes a string to a {@link DataOutput} target.
        /// Note that the methods within {@link DataOutput} are designed for <em>modified</em> UTF-8
        /// so can't be used directly with Fudge.
        /// 
        /// </summary>
        /// <param Name="os"> the data target</param>
        /// <param Name="str"> the string to encode</param>
        /// <returns>number of bytes written</returns>
        /// <exception cref="IOException">if the target raises one </exception>
        public static int WriteString(StreamWriter sw, String str)
        {
            // REVIEW 2010-01-26 Andrew -- Can this be optimised like the readString method? 
            byte[]
            bytearr = Encode(str);
            sw.Write(bytearr);
            return bytearr.Length;
        }

        /// <summary>
        /// <c>UTFDataFormatException</c> is thrown when there is a problem encoding or decoding modified-UTF8 data.
        /// </summary>
        public class UTFDataFormatException : FudgeRuntimeException
        {
            /// <summary>
            /// Constructs a new <c>UTFDataFormatException</c>
            /// </summary>
            /// <param name="message"></param>
            public UTFDataFormatException(string message)
                : base(message)
            {
            }
        }
    }
}
