using System;

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

namespace Fudge.Mapping
{


	/// <summary>
	/// An annotation that can be placed on any Java object to indicate that there is an
	/// instance of <seealso cref="IFudgeMessageBuilder"/> or <seealso cref="IFudgeObjectBuilder"/> (or a full
	/// <seealso cref="FudgeBuilder"/>) for that type.
	/// This will then be picked up at runtime and automatically configured for that type.
	/// <p/>
	/// All parameters are optional, and only have default values of <seealso cref="Object"/> for convenience.
	/// 
	/// @author Kirk Wylie
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Documented @Retention(RetentionPolicy.RUNTIME) @Target(ElementType.GetType()) @Inherited public class HasFudgeBuilder extends System.Attribute
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class HasFudgeBuilder : System.Attribute
    {
        private Type _builder = typeof(object);
        private Type _objectBuilder = typeof(object);
        private Type _messageBuilder = typeof(object);

        /// <summary>
        /// A class that implements both <seealso cref="IFudgeMessageBuilder"/> and <seealso cref="IFudgeObjectBuilder"/>
        /// for the annotated type.
        /// Convenience method (much like the convenience type <seealso cref="FudgeBuilder"/>). 
        /// </summary>
        public Type builder
        {
            get { return _builder; }
            set { _builder = value; }
        }

        /// <summary>
        /// A class that implements <seealso cref="IFudgeObjectBuilder"/> for the annotated type.
        /// </summary>
        public Type objectBuilder
        {
            get { return _objectBuilder; }
            set { _objectBuilder = value; }
        }
        /// <summary>
        /// A class that implements <seealso cref="IFudgeMessageBuilder"/> for the annotated type.
        /// </summary>
        public Type messageBuilder
        {
            get { return _messageBuilder; }
            set { _messageBuilder = value; }
        }
    }
}