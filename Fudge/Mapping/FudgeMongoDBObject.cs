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
using System.Collections;
using System.Collections.Generic;
using Fudge.Util;

namespace Fudge.Mapping
{
    //using BSONObject = Newtonsoft.Json.Bson.BsonObject;
	using ObjectId = Newtonsoft.Json.Bson.BsonObjectId;

	//using DBObject = MongoDB.Driver.DBObject;

	/// <summary>
	/// Wraps a <seealso cref="IFudgeFieldContainer"/> and implements the <seealso cref="DBObject"/> interface,
	/// without going through an object conversion stage (as the <seealso cref="MongoDBFudgeBuilder"/> will do).
	/// This class is very much a work in progress. For details on why, please see
	/// http://kirkwylie.blogspot.com/2010/06/performance-of-fudge-persistence-in.html and the comments
	/// from the 10gen team at the bottom.
	/// 
	/// @author Kirk Wylie
	/// </summary>
	public class FudgeMongoDBObject : DBObject
	{
	  private readonly FudgeMsg _underlying;
	  private readonly IDictionary<string, object> _fastSingleValueCache = new Dictionary<string, object>();
	  // This is used A LOT internally in MongoDB. Cache it specifically and avoid all the conversions.
	  private ObjectId _objectId;

	  /// <summary>
	  /// The primary constructor.
	  /// </summary>
	  /// <param name="underlying"> underlying FudgeFieldContainer to be wrapped </param>
	  public FudgeMongoDBObject(IMutableFudgeFieldContainer underlying)
	  {

		if (underlying == null)
		{
		  throw new System.ArgumentException("Must provide an underlying");
		}
		if (!(underlying is FudgeMsg))
		{
		  throw new System.ArgumentException("Underlying must extend FudgeMsgBase");
		}
		_underlying = (FudgeMsg) underlying;
		buildFastSingleValueCache();
	  }

	  /// 
	  private void buildFastSingleValueCache()
	  {
		HashSet<string> fieldNamesToIgnore = new HashSet<string>();
		foreach (IFudgeField field in Underlying.GetAllFields())
		{
		  if (field.Name == null)
		  {
			continue;
		  }
		  if (fieldNamesToIgnore.Contains(field.Name))
		  {
			continue;
		  }
		  if (_fastSingleValueCache.ContainsKey(field.Name))
		  {
			_fastSingleValueCache.Remove(field.Name);
			fieldNamesToIgnore.Add(field.Name);
			continue;
		  }
		  _fastSingleValueCache[field.Name] = convertFudgeToMongoDB(field);
		  if ("_id".Equals(field.Name))
		  {
			_objectId = new ObjectId(((string)field.Value).GetBytes());
		  }
		}
	  }

	  /// <returns> the underlying </returns>
	  public virtual FudgeMsg Underlying
	  {
		  get
		  {
			return _underlying;
		  }
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public override bool containsField(string s)
	  {
		if (_fastSingleValueCache.ContainsKey(s))
		{
		  return true;
		}
		return Underlying.HasField(s);
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public override bool containsKey(string s)
	  {
		return containsField(s);
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public override object get(string key)
	  {
		if ("_id".Equals(key))
		{
		  return _objectId;
		}
		object fastField = _fastSingleValueCache[key];
		if (fastField != null)
		{
		  return fastField;
		}

        IList<IFudgeField> allFields = Underlying.GetAllByName(key);
		if ((allFields == null) || allFields.Count == 0)
		{
		  return null;
		}
		if (allFields.Count > 0)
		{
		  IList<object> listResult = new List<object>(allFields.Count);
		  foreach (IFudgeField field in allFields)
		  {
			listResult.Add(convertFudgeToMongoDB(field));
		  }
		  return listResult;
		}
		else
		{
		  return convertFudgeToMongoDB(allFields[0]);
		}
	  }

	  private static object convertFudgeToMongoDB(IFudgeField field)
	  {
		if (field.Type.TypeId == FudgeTypeDictionary.FUDGE_MSG_TYPE_ID)
		{
		  // Sub-message.
		  return new FudgeMongoDBObject((IMutableFudgeFieldContainer) field.Value);
		}
		else
		{
		  return field.Value;
		}
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public override bool PartialObject
	  {
		  get
		  {
			return false;
		  }
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public override HashSet<string> keySet()
	  {
		return Underlying.AllFieldNames;
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public override void markAsPartialObject()
	  {
		// NOTE kirk 2010-06-14 -- Intentional no-op.
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public Object put(String key, Object v)
	  public override object put(string key, object v)
	  {
		if (v is IList)
		{
		  foreach (object o in (IList) v)
		  {
			put(key, o);
		  }
		}
		else if (v is DBObject)
		{
		  put(key, FudgeContext.GLOBAL_DEFAULT.toFudgeMsg((DBObject) v));
		}
		else if (v is ObjectId)
		{
		  // GROSS HACK HERE. Should be smarter in our fudge use.
		  Underlying.Add(key, ((ObjectId) v).ToString());
		  _objectId = (ObjectId) v;
		}
		else
		{
		  Underlying.Add(key, v);
		}
		return null;
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public override void putAll(BSONObject o)
	  {
		throw new System.NotSupportedException("Put All not yet supported");
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public void putAll(java.util.Map m)
	  public override void putAll(IDictionary m)
	  {
		throw new System.NotSupportedException("Put not yet supported");
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public override object removeField(string key)
	  {
		throw new System.NotSupportedException("Remove not yet supported");
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public java.util.Map toMap()
	  public override IDictionary toMap()
	  {
		IDictionary result = new Hashtable();
		foreach (IFudgeField field in Underlying.GetAllFields())
		{
		  if (field.Name == null)
		  {
			continue;
		  }
		  result[field.Name] = convertFudgeToMongoDB(field);
		}
		return result;
	  }

	}

}