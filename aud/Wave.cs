using System;

namespace aud
{

    // -------------------------------------------------------------------------- good

    //public class WaveReader : WaveProvider32
    //{
    //    private int Sample = 0;
    //    private Wave MainObject;

    //    /// <exception cref="ArgumentOutOfRangeException">value less than 0</exception>
    //    public int Position
    //    {
    //        get
    //        {
    //            return Sample;
    //        }
    //        set
    //        {
    //            if (value < 0)
    //                throw new ArgumentOutOfRangeException("value");
    //            Sample = value;
    //        }
    //    }

    //    public bool Finished
    //    {
    //        get
    //        {
    //            return Sample >= MainObject.Length;
    //        }
    //    }

    //    public void GoToStart()
    //    {
    //        Sample = 0;
    //    }

    //    public WaveReader(Wave Obj) : base(System.Convert.ToInt32(Obj.SampleRate), 1)
    //    {
    //        MainObject = Obj;
    //    }

    //    public override int Read(float[] buffer, int offset, int sampleCount)
    //    {
    //        for (var n = 0; n <= sampleCount - 1; n++)
    //        {
    //            if (Sample < MainObject.Length)
    //            {
    //                buffer[n + offset] = MainObject[Sample];
    //                Sample += 1;
    //            }
    //            else
    //                buffer[n + offset] = 0;
    //        }
    //        return sampleCount;
    //    }
    //}

    // -------------------------------------------------------------------------- very good
    //Thread safety on below...

    //is there data loss converting to float??
    public abstract class Wave
    {
        public abstract int Start { get; }

        public double StartSeconds { get { return Start / (double)SampleRate; } }

        public double StartMills { get { return StartSeconds * 1000; } }

        public float[] extract(int Start, int Count)
        {
            var tr = new float[Count];
            for (int i = 0; i < Count; i++)
            {
                tr[i] = this[Start + i];
            }
            return tr;
        }

        public abstract int Count { get; }

        public double CountSeconds { get { return Count / (double)SampleRate; } }

        public double CountMills { get { return CountSeconds * 1000; } }

        public int End => Start + Count;

        public double EndSeconds { get { return End / (double)SampleRate; } }

        public double EndMills { get { return EndSeconds * 1000; } }

        abstract public float this[int index] { get; }

        public uint SampleRate { get; }

        protected Wave(uint SampleRate)
        {
            this.SampleRate = SampleRate;
        }
    }

    //trusts Extend is not given more than count, and Count (on overload) is valid
    //returns 0 if accessed out of bounds
    //Start initialise to -Count
    public sealed class WaveRolling : Wave
    {
        public override float this[int index]
        {
            get
            {
                lock (iBuffer)
                {
                    index -= Start;
                    if (index < 0 || index >= Count)
                    { return 0; }
                    else if (index + StartOffset < Count)
                    { return iBuffer[index + StartOffset]; }
                    else
                    { return iBuffer[index + StartOffset - Count]; }
                }
            }
        }

        private int iPosition;
        public override int Start { get => iPosition; }

        public override int Count => iBuffer.Length;

        public void Extend(float[] extension)
        {
            Extend(extension, extension.Length);
        }

        public void Extend(float[] extension, int Count)
        {
            lock (iBuffer)
            {
                if (Count + StartOffset >= this.Count)
                {
                    Array.Copy(extension, 0, iBuffer, StartOffset, this.Count - StartOffset);
                    Array.Copy(extension, this.Count - StartOffset, iBuffer, 0, Count - this.Count + StartOffset);
                    StartOffset = Count - this.Count + StartOffset;
                }
                else
                {
                    Array.Copy(extension, 0, iBuffer, StartOffset, Count);
                    StartOffset += Count;
                }
                iPosition += Count;
            }
        }

        private float[] iBuffer;
        //0 to Count - 1
        private int StartOffset;

        public WaveRolling(int Length, uint SampleRate) : base(SampleRate)
        {
            iBuffer = new float[Length];
            StartOffset = 0;
            iPosition = -Length;
        }
    }

    //public class Wave16 : Wave, IReadOnlyList<short>
    //{
    //    private short[] iDDataObj;

    //    public override int Length
    //    {
    //        get
    //        {
    //            return iDDataObj.Length;
    //        }
    //    }

    //    public new short this[int index]
    //    {
    //        get
    //        {
    //            return iDDataObj[index];
    //        }
    //    }

    //    internal Wave16(uint pSampleRate, short[] pDataObj) : base(pSampleRate, x => pDataObj[x] / (float)32768)
    //    {
    //        iDDataObj = pDataObj;
    //    }

    //    /// <summary>Not Implemented</summary>
    //    private IEnumerator<short> GetEnumerator()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    /// <summary>Not Implemented</summary>
    //    private IEnumerator GetEnumerator2()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    IEnumerator<short> IEnumerable<short>.GetEnumerator()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //probably behaves right with length and stuff...

    //public class Wave24 : Wave, IReadOnlyList<int>
    //{
    //    private int[] iDDataObj;

    //    public override int Length
    //    {
    //        get
    //        {
    //            return iDDataObj.Length;
    //        }
    //    }

    //    public new int this[int index]
    //    {
    //        get
    //        {
    //            return iDDataObj[index];
    //        }
    //    }

    //    internal Wave24(uint pSampleRate, int[] pDataObj) : base(pSampleRate, x => (float)(pDataObj[x] / (double)8388608))
    //    {
    //        iDDataObj = pDataObj;
    //    }

    //    /// <summary>Not Implemented</summary>
    //    private IEnumerator<int> GetEnumerator()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    /// <summary>Not Implemented</summary>
    //    private IEnumerator GetEnumerator2()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    IEnumerator<int> IEnumerable<int>.GetEnumerator()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

}
