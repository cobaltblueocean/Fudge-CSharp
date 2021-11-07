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

namespace FudgeMessage.Tests.Unit
{
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
            Assert.NotNull(field);
            Assert.AreEqual(PrimitiveFieldTypes.BooleanType, field.Type);
            Assert.AreEqual(true, field.Value);
            Assert.AreEqual("boolean", field.Name);
            Assert.Null(field.Ordinal);

            field = msg.GetByName("Boolean");
            Assert.NotNull(field);
            Assert.AreEqual(PrimitiveFieldTypes.BooleanType, field.Type);
            Assert.AreEqual((object)false, field.Value);
            Assert.AreEqual("Boolean", field.Name);
            Assert.Null(field.Ordinal);

            fields = msg.GetAllByName("boolean");
            Assert.NotNull(fields);
            Assert.AreEqual(1, fields.Count);
            field = fields[0];
            Assert.NotNull(field);
            Assert.AreEqual(PrimitiveFieldTypes.BooleanType, field.Type);
            Assert.AreEqual(true, field.Value);
            Assert.AreEqual("boolean", field.Name);
            Assert.Null(field.Ordinal);

            // Check the indicator type specially
            Assert.AreEqual(IndicatorType.Instance, msg.GetValue("indicator"));
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
            Assert.NotNull(field);
            Assert.AreEqual(PrimitiveFieldTypes.BooleanType, field.Type);
            Assert.AreEqual(true, field.Value);
            Assert.AreEqual("boolean", field.Name);
            Assert.Null(field.Ordinal);

            fields = msg.GetAllByName("boolean");
            Assert.NotNull(fields);
            Assert.AreEqual(2, fields.Count);
            field = fields[0];
            Assert.NotNull(field);
            Assert.AreEqual(PrimitiveFieldTypes.BooleanType, field.Type);
            Assert.AreEqual(true, field.Value);
            Assert.AreEqual("boolean", field.Name);
            Assert.Null(field.Ordinal);

