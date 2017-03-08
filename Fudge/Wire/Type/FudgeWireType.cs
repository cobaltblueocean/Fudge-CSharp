/// <summary>
/// Copyright (C) 2009 - present by OpenGamma Inc. and other contributors.
/// 
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
/// 
///     http://www.apache.org/licenses/LICENSE-2.0
///     
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
/// </summary>
using System;
using System.IO;

namespace Fudge.Wire.Types
{


	/// <summary>
	/// The wire type of a field as defined by the Fudge encoding specification.
	/// <para>
	/// In order to efficiently send messages, Fudge needs to know the type of each piece of data.
	/// A standard set of types is supported by all Fudge-compliant systems.
	/// This set may be extended with custom types within a closed Fudge implementation.
	/// Custom types must be registered with <seealso cref="FudgeTypeDictionary"/>.
	/// </para>
	/// <para>
	/// This class is not final but is thread-safe in isolation.
	/// Subclasses must be immutable and thread-safe.
	/// </para>
	/// </summary>
    [Serializable]
    public abstract class FudgeWireType : FudgeFieldType
    {
        #region "static type list"
        /// <summary>
        /// Wire type id: indicator.
        /// See <a href="http://wiki.fudgemsg.org/display/FDG/Types">Fudge Types</a> for more details.
        /// </summary>
        public static readonly sbyte INDICATOR_TYPE_ID = (sbyte)0;
        /// <summary>
        /// Wire type id: boolean.
        /// See <a href="http://wiki.fudgemsg.org/display/FDG/Types">Fudge Types</a> for more details.
        /// </summary>
        public static readonly sbyte BOOLEAN_TYPE_ID = (sbyte)1;
        /// <summary>
        /// Wire type id: 8-bit signed integer.
        /// See <a href="http://wiki.fudgemsg.org/display/FDG/Types">Fudge Types</a> for more details.
        /// </summary>
        public static readonly sbyte BYTE_TYPE_ID = (sbyte)2;
        /// <summary>
        /// Wire type id: 16-bit signed integer.
        /// See <a href="http://wiki.fudgemsg.org/display/FDG/Types">Fudge Types</a> for more details.
        /// </summary>
        public static readonly sbyte SHORT_TYPE_ID = (sbyte)3;
        /// <summary>
        /// Wire type id: 32-bit signed integer.
        /// See <a href="http://wiki.fudgemsg.org/display/FDG/Types">Fudge Types</a> for more details.
        /// </summary>
        public static readonly sbyte INT_TYPE_ID = (sbyte)4;
        /// <summary>
        /// Wire type id: 64-bit signed integer.
        /// See <a href="http://wiki.fudgemsg.org/display/FDG/Types">Fudge Types</a> for more details.
        /// </summary>
        public static readonly sbyte LONG_TYPE_ID = (sbyte)5;
        /// <summary>
        /// Wire type id: byte array.
        /// See <a href="http://wiki.fudgemsg.org/display/FDG/Types">Fudge Types</a> for more details.
        /// </summary>
        public static readonly sbyte BYTE_ARRAY_TYPE_ID = (sbyte)6;
        /// <summary>
        /// Wire type id: array of 16-bit signed integers.
        /// See <a href="http://wiki.fudgemsg.org/display/FDG/Types">Fudge Types</a> for more details.
        /// </summary>
        public static readonly sbyte SHORT_ARRAY_TYPE_ID = (sbyte)7;
        /// <summary>
        /// Wire type id: array of 32-bit signed integers.
        /// See <a href="http://wiki.fudgemsg.org/display/FDG/Types">Fudge Types</a> for more details.
        /// </summary>
        public static readonly sbyte INT_ARRAY_TYPE_ID = (sbyte)8;
        /// <summary>
        /// Wire type id: array of 64-bit signed integers.
        /// See <a href="http://wiki.fudgemsg.org/display/FDG/Types">Fudge Types</a> for more details.
        /// </summary>
        public static readonly sbyte LONG_ARRAY_TYPE_ID = (sbyte)9;
        /// <summary>
        /// Wire type id: 32-bit floating point.
        /// See <a href="http://wiki.fudgemsg.org/display/FDG/Types">Fudge Types</a> for more details.
        /// </summary>
        public static readonly sbyte FLOAT_TYPE_ID = (sbyte)10;
        /// <summary>
        /// Wire type id: 64-bit floating point.
        /// See <a href="http://wiki.fudgemsg.org/display/FDG/Types">Fudge Types</a> for more details.
        /// </summary>
        public static readonly sbyte DOUBLE_TYPE_ID = (sbyte)11;
        /// <summary>
        /// Wire type id: array of 32-bit floating point.
        /// See <a href="http://wiki.fudgemsg.org/display/FDG/Types">Fudge Types</a> for more details.
        /// </summary>
        public static readonly sbyte FLOAT_ARRAY_TYPE_ID = (sbyte)12;
        /// <summary>
        /// Wire type id: array of 64-bit floating point.
        /// See <a href="http://wiki.fudgemsg.org/display/FDG/Types">Fudge Types</a> for more details.
        /// </summary>
        public static readonly sbyte DOUBLE_ARRAY_TYPE_ID = (sbyte)13;
        /// <summary>
        /// Wire type id: string.
        /// See <a href="http://wiki.fudgemsg.org/display/FDG/Types">Fudge Types</a> for more details.
        /// </summary>
        public static readonly sbyte STRING_TYPE_ID = (sbyte)14;
        /// <summary>
        /// Wire type id: embedded Fudge sub-message.
        /// See <a href="http://wiki.fudgemsg.org/display/FDG/Types">Fudge Types</a> for more details.
        /// </summary>
        public static readonly sbyte SUB_MESSAGE_TYPE_ID = (sbyte)15;
        // End message indicator type removed as unnecessary, hence no 16
        /// <summary>
        /// Wire type id: byte array of length 4.
        /// See <a href="http://wiki.fudgemsg.org/display/FDG/Types">Fudge Types</a> for more details.
        /// </summary>
        public static readonly sbyte BYTE_ARRAY_4_TYPE_ID = (sbyte)17;
        /// <summary>
        /// Wire type id: byte array of length 8.
        /// See <a href="http://wiki.fudgemsg.org/display/FDG/Types">Fudge Types</a> for more details.
        /// </summary>
        public static readonly sbyte BYTE_ARRAY_8_TYPE_ID = (sbyte)18;
        /// <summary>
        /// Wire type id: byte array of length 16.
        /// See <a href="http://wiki.fudgemsg.org/display/FDG/Types">Fudge Types</a> for more details.
        /// </summary>
        public static readonly sbyte BYTE_ARRAY_16_TYPE_ID = (sbyte)19;
        /// <summary>
        /// Wire type id: byte array of length 20.
        /// See <a href="http://wiki.fudgemsg.org/display/FDG/Types">Fudge Types</a> for more details.
        /// </summary>
        public static readonly sbyte BYTE_ARRAY_20_TYPE_ID = (sbyte)20;
        /// <summary>
        /// Wire type id: byte array of length 32.
        /// See <a href="http://wiki.fudgemsg.org/display/FDG/Types">Fudge Types</a> for more details.
        /// </summary>
        public static readonly sbyte BYTE_ARRAY_32_TYPE_ID = (sbyte)21;
        /// <summary>
        /// Wire type id: byte array of length 64.
        /// See <a href="http://wiki.fudgemsg.org/display/FDG/Types">Fudge Types</a> for more details.
        /// </summary>
        public static readonly sbyte BYTE_ARRAY_64_TYPE_ID = (sbyte)22;
        /// <summary>
        /// Wire type id: byte array of length 128.
        /// See <a href="http://wiki.fudgemsg.org/display/FDG/Types">Fudge Types</a> for more details.
        /// </summary>
        public static readonly sbyte BYTE_ARRAY_128_TYPE_ID = (sbyte)23;
        /// <summary>
        /// Wire type id: byte array of length 256.
        /// See <a href="http://wiki.fudgemsg.org/display/FDG/Types">Fudge Types</a> for more details.
        /// </summary>
        public static readonly sbyte BYTE_ARRAY_256_TYPE_ID = (sbyte)24;
        /// <summary>
        /// Wire type id: byte array of length 512.
        /// See <a href="http://wiki.fudgemsg.org/display/FDG/Types">Fudge Types</a> for more details.
        /// </summary>
        public static readonly sbyte BYTE_ARRAY_512_TYPE_ID = (sbyte)25;
        /// <summary>
        /// Wire type id: date.
        /// See <a href="http://wiki.fudgemsg.org/display/FDG/Types">Fudge Types</a> for more details.
        /// </summary>
        public static readonly sbyte DATE_TYPE_ID = (sbyte)26;
        /// <summary>
        /// Wire type id: time.
        /// See <a href="http://wiki.fudgemsg.org/display/FDG/Types">Fudge Types</a> for more details.
        /// </summary>
        public static readonly sbyte TIME_TYPE_ID = (sbyte)27;
        /// <summary>
        /// Wire type id: combined date and time.
        /// See <a href="http://wiki.fudgemsg.org/display/FDG/Types">Fudge Types</a> for more details.
        /// </summary>
        public static readonly sbyte DATETIME_TYPE_ID = (sbyte)28;

