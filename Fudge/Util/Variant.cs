using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fudge;
using Fudge.Types;

namespace Fudge.Util
{
    /// <summary>
    /// Enable to a Variant type return value for method.
    /// </summary>
    public class Variant
    {
        //Boolean, sbyte[], sbyte, Date[], Date, Double, Double[], float, float[], int, int[], long, long[], short, short[], String, Time, Object, FudgeMsg

        #region "Local variables for each Type"
        private readonly Func<Boolean> _func1;
        private readonly Func<sbyte> _func2;
        private readonly Func<sbyte[]> _func3;
        private readonly Func<DateTime> _func4;
        private readonly Func<DateTime[]> _func5;
        private readonly Func<Double> _func6;
        private readonly Func<Double[]> _func7;
        private readonly Func<float> _func8;
        private readonly Func<float[]> _func9;
        private readonly Func<int> _func10;
        private readonly Func<int[]> _func11;
        private readonly Func<long> _func12;
        private readonly Func<long[]> _func13;
        private readonly Func<short> _func14;
        private readonly Func<short[]> _func15;
        private readonly Func<String> _func16;
        private readonly Func<FudgeTime> _func17;
        private readonly Func<Object> _func18;
        private readonly Func<FudgeMsg> _func19;

        private Boolean _mValue1;
        private sbyte _mValue2;
        private sbyte[] _mValue3;
        private DateTime _mValue4;
        private DateTime[] _mValue5;
        private Double _mValue6;
        private Double[] _mValue7;
        private float _mValue8;
        private float[] _mValue9;
        private int _mValue10;
        private int[] _mValue11;
        private long _mValue12;
        private long[] _mValue13;
        private short _mValue14;
        private short[] _mValue15;
        private String _mValue16;
        private FudgeTime _mValue17;
        private Object _mValue18;
        private FudgeMsg _mValue19;

        private Boolean _m1 = false;
        private Boolean _m2 = false;
        private Boolean _m3 = false;
        private Boolean _m4 = false;
        private Boolean _m5 = false;
        private Boolean _m6 = false;
        private Boolean _m7 = false;
        private Boolean _m8 = false;
        private Boolean _m9 = false;
        private Boolean _m10 = false;
        private Boolean _m11 = false;
        private Boolean _m12 = false;
        private Boolean _m13 = false;
        private Boolean _m14 = false;
        private Boolean _m15 = false;
        private Boolean _m16 = false;
        private Boolean _m17 = false;
        private Boolean _m18 = false;
        private Boolean _m19 = false;
        #endregion

        public Variant(Func<Boolean> func1, Func<sbyte> func2, Func<sbyte[]> func3, Func<DateTime> func4, Func<DateTime[]> func5, Func<Double> func6, Func<Double[]> func7, Func<float> func8, Func<float[]> func9, Func<int> func10, Func<int[]> func11, Func<long> func12, Func<long[]> func13, Func<short> func14, Func<short[]> func15, Func<String> func16, Func<FudgeTime> func17, Func<Object> func18, Func<FudgeMsg> func19)
        {
            this._func1 = func1;
            this._func2 = func2;
            this._func3 = func3;
            this._func4 = func4;
            this._func5 = func5;
            this._func6 = func6;
            this._func7 = func7;
            this._func8 = func8;
            this._func9 = func9;
            this._func10 = func10;
            this._func11 = func11;
            this._func12 = func12;
            this._func13 = func13;
            this._func14 = func14;
            this._func15 = func15;
            this._func16 = func16;
            this._func17 = func17;
            this._func18 = func18;
            this._func19 = func19;
        }

        public static implicit operator Boolean(Variant variant)
        {
            //TODO: use Interlocked API
            if (!variant._m1)
            {
                variant._mValue1 = variant._func1();
                variant._m1 = true;
            }
            return variant._mValue1;
        }

        public static implicit operator sbyte(Variant variant)
        {
            //TODO: use Interlocked API
            if (!variant._m2)
            {
                variant._mValue2 = variant._func2();
                variant._m2 = true;
            }
            return variant._mValue2;
        }

