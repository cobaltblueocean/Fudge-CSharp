/**
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
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FudgeMessage;
using FudgeMessage.Types;
using System.Net;
using Mercury.Test.Utility;
using NUnit.Framework.Legacy;

namespace FudgeMessage.Tests.Unit
{
    [Parallelizable(ParallelScope.ContextMask)]
    public class FudgeMsgTest
    {
        private static readonly FudgeContext fudgeContext = new FudgeContext();

        [Test]
        public void LookupByNameSingleValue()
        {
            FudgeMsg msg = StandardFudgeMessages.CreateMessageAllNames(fudgeContext);
            IFudgeField field = null;
            IList<IFudgeField> fields = null;

            field = msg.GetByName("boolean");
            Assert2.NotNull(field);
            Assert2.AreEqual(PrimitiveFieldTypes.BooleanType, field.Type);
            Assert2.AreEqual(true, field.Value);
            Assert2.AreEqual("boolean", field.Name);
            Assert2.Null(field.Ordinal);

            field = msg.GetByName("Boolean");
            Assert2.NotNull(field);
            Assert2.AreEqual(PrimitiveFieldTypes.BooleanType, field.Type);
            Assert2.AreEqual((object)false, field.Value);
            Assert2.AreEqual("Boolean", field.Name);
            Assert2.Null(field.Ordinal);

            fields = msg.GetAllByName("boolean");
            Assert2.NotNull(fields);
            Assert2.AreEqual(1, fields.Count);
            field = fields[0];
            Assert2.NotNull(field);
            Assert2.AreEqual(PrimitiveFieldTypes.BooleanType, field.Type);
            Assert2.AreEqual(true, field.Value);
            Assert2.AreEqual("boolean", field.Name);
            Assert2.Null(field.Ordinal);

            // Check the indicator type specially
            Assert2.AreEqual(IndicatorType.Instance, msg.GetValue("indicator"));
        }

        [Test]
        public void LookupByNameMultipleValues()
        {
            FudgeMsg msg = StandardFudgeMessages.CreateMessageAllNames(fudgeContext);
            IFudgeField field = null;
            IList<IFudgeField> fields = null;

            // Now add a second by name.
            msg.Add("boolean", true);

            field = msg.GetByName("boolean");
            Assert2.NotNull(field);
            Assert2.AreEqual(PrimitiveFieldTypes.BooleanType, field.Type);
            Assert2.AreEqual(true, field.Value);
            Assert2.AreEqual("boolean", field.Name);
            Assert2.Null(field.Ordinal);

            fields = msg.GetAllByName("boolean");
            Assert2.NotNull(fields);
            Assert2.AreEqual(2, fields.Count);
            field = fields[0];
            Assert2.NotNull(field);
            Assert2.AreEqual(PrimitiveFieldTypes.BooleanType, field.Type);
            Assert2.AreEqual(true, field.Value);
            Assert2.AreEqual("boolean", field.Name);
            Assert2.Null(field.Ordinal);

            field = fields[1];
            Assert2.NotNull(field);
            Assert2.AreEqual(PrimitiveFieldTypes.BooleanType, field.Type);
            Assert2.AreEqual(true, field.Value);
            Assert2.AreEqual("boolean", field.Name);
            Assert2.Null(field.Ordinal);
        }

        [Test]
        public void PrimitiveExactQueriesNamesMatch()
        {
            FudgeMsg msg = StandardFudgeMessages.CreateMessageAllNames(fudgeContext);

            Assert2.AreEqual((sbyte)5, msg.GetSByte("byte"));
            Assert2.AreEqual((sbyte)5, msg.GetSByte("Byte"));

            short shortValue = ((short)sbyte.MaxValue) + 5;
            Assert2.AreEqual(shortValue, msg.GetShort("short"));
            Assert2.AreEqual(shortValue, msg.GetShort("Short"));

            int intValue = ((int)short.MaxValue) + 5;
            Assert2.AreEqual(intValue, msg.GetInt("int"));
            Assert2.AreEqual(intValue, msg.GetInt("Integer"));

            long longValue = ((long)int.MaxValue) + 5;
            Assert2.AreEqual(longValue, msg.GetLong("long"));
            Assert2.AreEqual(longValue, msg.GetLong("Long"));

            Assert2.AreEqual(0.5f, msg.GetFloat("float"));
            Assert2.AreEqual(0.5f, msg.GetFloat("Float"));
            Assert2.AreEqual(0.27362, msg.GetDouble("double"));
            Assert2.AreEqual(0.27362, msg.GetDouble("Double"));

            Assert2.AreEqual("Kirk Wylie", msg.GetString("String"));
        }

        [Test]
        public void PrimitiveExactQueriesNamesNoMatch()
        {
            FudgeMsg msg = StandardFudgeMessages.CreateMessageAllNames(fudgeContext);

            Assert2.ThrowsException<OverflowException>(() => msg.GetSByte("int"));
            Assert2.ThrowsException<OverflowException>(() => msg.GetShort("int"));
            Assert2.AreEqual(5, msg.GetInt("byte"));
            Assert2.AreEqual(((long)short.MaxValue) + 5, msg.GetLong("int"));
            Assert2.AreEqual(0.27362f, msg.GetFloat("double"));
            Assert2.AreEqual(0.5, msg.GetDouble("float"));
        }

        [Test]
        public void PrimitiveExactQueriesNoNames()
        {
            FudgeMsg msg = StandardFudgeMessages.CreateMessageAllNames(fudgeContext);

            Assert2.Null(msg.GetSByte("foobar"));
            Assert2.Null(msg.GetShort("foobar"));
            Assert2.Null(msg.GetInt("foobar"));
            Assert2.Null(msg.GetLong("foobar"));
            Assert2.Null(msg.GetFloat("foobar"));
            Assert2.Null(msg.GetDouble("foobar"));
            Assert2.Null(msg.GetString("foobar"));
        }

        [Test]
        public void AsQueriesToLongNames()
        {
            FudgeMsg msg = StandardFudgeMessages.CreateMessageAllNames(fudgeContext);

            Assert2.AreEqual((long?)((sbyte)5), msg.GetLong("byte"));
            Assert2.AreEqual((long?)((sbyte)5), msg.GetLong("Byte"));


            short shortValue = ((short)sbyte.MaxValue) + 5;
            Assert2.AreEqual((long?)(shortValue), msg.GetLong("short"));
            Assert2.AreEqual((long?)(shortValue), msg.GetLong("Short"));

            int intValue = ((int)short.MaxValue) + 5;
            Assert2.AreEqual((long?)(intValue), msg.GetLong("int"));
            Assert2.AreEqual((long?)(intValue), msg.GetLong("Integer"));

            long longValue = ((long)int.MaxValue) + 5;
            Assert2.AreEqual((long?)(longValue), msg.GetLong("long"));
            Assert2.AreEqual((long?)(longValue), msg.GetLong("Long"));

            Assert2.AreEqual((long?)(0), msg.GetLong("float"));
            Assert2.AreEqual((long?)(0), msg.GetLong("Float"));
            Assert2.AreEqual((long?)(0), msg.GetLong("double"));
            Assert2.AreEqual((long?)(0), msg.GetLong("Double"));
        }

        [Test]
        public void GetValueTyped()
        {
            FudgeMsg msg = StandardFudgeMessages.CreateMessageAllNames(fudgeContext);
            long longValue = ((long)int.MaxValue) + 5;
            Assert2.AreEqual(longValue, msg.GetValue<long>("long"));
            Assert2.AreEqual(5, msg.GetValue<long>("byte"));
        }

        // ------------

        [Test]
        public void PrimitiveExactQueriesOrdinalsMatch()
        {
            FudgeMsg msg = StandardFudgeMessages.CreateMessageAllOrdinals(fudgeContext);

            Assert2.AreEqual((sbyte)5, msg.GetSByte((short)3));
            Assert2.AreEqual((sbyte)5, msg.GetSByte((short)4));

            short shortValue = ((short)sbyte.MaxValue) + 5;
            Assert2.AreEqual(shortValue, msg.GetShort((short)5));
            Assert2.AreEqual(shortValue, msg.GetShort((short)6));

            int intValue = ((int)short.MaxValue) + 5;
            Assert2.AreEqual(intValue, msg.GetInt((short)7));
            Assert2.AreEqual(intValue, msg.GetInt((short)8));

            long longValue = ((long)int.MaxValue) + 5;
            Assert2.AreEqual(longValue, msg.GetLong((short)9));
            Assert2.AreEqual(longValue, msg.GetLong((short)10));

            Assert2.AreEqual(0.5f, msg.GetFloat((short)11));
            Assert2.AreEqual(0.5f, msg.GetFloat((short)12));
            Assert2.AreEqual(0.27362, msg.GetDouble((short)13));
            Assert2.AreEqual(0.27362, msg.GetDouble((short)14));

            Assert2.AreEqual("Kirk Wylie", msg.GetString((short)15));
        }

        [Test]
        public void PrimitiveExactQueriesOrdinalsNoMatch()
        {
            FudgeMsg msg = StandardFudgeMessages.CreateMessageAllOrdinals(fudgeContext);

            Assert2.ThrowsException<OverflowException>(() => msg.GetSByte(7));
            Assert2.ThrowsException<OverflowException>(() => msg.GetShort(7));
            Assert2.ThrowsException<OverflowException>(() => msg.GetInt(9));
            Assert2.AreEqual(((long)short.MaxValue) + 5, msg.GetLong(7));
            Assert2.AreEqual(0.27362f, msg.GetFloat(13));
            Assert2.AreEqual(0.5, msg.GetDouble(11));
        }

        [Test]
        public void PrimitiveExactOrdinalsNoOrdinals()
        {
            FudgeMsg msg = StandardFudgeMessages.CreateMessageAllOrdinals(fudgeContext);

            Assert2.Null(msg.GetSByte((short)100));
            Assert2.Null(msg.GetShort((short)100));
            Assert2.Null(msg.GetInt((short)100));
            Assert2.Null(msg.GetLong((short)100));
            Assert2.Null(msg.GetFloat((short)100));
            Assert2.Null(msg.GetDouble((short)100));
            Assert2.Null(msg.GetString((short)100));
        }

        [Test]
        public void AsQueriesToLongOrdinals()
        {
            FudgeMsg msg = StandardFudgeMessages.CreateMessageAllOrdinals(fudgeContext);

            Assert2.AreEqual((long)((sbyte)5), msg.GetLong((short)3));
            Assert2.AreEqual((long)((sbyte)5), msg.GetLong((short)4));

            short shortValue = ((short)sbyte.MaxValue) + 5;
            Assert2.AreEqual((long)(shortValue), msg.GetLong((short)5));
            Assert2.AreEqual((long)(shortValue), msg.GetLong((short)6));

            int intValue = ((int)short.MaxValue) + 5;
            Assert2.AreEqual((long)(intValue), msg.GetLong((short)7));
            Assert2.AreEqual((long)(intValue), msg.GetLong((short)8));

            long longValue = ((long)int.MaxValue) + 5;
            Assert2.AreEqual(longValue, msg.GetLong((short)9));
            Assert2.AreEqual(longValue, msg.GetLong((short)10));

            Assert2.AreEqual(0, msg.GetLong((short)11));
            Assert2.AreEqual(0, msg.GetLong((short)12));
            Assert2.AreEqual(0, msg.GetLong((short)13));
            Assert2.AreEqual(0, msg.GetLong((short)14));
        }

        [Test]
        public void ToByteArray()
        {
            FudgeMsg msg = StandardFudgeMessages.CreateMessageAllNames(fudgeContext);
            byte[] bytes = msg.ToByteArray();
            Assert2.NotNull(bytes);
            Assert2.True(bytes.Length > 10);
        }

        [Test]
        public void LongInLongOut()
        {
            FudgeMsg msg = new FudgeMsg();

            msg.Add("test", (long)5);
            Assert2.AreEqual((long)5, msg.GetLong("test"));
        }

        [Test]
        public void FixedLengthByteArrays()
        {
            FudgeMsg msg = StandardFudgeMessages.CreateMessageAllByteArrayLengths(fudgeContext);
            ClassicAssert.AreEqual(ByteArrayFieldType.Length4Instance, msg.GetByName("byte[4]").Type);
            Assert2.AreEqual(ByteArrayFieldType.Length8Instance, msg.GetByName("byte[8]").Type);
            Assert2.AreEqual(ByteArrayFieldType.Length16Instance, msg.GetByName("byte[16]").Type);
            Assert2.AreEqual(ByteArrayFieldType.Length20Instance, msg.GetByName("byte[20]").Type);
            Assert2.AreEqual(ByteArrayFieldType.Length32Instance, msg.GetByName("byte[32]").Type);
            Assert2.AreEqual(ByteArrayFieldType.Length64Instance, msg.GetByName("byte[64]").Type);
            Assert2.AreEqual(ByteArrayFieldType.Length128Instance, msg.GetByName("byte[128]").Type);
            Assert2.AreEqual(ByteArrayFieldType.Length256Instance, msg.GetByName("byte[256]").Type);
            Assert2.AreEqual(ByteArrayFieldType.Length512Instance, msg.GetByName("byte[512]").Type);

            Assert2.AreEqual(ByteArrayFieldType.VariableSizedInstance, msg.GetByName("byte[28]").Type);
        }

        [Test]
        public void Minimization()
        {
            FudgeMsg msg = new FudgeMsg();
            msg.Add("int?", 17);

            Assert2.AreEqual(PrimitiveFieldTypes.SByteType, msg.GetByName("int?").Type);
        }

        [Test]
        public void GuidsAsSecondaryTypes()
        {
            Guid guid = Guid.NewGuid();
            var msg = fudgeContext.NewMessage(); ;
            msg.Add("guid", guid);

            Assert2.AreEqual(ByteArrayFieldType.Length16Instance, msg.GetByName("guid").Type);

            Guid guid2 = msg.GetValue<Guid>("guid");
            Assert2.AreEqual(guid, guid2);
        }

        [Test]
        public void IPAddressesAsSecondaryTypes()
        {
            var ipv6Address = IPAddress.Parse("2001:db8:85a3::8a2e:370:7334");
            var ipv4Address = IPAddress.Parse("192.168.4.1");

            var msg = fudgeContext.NewMessage();
            msg.Add("ipv6", ipv6Address);
            msg.Add("ipv4", ipv4Address);

            Assert2.AreEqual(ByteArrayFieldType.Length16Instance, msg.GetByName("ipv6").Type);
            Assert2.AreEqual(ByteArrayFieldType.Length4Instance, msg.GetByName("ipv4").Type);

            var ipv6_2 = msg.GetValue<IPAddress>("ipv6");
            var ipv4_2 = msg.GetValue<IPAddress>("ipv4");

            Assert2.AreEqual(ipv6Address, ipv6_2);
            Assert2.IsTrue(ipv4Address.AreObjectsEqual(ipv4_2, new string[] { "ScopeId" }));  // According to .NET specification, ScopeId is not supported for IP4.  Ignore this roperty.
        }

        [Test]
        public void URIsAsSecondaryTypes_FRN75()
        {
            var uri = new Uri("http://www.fudgemsg.org/dashboard.action");

            var msg = fudgeContext.NewMessage();
            msg.Add("uri", uri);

            Assert2.AreEqual(StringFieldType.Instance, msg.GetByName("uri").Type);

            var uri2 = msg.GetValue<Uri>("uri");

            Assert2.AreEqual(uri, uri2);
        }

        [Test]
        public void Iterable()
        {
            FudgeMsg msg = StandardFudgeMessages.CreateMessageAllNames(fudgeContext);
            int fieldCount = 0;
            foreach (IFudgeField field in msg)
            {
                fieldCount++;
            }
            Assert2.AreEqual(msg.GetNumFields(), fieldCount);
        }

        [Test]
        public void IterableContainer()
        {
            IFudgeFieldContainer msg = StandardFudgeMessages.CreateMessageAllNames(fudgeContext);
            int fieldCount = 0;
            foreach (IFudgeField field in msg)
            {
                fieldCount++;
            }
            Assert2.AreEqual(msg.GetNumFields(), fieldCount);
        }

        [Test]
        public void AddingFieldContainerCopiesFields()
        {
            var msg = new FudgeMsg();

            // Add a normal sub-message (shouldn't copy)
            IFudgeFieldContainer sub1 = new FudgeMsg(new Field("age", 37));
            msg.Add("sub1", sub1);
            Assert2.AreEqual(sub1, msg.GetValue("sub1"));

            // Add a sub-message that isn't a FudgeMsg (should copy)
            IFudgeFieldContainer sub2 = (IFudgeFieldContainer)new Field("dummy", new Field("colour", "blue")).Value;
            Assert2.IsNotType<FudgeMsg>(sub2);       // Just making sure
            msg.Add("sub2", sub2);
            Assert2.AreNotSame(sub2, msg.GetValue("sub2"));
            Assert2.IsType<FudgeMsg>(msg.GetValue("sub2"));
            Assert2.AreEqual("blue", msg.GetMessage("sub2").GetString("colour"));
        }

        [Test]
        public void GetAllNames()
        {
            var msg = new FudgeMsg();
            msg.Add("foo", 3);
            msg.Add("bar", 17);
            msg.Add("foo", 2);      // Deliberately do a duplicate
            var names = msg.GetAllFieldNames();
            Assert2.AreEqual(2, names.Count);
            Assert2.Contains("foo", names.ToArray());
            Assert2.Contains("bar", names.ToArray());
        }

        [Test]
        public void GetMessageMethodsFRN5()
        {
            var msg = StandardFudgeMessages.CreateMessageWithSubMsgs(fudgeContext);
            Assert2.Null(msg.GetMessage(42));
            Assert2.Null(msg.GetMessage("No Such Field"));
            Assert2.True(msg.GetMessage("sub1") is IFudgeFieldContainer);
        }
    }
}