        /// <summary>
        /// Wire type: embedded sub-message.
        /// See <seealso cref="#SUB_MESSAGE_TYPE_ID"/>.
        /// </summary>
        public static readonly FudgeWireType SUB_MESSAGE = SubMessageWireType.Instance;
        /// <summary>
        /// Wire type: indicator.
        /// See <seealso cref="#INDICATOR_TYPE_ID"/>.
        /// </summary>
        public static readonly FudgeWireType INDICATOR = IndicatorWireType.Instance;
        /// <summary>
        /// Wire type: boolean.
        /// See <seealso cref="#BOOLEAN_TYPE_ID"/>.
        /// </summary>
        public static readonly FudgeWireType BOOLEAN = BooleanWireType.Instance;
        /// <summary>
        /// Wire type: boolean.
        /// See <seealso cref="#BYTE_TYPE_ID"/>.
        /// </summary>
        public static readonly FudgeWireType BYTE = ByteWireType.Instance;
        /// <summary>
        /// Wire type: boolean.
        /// See <seealso cref="#SHORT_TYPE_ID"/>.
        /// </summary>
        public static readonly FudgeWireType SHORT = ShortWireType.Instance;
        /// <summary>
        /// Wire type: boolean.
        /// See <seealso cref="#INT_TYPE_ID"/>.
        /// </summary>
        public static readonly FudgeWireType INT = IntWireType.Instance;
        /// <summary>
        /// Wire type: boolean.
        /// See <seealso cref="#LONG_TYPE_ID"/>.
        /// </summary>
        public static readonly FudgeWireType LONG = LongWireType.Instance;
        /// <summary>
        /// Wire type: boolean.
        /// See <seealso cref="#FLOAT_TYPE_ID"/>.
        /// </summary>
        public static readonly FudgeWireType FLOAT = FloatWireType.Instance;
        /// <summary>
        /// Wire type: boolean.
        /// See <seealso cref="#DOUBLE_TYPE_ID"/>.
        /// </summary>
        public static readonly FudgeWireType DOUBLE = DoubleWireType.Instance;
        /// <summary>
        /// Wire type: string.
        /// See <seealso cref="#STRING_TYPE_ID"/>.
        /// </summary>
        public static readonly FudgeWireType STRING = StringWireType.Instance;
        /// <summary>
        /// Wire type: short array.
        /// See <seealso cref="#SHORT_ARRAY_TYPE_ID"/>.
        /// </summary>
        public static readonly FudgeWireType SHORT_ARRAY = ShortArrayWireType.Instance;
        /// <summary>
        /// Wire type: int array.
        /// See <seealso cref="#INT_ARRAY_TYPE_ID"/>.
        /// </summary>
        public static readonly FudgeWireType INT_ARRAY = IntArrayWireType.Instance;
        /// <summary>
        /// Wire type: long array.
        /// See <seealso cref="#LONG_ARRAY_TYPE_ID"/>.
        /// </summary>
        public static readonly FudgeWireType LONG_ARRAY = LongArrayWireType.Instance;
        /// <summary>
        /// Wire type: float array.
        /// See <seealso cref="#FLOAT_ARRAY_TYPE_ID"/>.
        /// </summary>
        public static readonly FudgeWireType FLOAT_ARRAY = FloatArrayWireType.Instance;
        /// <summary>
        /// Wire type: double array.
        /// See <seealso cref="#DOUBLE_ARRAY_TYPE_ID"/>.
        /// </summary>
        public static readonly FudgeWireType DOUBLE_ARRAY = DoubleArrayWireType.Instance;
        /// <summary>
        /// Wire type: arbitrary length byte array.
        /// See <seealso cref="#BYTE_ARRAY_TYPE_ID"/>.
        /// </summary>
        public static readonly FudgeWireType BYTE_ARRAY = ByteArrayWireTypes.VARIABLE_SIZED_INSTANCE;
        /// <summary>
        /// Wire type: byte array of length 4.
        /// See <seealso cref="#BYTE_ARRAY_4_TYPE_ID"/>.
        /// </summary>
        public static readonly FudgeWireType BYTE_ARRAY_4 = ByteArrayWireTypes.LENGTH_4_INSTANCE;
        /// <summary>
        /// Wire type: byte array of length 8.
        /// See <seealso cref="#BYTE_ARRAY_8_TYPE_ID"/>.
        /// </summary>
        public static readonly FudgeWireType BYTE_ARRAY_8 = ByteArrayWireTypes.LENGTH_8_INSTANCE;
        /// <summary>
        /// Wire type: byte array of length 16.
        /// See <seealso cref="#BYTE_ARRAY_16_TYPE_ID"/>.
        /// </summary>
        public static readonly FudgeWireType BYTE_ARRAY_16 = ByteArrayWireTypes.LENGTH_16_INSTANCE;
        /// <summary>
        /// Wire type: byte array of length 20.
        /// See <seealso cref="#BYTE_ARRAY_20_TYPE_ID"/>.
        /// </summary>
        public static readonly FudgeWireType BYTE_ARRAY_20 = ByteArrayWireTypes.LENGTH_20_INSTANCE;
        /// <summary>
        /// Wire type: byte array of length 32.
        /// See <seealso cref="#BYTE_ARRAY_32_TYPE_ID"/>.
        /// </summary>
        public static readonly FudgeWireType BYTE_ARRAY_32 = ByteArrayWireTypes.LENGTH_32_INSTANCE;
        /// <summary>
        /// Wire type: byte array of length 64.
        /// See <seealso cref="#BYTE_ARRAY_64_TYPE_ID"/>.
        /// </summary>
        public static readonly FudgeWireType BYTE_ARRAY_64 = ByteArrayWireTypes.LENGTH_64_INSTANCE;
        /// <summary>
        /// Wire type: byte array of length 128.
        /// See <seealso cref="#BYTE_ARRAY_128_TYPE_ID"/>.
        /// </summary>
        public static readonly FudgeWireType BYTE_ARRAY_128 = ByteArrayWireTypes.LENGTH_128_INSTANCE;
        /// <summary>
        /// Wire type: byte array of length 256.
        /// See <seealso cref="#BYTE_ARRAY_256_TYPE_ID"/>.
        /// </summary>
        public static readonly FudgeWireType BYTE_ARRAY_256 = ByteArrayWireTypes.LENGTH_256_INSTANCE;
        /// <summary>
        /// Wire type: byte array of length 512.
        /// See <seealso cref="#BYTE_ARRAY_512_TYPE_ID"/>.
        /// </summary>
        public static readonly FudgeWireType BYTE_ARRAY_512 = ByteArrayWireTypes.LENGTH_512_INSTANCE;
        /// <summary>
        /// Wire type: date.
        /// See <seealso cref="#TIME_TYPE_ID"/>.
        /// </summary>
        public static readonly FudgeWireType DATE = DateWireType.Instance;
        /// <summary>
        /// Wire type: date.
        /// See <seealso cref="#TIME_TYPE_ID"/>.
        /// </summary>
        public static readonly FudgeWireType TIME = TimeWireType.Instance;
        /// <summary>
        /// Wire type: combined date and time.
        /// See <seealso cref="#DATETIME_TYPE_ID"/>.
        /// </summary>
        public static readonly FudgeWireType DATETIME = DateTimeWireType.Instance;

