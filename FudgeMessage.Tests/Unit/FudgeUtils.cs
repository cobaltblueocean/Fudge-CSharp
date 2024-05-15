/**
 * <!--
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
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using FudgeMessage;
using Mercury.Test.Utility;

namespace FudgeMessage.Tests.Unit
{
    /// <summary>
    /// Utility methods to help testing
    /// </summary>
    public static class FudgeUtils
    {
        public static void AssertAllFieldsMatch(FudgeMsg expectedMsg, FudgeMsg actualMsg)
        {
            var expectedIter = expectedMsg.GetAllFields().GetEnumerator();
            var actualIter = actualMsg.GetAllFields().GetEnumerator();
            while (expectedIter.MoveNext())
            {
                Assert2.True(actualIter.MoveNext());
                IFudgeField expectedField = expectedIter.Current;
                IFudgeField actualField = actualIter.Current;

                Assert2.AreEqual(expectedField.Name, actualField.Name);
                Assert2.AreEqual(expectedField.Type, actualField.Type);
                Assert2.AreEqual(expectedField.Ordinal, actualField.Ordinal);
                if (expectedField.Value.GetType().IsArray)
                {
                    Assert2.AreEqual(expectedField.Value.GetType(), actualField.Value.GetType());
                    Assert2.AreEqual(expectedField.Value, actualField.Value);       // XUnit will check all values in the arrays
                }
                else if (expectedField.Value is FudgeMsg)
                {
                    Assert2.True(actualField.Value is FudgeMsg);
                    AssertAllFieldsMatch((FudgeMsg)expectedField.Value,
                        (FudgeMsg)actualField.Value);
                }
                else if (expectedField.Value is UnknownFudgeFieldValue)
                {
                    Assert2.IsType<UnknownFudgeFieldValue>(actualField.Value);
                    UnknownFudgeFieldValue expectedValue = (UnknownFudgeFieldValue)expectedField.Value;
                    UnknownFudgeFieldValue actualValue = (UnknownFudgeFieldValue)actualField.Value;
                    Assert2.AreEqual(expectedField.Type.TypeId, actualField.Type.TypeId);
                    Assert2.AreEqual(expectedValue.Type.TypeId, actualField.Type.TypeId);
                    Assert2.AreEqual(expectedValue.Contents, actualValue.Contents);
                }
                else
                {
                    Assert2.AreEqual(expectedField.Value, actualField.Value);
                }
            }
            Assert2.False(actualIter.MoveNext());
        }

        public static string ToNiceString(this byte[] bytes)
        {
            return string.Join("-", bytes.Select(b => b.ToString("x2")).ToArray());
        }
    }
}
