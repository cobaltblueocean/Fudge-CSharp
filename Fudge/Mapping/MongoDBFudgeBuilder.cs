using System.Collections.Generic;

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
using Fudge.Types;
using MongoDB;
using Fudge.Serialization;

namespace Fudge.Mapping
{
    
	using ISecondaryFieldType = Fudge.Types.ISecondaryFieldType;

    //using BasicDBObject = MongoDB.Driver.BasicDBObject;
    using DBObject = MongoDB.Driver.MongoDatabase;

	/// <summary>
	/// <seealso cref="FudgeBuilder"/> instance for encoding and decoding MongoDB objects.
	/// 
	/// @author Andrew Griffin
	/// </summary>
	/* package */	 internal class MongoDBFudgeBuilder : IFudgeBuilder<DBObject>
	 {

	  /// 
	  public static readonly IFudgeBuilder<DBObject> INSTANCE = new MongoDBFudgeBuilder();

	  private MongoDBFudgeBuilder()
	  {
	  }

	  private object decodeObjectValue(FudgeSerializer context, object value)
	  {
		if (value is DBObject)
		{
		  DBObject dbObject = (DBObject) value;
		  return buildMessage(context, dbObject);
		}
		return value;
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public override IMutableFudgeFieldContainer buildMessage(FudgeSerializer context, DBObject dbObject)
	  {
		if (dbObject == null)
		{
		  return null;
		}
		IMutableFudgeFieldContainer msg = context.NewMessage();
		foreach (string key in dbObject.Keys)
		{
		  object value = dbObject.get(key);
//JAVA TO C# CONVERTER TODO TASK: Java wildcard generics are not converted to .NET:
//ORIGINAL LINE: if(value instanceof java.util.List)
		  if (value is IList)
		  {
//JAVA TO C# CONVERTER TODO TASK: Java wildcard generics are not converted to .NET:
//ORIGINAL LINE: for(Object element : (java.util.List) value)
			foreach (object element in (IList) value)
			{
			  msg.add(key, decodeObjectValue(context, element));
			}
		  }
		  else
		  {
			msg.add(key, decodeObjectValue(context, value));
		  }
		}
		return msg;
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") private Object encodePrimitiveFieldValue(final FudgeDeserializationContext context, Object fieldValue)
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
	  private object encodePrimitiveFieldValue(FudgeDeserializer context, object fieldValue)
	  {
//JAVA TO C# CONVERTER TODO TASK: Java wildcard generics are not converted to .NET:
//ORIGINAL LINE: org.fudgemsg.FudgeFieldType valueType = context.getFudgeContext().getTypeDictionary().getByJavaType(fieldValue.getClass());
		FudgeFieldType valueType = context.FudgeContext.TypeDictionary.GetByCSharpType(fieldValue.GetType());
		if (valueType == null)
		{
		  throw new System.ArgumentException("Cannot handle serialization of object " + fieldValue + " of type " + fieldValue.GetType() + " as no Fudge type available in context");
		}

		switch (valueTypetypeId)
		{
		case FudgeTypeDictionary.INDICATOR_TYPE_ID:
		  // REVIEW kirk 2010-08-20 -- Is this the right behavior here?
		  return null;
		case FudgeTypeDictionary.BOOLEAN_TYPE_ID :
		case FudgeTypeDictionary.BYTE_ARR_128_TYPE_ID:
		case FudgeTypeDictionary.BYTE_ARR_16_TYPE_ID:
		case FudgeTypeDictionary.BYTE_ARR_20_TYPE_ID:
		case FudgeTypeDictionary.BYTE_ARR_256_TYPE_ID:
		case FudgeTypeDictionary.BYTE_ARR_32_TYPE_ID:
		case FudgeTypeDictionary.BYTE_ARR_4_TYPE_ID:
		case FudgeTypeDictionary.BYTE_ARR_512_TYPE_ID:
		case FudgeTypeDictionary.BYTE_ARR_64_TYPE_ID:
		case FudgeTypeDictionary.BYTE_ARR_8_TYPE_ID:
		case FudgeTypeDictionary.BYTE_ARRAY_TYPE_ID:
		case FudgeTypeDictionary.BYTE_TYPE_ID:
		case FudgeTypeDictionary.DOUBLE_ARRAY_TYPE_ID:
		case FudgeTypeDictionary.DOUBLE_TYPE_ID:
		case FudgeTypeDictionary.FLOAT_ARRAY_TYPE_ID:
		case FudgeTypeDictionary.FLOAT_TYPE_ID:
		case FudgeTypeDictionary.INT_ARRAY_TYPE_ID:
		case FudgeTypeDictionary.INT_TYPE_ID:
		case FudgeTypeDictionary.LONG_ARRAY_TYPE_ID:
		case FudgeTypeDictionary.LONG_TYPE_ID:
		case FudgeTypeDictionary.SHORT_ARRAY_TYPE_ID:
		case FudgeTypeDictionary.SHORT_TYPE_ID:
		case FudgeTypeDictionary.STRING_TYPE_ID:
		  if (valueType is ISecondaryFieldType)
		  {
			ISecondaryFieldType secondaryType = (ISecondaryFieldType) valueType;
			return secondaryType.secondaryToPrimary(fieldValue);
		  }
		  // Built-in support.
		  return fieldValue;
		case FudgeTypeDictionary.DATE_TYPE_ID:
		case FudgeTypeDictionary.DATETIME_TYPE_ID:
		case FudgeTypeDictionary.TIME_TYPE_ID:
		  // FIXME kirk 2010-08-20 -- This is an insanely gross hack around the rest of the
		  // fix for FRJ-83 breaking all dates, exposed by FRJ-84.
		  return fieldValue;
		}
		// If we get this far, it's a user-defined type. Nothing we can do here.
		throw new IllegalStateException("User-defined types must be handled before they get to MongoDBFudgeBuilder currently. Value type " + valueType);
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: private Object encodeFieldValue(final FudgeDeserializationContext context, final Object currentValue, Object fieldValue)
	  private object encodeFieldValue(FudgeDeserializer context, object currentValue, object fieldValue)
	  {
		bool structureExpected = false;
		if (fieldValue is IFudgeFieldContainer)
		{
		  fieldValue = buildObject(context, (IFudgeFieldContainer) fieldValue);
		  structureExpected = true;
		}
//JAVA TO C# CONVERTER TODO TASK: Java wildcard generics are not converted to .NET:
//ORIGINAL LINE: if(currentValue instanceof java.util.List)
		if (currentValue is IList)
		{
//JAVA TO C# CONVERTER TODO TASK: Java wildcard generics are not converted to .NET:
//ORIGINAL LINE: java.util.List<Object> l = new java.util.ArrayList<Object>((java.util.List)(currentValue));
		  IList<object> l = new List<object>((IList)(currentValue));
		  l.Add(fieldValue);
		  return l;
		}
		else if (currentValue != null)
		{
		  IList<object> l = new List<object>();
		  l.Add(currentValue);
		  if (!structureExpected)
		  {
			fieldValue = encodePrimitiveFieldValue(context, fieldValue);
		  }
		  l.Add(fieldValue);
		  return l;
		}

		if (structureExpected)
		{
		  return fieldValue;
		}

		return encodePrimitiveFieldValue(context, fieldValue);
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public override DBObject buildObject(FudgeDeserializer context, IFudgeFieldContainer fields)
	  {
		if (fields == null)
		{
		  return null;
		}
		BasicDBObject dbObject = new BasicDBObject();

		foreach (IFudgeField field in fields.AllFields)
		{
		  if (field.Name == null)
		  {
			if (field.Ordinal == 0)
			{
			  continue;
			}
			// REVIEW kirk 2009-10-22 -- Should this be configurable so that it just
			// silently drops unnamed fields?
			throw new System.ArgumentException("Field encountered without a name (" + field + ")");
		  }
		  object value = field.Value;
		  value = encodeFieldValue(context, dbObject.get(field.Name), value);
		  dbObject.put(field.Name, value);
		}

		return dbObject;
	  }

	 }
}