        #endregion

        /// <summary>
        /// Creates a new variable width wire type when the type is unknown.
        /// </summary>
        /// <param name="typeId">  the wire type identifier </param>
        /// <returns> the wire type, not null </returns>
        public static FudgeWireType unknown(int typeId)
        {
            return new UnknownWireType(typeId);
        }

        /// <summary>
        /// Creates a new fixed width wire type when the type is unknown.
        /// </summary>
        /// <param name="typeId">  the wire type identifier </param>
        /// <param name="fixedWidth">  the fixed width </param>
        /// <returns> the wire type, not null </returns>
        public static FudgeWireType unknown(int typeId, int fixedWidth)
        {
            return new UnknownWireType(typeId, fixedWidth);
        }

        /// <summary>
        /// Chooses the best wire type for the specified byte array.
        /// <para>
        /// There are byte array wire types for a variety of common lengths.
        /// This method chooses the most efficient to use.
        /// 
        /// </para>
        /// </summary>
        /// <param name="array">  the array to choose a wire type for, null returns the variable length type </param>
        /// <returns> the most efficient wire type available, not null </returns>
        public static FudgeWireType bestMatchByteArray(sbyte[] array)
        {
            if (array == null)
            {
                return BYTE_ARRAY;
            }
            switch (array.Length)
            {
                case 4:
                    return BYTE_ARRAY_4;
                case 8:
                    return BYTE_ARRAY_8;
                case 16:
                    return BYTE_ARRAY_16;
                case 20:
                    return BYTE_ARRAY_20;
                case 32:
                    return BYTE_ARRAY_32;
                case 64:
                    return BYTE_ARRAY_64;
                case 128:
                    return BYTE_ARRAY_128;
                case 256:
                    return BYTE_ARRAY_256;
                case 512:
                    return BYTE_ARRAY_512;
                default:
                    return BYTE_ARRAY;
            }
        }

