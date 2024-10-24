﻿/**
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
using FudgeMessage;
using FudgeMessage.Linq;
using NUnit.Framework;
using System.Xml.Linq;
using Mercury.Test.Utility;

namespace FudgeMessage.Tests.Unit.Linq
{
    public class Examples
    {
        private class Tick
        {
            // Models the data we are going to process
            public double Bid { get; set; }
            public double Ask { get; set; }
            public string Ticker { get; set; }
        }

        [Test]
        public void SimpleSelect()
        {
            var msgs = new FudgeMsg[] { CreateTickMsg(10.3, 11.1, "FOO"), CreateTickMsg(2.4, 3.1, "BAR") };

            var query = from tick in msgs.AsQueryable<Tick>() select tick.Ticker;

            string[] tickers = query.ToArray();
            Assert2.AreEqual(new string[] {"FOO", "BAR"}, tickers);
        }

        [Test]
        public void FudgeMsgVsIFudgeFieldContainer()
        {
            // Make sure that we can work with enumerables of IFudgeFieldContainer, enumerables of IFudgeMsg and also arrays directly
            var array = new FudgeMsg[] { CreateTickMsg(10.3, 11.1, "FOO"), CreateTickMsg(2.4, 3.1, "BAR") };
            IEnumerable<IFudgeFieldContainer> containerList = array;
            IEnumerable<FudgeMsg> msgList = array;
            var query1 = from tick in containerList.AsQueryable<Tick>() select tick.Ticker;
            var query2 = from tick in msgList.AsQueryable<Tick>() select tick.Ticker;
            var query3 = from tick in array.AsQueryable<Tick>() select tick.Ticker;

            Assert2.AreEqual(query1, query2);
            Assert2.AreEqual(query1, query3);
        }

        [Test]
        public void SelectWithExpression()
        {
            var msgs = new FudgeMsg[] { CreateTickMsg(10.3, 11.1, "FOO"), CreateTickMsg(2.4, 3.1, "BAR") };

            var query = from tick in msgs.AsQueryable<Tick>()
                        select tick.Bid * 2;

            double[] vals = query.ToArray();
            Assert2.AreEqual(new double[] { 20.6, 4.8 }, vals);
        }

        [Test]
        public void WhereFilter()
        {
            var msgs = new FudgeMsg[] { CreateTickMsg(10.3, 11.1, "FOO"), CreateTickMsg(2.4, 3.1, "BAR") };

            var query = from tick in msgs.AsQueryable<Tick>()
                        where tick.Bid < 5.0
                        select tick.Ticker;

            string[] tickers = query.ToArray();
            Assert2.AreEqual(new string[] { "BAR" }, tickers);
        }

        [Test]
        public void ComplexSelect()
        {
            // This code is used in the Fudge.Linq namespace doc example.

            var msgs = new FudgeMsg[] { CreateTickMsg(10.3, 11.1, "FOO"), CreateTickMsg(2.4, 3.1, "BAR") };

            var query = from tick in msgs.AsQueryable<Tick>()
                        where tick.Ask > 4.0
                        select new { tick.Ticker, tick.Ask };

            var results = query.ToArray();
            Assert2.AreEqual(1, results.Length);
            Assert2.AreEqual("FOO", results[0].Ticker);
            Assert2.AreEqual(11.1, results[0].Ask);
        }

        [Test]
        public void OrderBy()
        {
            var msgs = new FudgeMsg[] { CreateTickMsg(10.3, 11.1, "FOO"), CreateTickMsg(2.4, 3.1, "BAR"), CreateTickMsg(5.2, 5.5, "ZIP") };

            var query = from tick in msgs.AsQueryable<Tick>()
                        orderby tick.Ask
                        select tick.Ticker;

            string[] tickers = query.ToArray();
            Assert2.AreEqual(new string[] { "BAR", "ZIP", "FOO" }, tickers);

            // And descending
            query = from tick in msgs.AsQueryable<Tick>()
                        orderby tick.Bid descending
                        select tick.Ticker;

            tickers = query.ToArray();
            Assert2.AreEqual(new string[] { "FOO", "ZIP", "BAR" }, tickers);
        }

        [Test]
        public void BindingParams()
        {
            // Make sure we can handle constants that are coming in from outside the query
            var msgs = new FudgeMsg[] { CreateTickMsg(10.3, 11.1, "FOO"), CreateTickMsg(2.4, 3.1, "BAR"), CreateTickMsg(5.2, 5.5, "ZIP") };
            double bid = 2.4;
            var query = from tick in msgs.AsQueryable<Tick>()
                        where tick.Bid == bid               // bid here comes from outside the query
                        select tick.Ticker;

            var tickers = query.ToArray();
            Assert2.AreEqual(new string[] { "BAR" }, tickers);
        }

        [Test]
        public void XmlExample()
        {
            var msgs = new FudgeMsg[] { CreateTickMsg(10.3, 11.1, "FOO"), CreateTickMsg(2.4, 3.1, "BAR") };

            XElement tree = new XElement("Ticks", from tick in msgs.AsQueryable<Tick>()
                                                  select new XElement("Tick",
                                                      new XElement("Ticker", tick.Ticker),
                                                      new XElement("Bid", tick.Bid),
                                                      new XElement("Ask", tick.Ask)));
            string s = tree.ToString();
            //<Ticks>
            //  <Tick>
            //    <Ticker>FOO</Ticker>
            //    <Bid>10.3</Bid>
            //    <Ask>11.1</Ask>
            //  </Tick>
            //  <Tick>
            //    <Ticker>BAR</Ticker>
            //    <Bid>2.4</Bid>
            //    <Ask>3.1</Ask>
            //  </Tick>
            //</Ticks>
        }

        private static FudgeMsg CreateTickMsg(double bid, double ask, string ticker)
        {
            FudgeMsg msg = new FudgeMsg(
                                new Field("Bid", bid),
                                new Field("Ask", ask),
                                new Field("Ticker", ticker));
            return msg;
        }
    }
}