        public static implicit operator sbyte[](Variant variant)
        {
            //TODO: use Interlocked API
            if (!variant._m3)
            {
                variant._mValue3 = variant._func3();
                variant._m3 = true;
            }
            return variant._mValue3;
        }

        public static implicit operator DateTime(Variant variant)
        {
            //TODO: use Interlocked API
            if (!variant._m4)
            {
                variant._mValue4 = variant._func4();
                variant._m4 = true;
            }
            return variant._mValue4;
        }

        public static implicit operator DateTime[](Variant variant)
        {
            //TODO: use Interlocked API
            if (!variant._m5)
            {
                variant._mValue5 = variant._func5();
                variant._m5 = true;
            }
            return variant._mValue5;
        }

        public static implicit operator Double(Variant variant)
        {
            //TODO: use Interlocked API
            if (!variant._m6)
            {
                variant._mValue6 = variant._func6();
                variant._m6 = true;
            }
            return variant._mValue6;
        }

        public static implicit operator Double[](Variant variant)
        {
            //TODO: use Interlocked API
            if (!variant._m7)
            {
                variant._mValue7 = variant._func7();
                variant._m7 = true;
            }
            return variant._mValue7;
        }

        public static implicit operator float(Variant variant)
        {
            //TODO: use Interlocked API
            if (!variant._m8)
            {
                variant._mValue8 = variant._func8();
                variant._m8 = true;
            }
            return variant._mValue8;
        }

        public static implicit operator float[](Variant variant)
        {
            //TODO: use Interlocked API
            if (!variant._m9)
            {
                variant._mValue9 = variant._func9();
                variant._m9 = true;
            }
            return variant._mValue9;
        }

        public static implicit operator int(Variant variant)
        {
            //TODO: use Interlocked API
            if (!variant._m10)
            {
                variant._mValue10 = variant._func10();
                variant._m10 = true;
            }
            return variant._mValue10;
        }

        public static implicit operator int[](Variant variant)
        {
            //TODO: use Interlocked API
            if (!variant._m11)
            {
                variant._mValue11 = variant._func11();
                variant._m11 = true;
            }
            return variant._mValue11;
        }

        public static implicit operator long(Variant variant)
        {
            //TODO: use Interlocked API
            if (!variant._m12)
            {
                variant._mValue12 = variant._func12();
                variant._m12 = true;
            }
            return variant._mValue12;
        }

        public static implicit operator long[](Variant variant)
        {
            //TODO: use Interlocked API
            if (!variant._m13)
            {
                variant._mValue13 = variant._func13();
                variant._m13 = true;
            }
            return variant._mValue13;
        }

        public static implicit operator short(Variant variant)
        {
            //TODO: use Interlocked API
            if (!variant._m14)
            {
                variant._mValue14 = variant._func14();
                variant._m14 = true;
            }
            return variant._mValue14;
        }

        public static implicit operator short[](Variant variant)
        {
            //TODO: use Interlocked API
            if (!variant._m15)
            {
                variant._mValue15 = variant._func15();
                variant._m15 = true;
            }
            return variant._mValue15;
        }

        public static implicit operator String(Variant variant)
        {
            //TODO: use Interlocked API
            if (!variant._m16)
            {
                variant._mValue16 = variant._func16();
                variant._m16 = true;
            }
            return variant._mValue16;
        }

        public static implicit operator FudgeTime(Variant variant)
        {
            //TODO: use Interlocked API
            if (!variant._m17)
            {
                variant._mValue17 = variant._func17();
                variant._m17 = true;
            }
            return variant._mValue17;
        }

        public static implicit operator Object(Variant variant)
        {
            //TODO: use Interlocked API
            if (!variant._m18)
            {
                variant._mValue18 = variant._func18();
                variant._m18 = true;
            }
            return variant._mValue18;
        }

        public static implicit operator FudgeMsg(Variant variant)
        {
            //TODO: use Interlocked API
            if (!variant._m19)
            {
                variant._mValue19 = variant._func19();
                variant._m19 = true;
            }
            return variant._mValue19;
        }
    }
}