        //-------------------------------------------------------------------------
        //  /**
        //   * The Fudge type id, from the specification.
        //   */
        //  private final int _typeId;
        //  /**
        //   * The standard Java equivalent type.
        //   */
        //  private final Class _javaType;
        //  /**
        //   * Whether the type is sent as a variable size in the protocol.
        //   */
        //  private final boolean _isVariableSize;
        //  /**
        //   * The size of the type in bytes when the size is fixed.
        //   */
        //  private final int _fixedSize;

        /// <summary>
        /// Constructs a new variable width wire type based on the underlying Java type.
        /// <para>
        /// The Fudge type identifier must be unique within the <seealso cref="FudgeTypeDictionary"/>.
        /// 
        /// </para>
        /// </summary>
        /// <param name="typeId">  the type dictionary unique type identifier, from 0 to 255 </param>
        /// <param name="javaType">  the underlying Java type, not null </param>
        protected internal FudgeWireType(int typeId, Type javaType)
            : base(typeId, javaType, true, 0)
        {
            //    ArgumentChecker.notNull(javaType, "Java type must not be null");
            //    if (typeId < 0 || typeId > 255) {
            //      throw new IllegalArgumentException("The type id must fit in an unsigned byte");
            //    }
            //    _typeId = typeId;
            //    _javaType = javaType;
            //    _isVariableSize = true;
            //    _fixedSize = 0;
        }