            field = fields[1];
            Assert.NotNull(field);
            Assert.AreEqual(PrimitiveFieldTypes.BooleanType, field.Type);
            Assert.AreEqual(true, field.Value);
            Assert.AreEqual("boolean", field.Name);
            Assert.Null(field.Ordinal);
        }

        [Test]
        public void PrimitiveExactQueriesNamesMatch()
        {
            FudgeMsg msg = StandardFudgeMessages.CreateMessageAllNames(fudgeContext);

            Assert.AreEqual((sbyte)5, msg.GetSByte("byte"));
            Assert.AreEqual((sbyte)5, msg.GetSByte("Byte"));

            short shortValue = ((short)sbyte.MaxValue) + 5;
            Assert.AreEqual(shortValue, msg.GetShort("short"));
            Assert.AreEqual(shortValue, msg.GetShort("Short"));

            int intValue = ((int)short.MaxValue) + 5;
            Assert.AreEqual(intValue, msg.GetInt("int"));
            Assert.AreEqual(intValue, msg.GetInt("Integer"));

            long longValue = ((long)int.MaxValue) + 5;
            Assert.AreEqual(longValue, msg.GetLong("long"));
            Assert.AreEqual(longValue, msg.GetLong("Long"));

            Assert.AreEqual(0.5f, msg.GetFloat("float"));
            Assert.AreEqual(0.5f, msg.GetFloat("Float"));
            Assert.AreEqual(0.27362, msg.GetDouble("double"));
            Assert.AreEqual(0.27362, msg.GetDouble("Double"));

            Assert.AreEqual("Kirk Wylie", msg.GetString("String"));
        }

        [Test]
        public void PrimitiveExactQueriesNamesNoMatch()
        {
            FudgeMsg msg = StandardFudgeMessages.CreateMessageAllNames(fudgeContext);

            Assert.Throws<OverflowException>(() => msg.GetSByte("int"));
            Assert.Throws<OverflowException>(() => msg.GetShort("int"));
            Assert.AreEqual(5, msg.GetInt("byte"));
            Assert.AreEqual(((long)short.MaxValue) + 5, msg.GetLong("int"));
            Assert.AreEqual(0.27362f, msg.GetFloat("double"));
            Assert.AreEqual(0.5, msg.GetDouble("float"));
        }

        [Test]
        public void PrimitiveExactQueriesNoNames()
        {
            FudgeMsg msg = StandardFudgeMessages.CreateMessageAllNames(fudgeContext);

            Assert.Null(msg.GetSByte("foobar"));
            Assert.Null(msg.GetShort("foobar"));
            Assert.Null(msg.GetInt("foobar"));
            Assert.Null(msg.GetLong("foobar"));
            Assert.Null(msg.GetFloat("foobar"));
            Assert.Null(msg.GetDouble("foobar"));
            Assert.Null(msg.GetString("foobar"));
        }

        [Test]
        public void AsQueriesToLongNames()
        {
            FudgeMsg msg = StandardFudgeMessages.CreateMessageAllNames(fudgeContext);

            Assert.AreEqual((long?)((sbyte)5), msg.GetLong("byte"));
            Assert.AreEqual((long?)((sbyte)5), msg.GetLong("Byte"));


            short shortValue = ((short)sbyte.MaxValue) + 5;
            Assert.AreEqual((long?)(shortValue), msg.GetLong("short"));
            Assert.AreEqual((long?)(shortValue), msg.GetLong("Short"));

            int intValue = ((int)short.MaxValue) + 5;
            Assert.AreEqual((long?)(intValue), msg.GetLong("int"));
            Assert.AreEqual((long?)(intValue), msg.GetLong("Integer"));

            long longValue = ((long)int.MaxValue) + 5;
            Assert.AreEqual((long?)(longValue), msg.GetLong("long"));
            Assert.AreEqual((long?)(longValue), msg.GetLong("Long"));

            Assert.AreEqual((long?)(0), msg.GetLong("float"));
            Assert.AreEqual((long?)(0), msg.GetLong("Float"));
            Assert.AreEqual((long?)(0), msg.GetLong("double"));
            Assert.AreEqual((long?)(0), msg.GetLong("Double"));
        }

        [Test]
        public void GetValueTyped()
        {
            FudgeMsg msg = StandardFudgeMessages.CreateMessageAllNames(fudgeContext);
            long longValue = ((long)int.MaxValue) + 5;
            Assert.AreEqual(longValue, msg.GetValue<long>("long"));
            Assert.AreEqual(5, msg.GetValue<long>("byte"));
        }

        // ------------

        [Test]
        public void PrimitiveExactQueriesOrdinalsMatch()
        {
            FudgeMsg msg = StandardFudgeMessages.CreateMessageAllOrdinals(fudgeContext);

            Assert.AreEqual((sbyte)5, msg.GetSByte((short)3));
            Assert.AreEqual((sbyte)5, msg.GetSByte((short)4));

            short shortValue = ((short)sbyte.MaxValue) + 5;
            Assert.AreEqual(shortValue, msg.GetShort((short)5));
            Assert.AreEqual(shortValue, msg.GetShort((short)6));

            int intValue = ((int)short.MaxValue) + 5;
            Assert.AreEqual(intValue, msg.GetInt((short)7));
            Assert.AreEqual(intValue, msg.GetInt((short)8));

            long longValue = ((long)int.MaxValue) + 5;
            Assert.AreEqual(longValue, msg.GetLong((short)9));
            Assert.AreEqual(longValue, msg.GetLong((short)10));

            Assert.AreEqual(0.5f, msg.GetFloat((short)11));
            Assert.AreEqual(0.5f, msg.GetFloat((short)12));
            Assert.AreEqual(0.27362, msg.GetDouble((short)13));
            Assert.AreEqual(0.27362, msg.GetDouble((short)14));

            Assert.AreEqual("Kirk Wylie", msg.GetString((short)15));
        }

        [Test]
        public void PrimitiveExactQueriesOrdinalsNoMatch()
        {
            FudgeMsg msg = StandardFudgeMessages.CreateMessageAllOrdinals(fudgeContext);

            Assert.Throws<OverflowException>(() => msg.GetSByte(7));
            Assert.Throws<OverflowException>(() => msg.GetShort(7));
            Assert.Throws<OverflowException>(() => msg.GetInt(9));
            Assert.AreEqual(((long)short.MaxValue) + 5, msg.GetLong(7));
            Assert.AreEqual(0.27362f, msg.GetFloat(13));
            Assert.AreEqual(0.5, msg.GetDouble(11));
        }

        [Test]
        public void PrimitiveExactOrdinalsNoOrdinals()
        {
            FudgeMsg msg = StandardFudgeMessages.CreateMessageAllOrdinals(fudgeContext);

            Assert.Null(msg.GetSByte((short)100));
            Assert.Null(msg.GetShort((short)100));
            Assert.Null(msg.GetInt((short)100));
            Assert.Null(msg.GetLong((short)100));
            Assert.Null(msg.GetFloat((short)100));
            Assert.Null(msg.GetDouble((short)100));
            Assert.Null(msg.GetString((short)100));
        }

        [Test]
        public void AsQueriesToLongOrdinals()
        {
            FudgeMsg msg = StandardFudgeMessages.CreateMessageAllOrdinals(fudgeContext);

            Assert.AreEqual((long)((sbyte)5), msg.GetLong((short)3));
            Assert.AreEqual((long)((sbyte)5), msg.GetLong((short)4));

            short shortValue = ((short)sbyte.MaxValue) + 5;
            Assert.AreEqual((long)(shortValue), msg.GetLong((short)5));
            Assert.AreEqual((long)(shortValue), msg.GetLong((short)6));

            int intValue = ((int)short.MaxValue) + 5;
            Assert.AreEqual((long)(intValue), msg.GetLong((short)7));
            Assert.AreEqual((long)(intValue), msg.GetLong((short)8));

            long longValue = ((long)int.MaxValue) + 5;
            Assert.AreEqual(longValue, msg.GetLong((short)9));
            Assert.AreEqual(longValue, msg.GetLong((short)10));

            Assert.AreEqual(0, msg.GetLong((short)11));
            Assert.AreEqual(0, msg.GetLong((short)12));
            Assert.AreEqual(0, msg.GetLong((short)13));
            Assert.AreEqual(0, msg.GetLong((short)14));
        }

        [Test]
        public void ToByteArray()
        {
            FudgeMsg msg = StandardFudgeMessages.CreateMessageAllNames(fudgeContext);
            byte[] bytes = msg.ToByteArray();
            Assert.NotNull(bytes);
            Assert.True(bytes.Length > 10);
        }

        [Test]
        public void LongInLongOut()
        {
            FudgeMsg msg = new FudgeMsg();

            msg.Add("test", (long)5);
            Assert.AreEqual((long)5, msg.GetLong("test"));
        }

        [Test]
        public void FixedLengthByteArrays()
        {
            FudgeMsg msg = StandardFudgeMessages.CreateMessageAllByteArrayLengths(fudgeContext);
            Assert.AreEqual(ByteArrayFieldType.Length4Instance, msg.GetByName("byte[4]").Type);
            Assert.AreEqual(ByteArrayFieldType.Length8Instance, msg.GetByName("byte[8]").Type);
            Assert.AreEqual(ByteArrayFieldType.Length16Instance, msg.GetByName("byte[16]").Type);
            Assert.AreEqual(ByteArrayFieldType.Length20Instance, msg.GetByName("byte[20]").Type);
            Assert.AreEqual(ByteArrayFieldType.Length32Instance, msg.GetByName("byte[32]").Type);
            Assert.AreEqual(ByteArrayFieldType.Length64Instance, msg.GetByName("byte[64]").Type);
            Assert.AreEqual(ByteArrayFieldType.Length128Instance, msg.GetByName("byte[128]").Type);
            Assert.AreEqual(ByteArrayFieldType.Length256Instance, msg.GetByName("byte[256]").Type);
            Assert.AreEqual(ByteArrayFieldType.Length512Instance, msg.GetByName("byte[512]").Type);

            Assert.AreEqual(ByteArrayFieldType.VariableSizedInstance, msg.GetByName("byte[28]").Type);
        }

        [Test]
        public void Minimization()
        {
            FudgeMsg msg = new FudgeMsg();
            msg.Add("int?", 17);

            Assert.AreEqual(PrimitiveFieldTypes.SByteType, msg.GetByName("int?").Type);
        }

        [Test]
        public void GuidsAsSecondaryTypes()
        {
            Guid guid = Guid.NewGuid();
            var msg = fudgeContext.NewMessage(); ;
            msg.Add("guid", guid);

            Assert.AreEqual(ByteArrayFieldType.Length16Instance, msg.GetByName("guid").Type);

            Guid guid2 = msg.GetValue<Guid>("guid");
            Assert.AreEqual(guid, guid2);
        }

        [Test]
        public void IPAddressesAsSecondaryTypes()
        {
            var ipv6Address = IPAddress.Parse("2001:db8:85a3::8a2e:370:7334");
            var ipv4Address = IPAddress.Parse("192.168.4.1");

            var msg = fudgeContext.NewMessage();
            msg.Add("ipv6", ipv6Address);
            msg.Add("ipv4", ipv4Address);

            Assert.AreEqual(ByteArrayFieldType.Length16Instance, msg.GetByName("ipv6").Type);
            Assert.AreEqual(ByteArrayFieldType.Length4Instance, msg.GetByName("ipv4").Type);

            var ipv6_2 = msg.GetValue<IPAddress>("ipv6");
            var ipv4_2 = msg.GetValue<IPAddress>("ipv4");

            Assert.AreEqual(ipv6Address, ipv6_2);
            Assert.AreEqual(ipv4Address, ipv4_2);
        }

        [Test]
        public void URIsAsSecondaryTypes_FRN75()
        {
            var uri = new Uri("http://www.fudgemsg.org/dashboard.action");

            var msg = fudgeContext.NewMessage();
            msg.Add("uri", uri);

            Assert.AreEqual(StringFieldType.Instance, msg.GetByName("uri").Type);

            var uri2 = msg.GetValue<Uri>("uri");

            Assert.AreEqual(uri, uri2);
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
            Assert.AreEqual(msg.GetNumFields(), fieldCount);
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
            Assert.AreEqual(msg.GetNumFields(), fieldCount);
        }

        [Test]
        public void AddingFieldContainerCopiesFields()
        {
            var msg = new FudgeMsg();

            // Add a normal sub-message (shouldn't copy)
            IFudgeFieldContainer sub1 = new FudgeMsg(new Field("age", 37));
            msg.Add("sub1", sub1);
            Assert.AreEqual(sub1, msg.GetValue("sub1"));

            // Add a sub-message that isn't a FudgeMsg (should copy)
            IFudgeFieldContainer sub2 = (IFudgeFieldContainer)new Field("dummy", new Field("colour", "blue")).Value;
            Assert2.IsNotType<FudgeMsg>(sub2);       // Just making sure
            msg.Add("sub2", sub2);
            Assert.AreNotSame(sub2, msg.GetValue("sub2"));
            Assert2.IsType<FudgeMsg>(msg.GetValue("sub2"));
            Assert.AreEqual("blue", msg.GetMessage("sub2").GetString("colour"));
        }

        [Test]
        public void GetAllNames()
        {
            var msg = new FudgeMsg();
            msg.Add("foo", 3);
            msg.Add("bar", 17);
            msg.Add("foo", 2);      // Deliberately do a duplicate
            var names = msg.GetAllFieldNames();
            Assert.AreEqual(2, names.Count);
            Assert.Contains("foo", names.ToArray());
            Assert.Contains("bar", names.ToArray());
        }

        [Test]
        public void GetMessageMethodsFRN5()
        {
            var msg = StandardFudgeMessages.CreateMessageWithSubMsgs(fudgeContext);
            Assert.Null(msg.GetMessage(42));
            Assert.Null(msg.GetMessage("No Such Field"));
            Assert.True(msg.GetMessage("sub1") is IFudgeFieldContainer);
        }
    }
}
