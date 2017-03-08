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
using System.Collections.Generic;
using Fudge.Serialization;
using System.Security;
using System.Reflection;

namespace Fudge.Mapping
{
	/// <summary>
	/// A message and object builder implementation using the BeanUtils tools to work with
	/// Java Beans.
	/// </summary>
	/// @param <T> Bean class that can be serialized or deserialized using this builder
	/// @author Andrew Griffin </param>
	/* package */	 internal class DotNetAssemblyBuilder<T> : IFudgeBuilder<T>
	 {

	  private readonly PropertyInfo[] _properties;
	  private readonly string _assemblyName;
	  private readonly Type _assemblyType;

	  /// <summary>
	  /// Creates a new <seealso cref="DotNetAssemblyBuilder"/> for a class.
	  /// </summary>
	  /// @param <T> class the builder should process </param>
	  /// <param name="clazz"> class the builder should process </param>
	  /// <returns> the {@code DotNetAssemblyBuilder} </returns>
	  /* package *///JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: static <T> DotNetAssemblyBuilder<T> create(final Class clazz)
      internal static DotNetAssemblyBuilder<T> create<T>(Type clazz)
      {
          // customise the properties
          // The original code is using PropertyUtility for allowing Java annotation to create anc access Properties,
          // but .NET has native feature of Property, so, just copy the properties from the assembly to the list.
          List<PropertyInfo> propList = new List<PropertyInfo>();
          foreach (PropertyInfo prop in clazz.GetProperties())
          {
              //Process if the property is Serializable,
              // in other word, give up if it's a transient property
              if (prop.GetType().IsSerializable)
              {
                  propList.Add(prop);
              }
          }
          // try and find a constructor
          try
          {
              return new DotNetAssemblyBuilder<T>(propList.ToArray(), clazz);
          }
          catch (SecurityException)
          {
              // ignore
          }
          catch (NotImplementedException)
          {
              // ignore
          }
          // otherwise bean behaviour (about 5 times slower!)
          //JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
          return new DotNetAssemblyBuilder<T>(propList.ToArray(), clazz.FullName);
      }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: private DotNetAssemblyBuilder(final JBProperty[] properties, final String beanName)
	  private DotNetAssemblyBuilder(PropertyInfo[] properties, string beanName)
	  {
		_properties = properties;
		_assemblyName = beanName;
		_assemblyType = null;
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: private DotNetAssemblyBuilder(final JBProperty[] properties, final Constructor<T> constructor)
      private DotNetAssemblyBuilder(PropertyInfo[] properties, Type assemblyType)
	  {
		_properties = properties;
		_assemblyName = null;
		_assemblyType = assemblyType;
	  }

      private PropertyInfo[] Properties
	  {
		  get
		  {
			return _properties;
		  }
	  }

	  private string BeanName
	  {
		  get
		  {
			return _assemblyName;
		  }
	  }

	  private Type AssemblyType
	  {
		  get
		  {
			return _assemblyType;
		  }
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") private T newBeanObject() throws IllegalArgumentException, InstantiationException, IllegalAccessException, InvocationTargetException, java.io.IOException, TypeLoadException
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
	  private T newAssemblyObject()
	  {
		if (AssemblyType != null)
		{
            return (T)Activator.CreateInstance(_assemblyType.GetType());
		}
		else
		{
            var asm = Assembly.GetCallingAssembly();
            var typeInfo = asm.GetType(_assemblyName);
            return (T)Activator.CreateInstance(typeInfo);
		}
	  }

	  /// 
	  public override IMutableFudgeFieldContainer buildMessage(FudgeSerializer context, T @object)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final org.fudgemsg.IMutableFudgeFieldContainer message = context.newMessage();
		IMutableFudgeFieldContainer message = context.NewMessage();
		try
		{
		  foreach (PropertyInfo prop in Properties)
		  {
			if (!prop.CanRead)
			{
				continue;
			}
            
            FudgeFieldName fudgeFieldName = (FudgeFieldName)Attribute.GetCustomAttribute(prop, typeof(FudgeFieldName));
            FudgeFieldOrdinal fudgeFieldOrdinal = (FudgeFieldOrdinal)Attribute.GetCustomAttribute(prop, typeof(FudgeFieldOrdinal));

            context.ObjectToFudgeMsgWithClassHeaders(message, prop.Name, fudgeFieldOrdinal.Value, prop, prop.GetType());
		  }
		}
		catch (System.ArgumentException e)
		{
		  throw new FudgeRuntimeException("Couldn't serialise " + @object, e);
		}
        catch (MissingMethodException e)
		{
		  throw new FudgeRuntimeException("Couldn't serialise " + @object, e);
		}
        catch (TargetInvocationException e)
		{
		  throw new FudgeRuntimeException("Couldn't serialise " + @object, e);
		}
		return message;
	  }

	  /// 
	  public override T buildObject(FudgeDeserializer context, IFudgeFieldContainer message)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final T object;
		T @object;
		try
		{
		  @object = newAssemblyObject();
          foreach (PropertyInfo prop in Properties)
		  {
			if (!prop.CanRead)
			{
				continue;
			}
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final org.fudgemsg.FudgeField field;
			IFudgeField field;
            FudgeFieldOrdinal fudgeFieldOrdinal = (FudgeFieldOrdinal)Attribute.GetCustomAttribute(prop, typeof(FudgeFieldOrdinal));

            if (fudgeFieldOrdinal == null)
			{
			  field = message.GetByName(prop.Name);
			}
			else
			{
                field = message.GetByOrdinal(fudgeFieldOrdinal.Value);
			}
			if (field == null)
			{
				continue;
			}
			prop.SetValue(@object, context.fieldValueToObject(field));
		  }
		}
        catch (System.IO.IOException e)
		{
		  throw new FudgeRuntimeException("Couldn't deserialise " + BeanName, e);
		}
		catch (TypeLoadException e)
		{
		  throw new FudgeRuntimeException("Couldn't deserialise " + BeanName, e);
		}
		catch (System.ArgumentException e)
		{
		  throw new FudgeRuntimeException("Couldn't deserialise " + BeanName, e);
		}
        catch (MissingMethodException e)
		{
		  throw new FudgeRuntimeException("Couldn't deserialise " + BeanName, e);
		}
        catch (TargetInvocationException e)
		{
		  throw new FudgeRuntimeException("Couldn't deserialise " + BeanName, e);
		}
		return @object;
	  }
	 }
}