        /// <summary>
        /// Constructs a new fixed width wire type based on the underlying Java type.
        /// <para>
        /// The Fudge type identifier must be unique within the <seealso cref="FudgeTypeDictionary"/>.
        /// 
        /// </para>
        /// </summary>
        /// <param name="typeId">  the type dictionary unique type identifier, from 0 to 255 </param>
        /// <param name="javaType">  the underlying Java type, not null </param>
        /// <param name="fixedSize">  the size in bytes if fixed size, zero for variable width </param>
        protected internal FudgeWireType(int typeId, Type javaType, int fixedSize)
            : base(typeId, javaType, false, fixedSize)
        {
            //    ArgumentChecker.notNull(javaType, "Java type must not be null");
            //    if (typeId < 0 || typeId > 255) {
            //      throw new IllegalArgumentException("The type id must fit in an unsigned byte");
            //    }
            //    _typeId = typeId;
            //    _javaType = javaType;
            //    _isVariableSize = false;
            //    _fixedSize = fixedSize;
        }

        //  //-------------------------------------------------------------------------
        //  /**
        //   * Gets the Fudge wire type identifier.
        //   * <p>
        //   * This is the unsigned byte used on the wire to identify the type.
        //   * 
        //   * @return the type identifier, from 0 to 255
        //   */
        //  public final int getTypeId() {
        //    return _typeId;
        //  }
        //
        //  /**
        //   * Gets the standard Java type for values of this type.
        //   * 
        //   * @return the standard Java type, not null
        //   */
        //  public final Class getJavaType() {
        //    return _javaType;
        //  }
        //
        //  /**
        //   * Checks if the type has a variable width.
        //   * 
        //   * @return true if variable width, false for fixed width
        //   */
        //  public final boolean isVariableSize() {
        //    return _isVariableSize;
        //  }
        //
        //  /**
        //   * Checks if the type has a fixed width.
        //   * 
        //   * @return true if variable width, false for fixed width
        //   */
        //  public final boolean isFixedSize() {
        //    return !_isVariableSize;
        //  }
        //
        //  /**
        //   * Gets the number of bytes used to encode a value if the type is fixed width.
        //   * 
        //   * @return the fixed width size in bytes, zero if variable width
        //   */
        //  public final int getFixedSize() {
        //    return _fixedSize;
        //  }

