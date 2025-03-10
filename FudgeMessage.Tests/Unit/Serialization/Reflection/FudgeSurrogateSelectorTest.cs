﻿/* <!--
 * Copyright (C) 2009 - 2010 by OpenGamma Inc. and other contributors.
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
using NUnit.Framework;
using FudgeMessage;
using FudgeMessage.Serialization;
using FudgeMessage.Serialization.Reflection;
using Mercury.Test.Utility;

namespace FudgeMessage.Tests.Unit.Serialization.Reflection
{
    [Parallelizable(ParallelScope.ContextMask)]
    public class FudgeSurrogateSelectorTest
    {
        private readonly FudgeContext context = new FudgeContext();

        [Test]
        public void DirectSerializaton()
        {
            // Basic case
            var selector = new FudgeSurrogateSelector(context);
            var surrogate = selector.GetSurrogate(typeof(DirectTest), FudgeFieldNameConvention.Identity);
            Assert2.IsType<SerializableSurrogate>(surrogate);

            // Test exception thrown if no default constructor
            Assert2.ThrowsException<FudgeRuntimeException>(() => selector.GetSurrogate(typeof(DirectNoDefaultConstructorTest), FudgeFieldNameConvention.Identity));
        }

        [Test]
        public void SurrogateAttribute()
        {
            var selector = new FudgeSurrogateSelector(context);

            var surrogate = selector.GetSurrogate(typeof(SurrogateTest), FudgeFieldNameConvention.Identity);
            Assert2.IsType<SurrogateTest.SurrogateTestSurrogate>(surrogate);

            // SurrogateTest3 has a constructor on the surrogate which takes type
            surrogate = selector.GetSurrogate(typeof(SurrogateTest3), FudgeFieldNameConvention.Identity);
            Assert2.IsType<SurrogateTest3.SurrogateTest3Surrogate>(surrogate);
            Assert2.AreEqual(typeof(SurrogateTest3), ((SurrogateTest3.SurrogateTest3Surrogate)surrogate).Type);

            // SurrogateTest4 has a constructor on the surrogate which takes context and type
            surrogate = selector.GetSurrogate(typeof(SurrogateTest4), FudgeFieldNameConvention.Identity);
            Assert2.IsType<SurrogateTest4.SurrogateTest4Surrogate>(surrogate);
            Assert2.AreEqual(typeof(SurrogateTest4), ((SurrogateTest4.SurrogateTest4Surrogate)surrogate).Type);
            Assert2.AreEqual(context, ((SurrogateTest4.SurrogateTest4Surrogate)surrogate).Context);
            
            //ISurrogateTest is an interface

            surrogate = selector.GetSurrogate(typeof(ISurrogateTest), FudgeFieldNameConvention.Identity);
            Assert2.IsType<InterfaceSurrogateTestSurrogate>(surrogate);
            Assert2.AreEqual(typeof(ISurrogateTest), ((InterfaceSurrogateTestSurrogate)surrogate).Type);
            Assert2.AreEqual(context, ((InterfaceSurrogateTestSurrogate)surrogate).Context);
        }

        #region Test classes

        private class DirectTest : IFudgeSerializable
        {
            #region IFudgeSerializable Members

            public void Serialize(IAppendingFudgeFieldContainer msg, IFudgeSerializer serializer)
            {
                throw new NotImplementedException();
            }

            public void Deserialize(IFudgeFieldContainer msg, IFudgeDeserializer deserializer)
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        private class DirectNoDefaultConstructorTest : IFudgeSerializable
        {
            public DirectNoDefaultConstructorTest(int i)
            {
            }

            #region IFudgeSerializable Members

            public void Serialize(IAppendingFudgeFieldContainer msg, IFudgeSerializer serializer)
            {
                throw new NotImplementedException();
            }

            public void Deserialize(IFudgeFieldContainer msg, IFudgeDeserializer deserializer)
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        [FudgeSurrogate(typeof(SurrogateTestSurrogate))]
        private class SurrogateTest
        {
            public SurrogateTest(int n)
            {
                Number = n;
            }

            public int Number { get; private set; }

            public class SurrogateTestSurrogate : IFudgeSerializationSurrogate
            {
                #region IFudgeSerializationSurrogate Members

                public void Serialize(object obj, IAppendingFudgeFieldContainer msg, IFudgeSerializer serializer)
                {
                    throw new NotImplementedException();
                }

                public object Deserialize(IFudgeFieldContainer msg, IFudgeDeserializer deserializer)
                {
                    throw new NotImplementedException();
                }

                #endregion
            }
        }

        [FudgeSurrogate(typeof(SurrogateTest2Surrogate))]
        private class SurrogateTest2
        {
            public class SurrogateTest2Surrogate : IFudgeSerializationSurrogate
            {
                #region IFudgeSerializationSurrogate Members

                public void Serialize(object obj, IAppendingFudgeFieldContainer msg, IFudgeSerializer serializer)
                {
                    throw new NotImplementedException();
                }

                public object Deserialize(IFudgeFieldContainer msg, IFudgeDeserializer deserializer)
                {
                    throw new NotImplementedException();
                }

                #endregion
            }
        }

        [FudgeSurrogate(typeof(SurrogateTest3Surrogate))]
        private class SurrogateTest3
        {
            public class SurrogateTest3Surrogate : IFudgeSerializationSurrogate
            {
                public Type Type { get; set; }

                public SurrogateTest3Surrogate(Type type)
                {
                    this.Type = type;
                }

                #region IFudgeSerializationSurrogate Members

                public void Serialize(object obj, IAppendingFudgeFieldContainer msg, IFudgeSerializer serializer)
                {
                    throw new NotImplementedException();
                }

                public object Deserialize(IFudgeFieldContainer msg, IFudgeDeserializer deserializer)
                {
                    throw new NotImplementedException();
                }

                #endregion
            }
        }

        [FudgeSurrogate(typeof(SurrogateTest4Surrogate))]
        private class SurrogateTest4
        {
            public class SurrogateTest4Surrogate : IFudgeSerializationSurrogate
            {
                public Type Type { get; set; }
                public FudgeContext Context { get; set; }

                public SurrogateTest4Surrogate(FudgeContext context, Type type)
                {
                    this.Context = context;
                    this.Type = type;
                }

                #region IFudgeSerializationSurrogate Members

                public void Serialize(object obj, IAppendingFudgeFieldContainer msg, IFudgeSerializer serializer)
                {
                    throw new NotImplementedException();
                }

                public object Deserialize(IFudgeFieldContainer msg, IFudgeDeserializer deserializer)
                {
                    throw new NotImplementedException();
                }

                #endregion
            }
        }

        [FudgeSurrogate(typeof(InterfaceSurrogateTestSurrogate))]
        private interface ISurrogateTest
        {
           int Number { get;}
        }

        private class InterfaceSurrogateTestSurrogate : IFudgeSerializationSurrogate
        {
            public Type Type { get; set; }
            public FudgeContext Context { get; set; }

            public InterfaceSurrogateTestSurrogate(FudgeContext context, Type type)
            {
                this.Context = context;
                this.Type = type;
            }

            #region IFudgeSerializationSurrogate Members

            public void Serialize(object obj, IAppendingFudgeFieldContainer msg, IFudgeSerializer serializer)
            {
                throw new NotImplementedException();
            }

            public object Deserialize(IFudgeFieldContainer msg, IFudgeDeserializer deserializer)
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        #endregion
    }
}