        /// <summary>
        /// Checks if the type is registered and known.
        /// </summary>
        /// <returns> true if type is known </returns>
        public bool TypeKnown
        {
            get
            {
                return !TypeUnknown;
            }
        }

        /// <summary>
        /// Checks if the type is unregistered and unknown.
        /// </summary>
        /// <returns> true if type is unknown </returns>
        public bool TypeUnknown
        {
            get
            {
                return this is UnknownWireType;
            }
        }

        //  //-------------------------------------------------------------------------
        //  /**
        //   * Gets the number of bytes used to encode a value.
        //   * <p>
        //   * A variable width type must override this method.
        //   * A fixed width type will return the {@link #getFixedSize() fixed size}.
        //   * 
        //   * @param value  the value to check, not used for fixed width types
        //   * @param taxonomy  the taxonomy being used for the encoding, not used for fixed width types
        //   * @return the size in bytes
        //   */
        //  public int getSize(Object value, FudgeTaxonomy taxonomy) {
        //    if (isVariableSize()) {
        //      throw new UnsupportedOperationException("This method must be overridden for variable size types");
        //    }
        //    return getFixedSize();
        //  }
        //
        //  //-------------------------------------------------------------------------
        //  /**
        //   * Writes a value of this type to the output.
        //   * <p>
        //   * This is intended for use by variable width types and must write the given value.
        //   * The implementation must write exactly the number of bytes returned by the
        //   * {@link #getSize(Object,FudgeTaxonomy) size calculation}.
        //   * 
        //   * @param output  the output target to write the value to, not null
        //   * @param value  the value to write
        //   * @throws IOException if an error occurs, which must be wrapped by the caller
        //   */
        //  public abstract void WriteValue(BinaryWriter output, Object value) throws IOException;
        //
        //  /**
        //   * Reads a value of this type to the output.
        //   * <p>
        //   * This is intended for use by variable width types and must read the given value.
        //   * The implementation must read exactly the number of bytes passed into the method.
        //   * 
        //   * @param input  the input source to read the value from, not null
        //   * @param dataSize  the number of bytes of data to read
        //   * @return the value that was read
        //   * @throws IOException if an error occurs, which must be wrapped by the caller
        //   */
        //  public abstract Object ReadValue(BinaryReader input, int dataSize) throws IOException;

        //  //-------------------------------------------------------------------------
        //  /**
        //   * Checks if this type equals another at the wire type level.
        //   * <p>
        //   * Note that this only checks the wire type identifier, not the Java type.
        //   * 
        //   * @param obj  the object to compare to, null returns false
        //   * @return true if equal
        //   */
        //  @Override
        //  public final boolean equals(Object obj) {
        //    if (obj == this) {
        //      return true;
        //    }
        //    if (obj instanceof FudgeWireType) {
        //      FudgeWireType other = (FudgeWireType) obj;
        //      return getTypeId() == other.getTypeId(); // assume system is correctly setup and type is unique
        //    }
        //    return false;
        //  }
        //
        //  /**
        //   * Gets a suitable hash code.
        //   * 
        //   * @return the hash code
        //   */
        //  @Override
        //  public final int hashCode() {
        //    return getTypeId();
        //  }
        //
        //  /**
        //   * Returns a description of the type.
        //   * 
        //   * @return the descriptive string, not null
        //   */
        //  @Override
        //  public final String toString() {
        //    return "FudgeWireType[" + getTypeId() + "-" + getJavaType() + "]";
        //  }

    }
}