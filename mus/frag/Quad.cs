using mus.Gen;
using System;
using System.Collections.Generic;

namespace mus.Chorale
{

    public static class Quad
    {
        public static Quad<TOut> Select<TIn, TOut>(IQuad<TIn> input, Func<TIn, TOut> selector)
        {
            return new Quad<TOut>(
                selector(input.S),
                selector(input.A),
                selector(input.T),
                selector(input.B));
        }
    }

    public interface IQuad<Ty>
    {
        Ty S { get; }
        Ty A { get; }
        Ty T { get; }
        Ty B { get; }
    }

    // immutable provided type parameter is not mutable reference type
    // no validation done on inputs
    // uses default comparisons (of base class)
    // does not 'value' components - just wraps relevant constructors
    public class VQuad<Ty> : TreeValued, IQuad<Ty>
    {
        // disabled, to prevent accidental neglect of children
        //public VQuad(Ty s, Ty a, Ty t, Ty b)
        //{
        //    S = s;
        //    A = a;
        //    T = t;
        //    B = b;
        //}

        public VQuad(Ty s, Ty a, Ty t, Ty b, double intrinticPenalty) : base(intrinticPenalty)
        {
            S = s;
            A = a;
            T = t;
            B = b;
        }

        public VQuad(Ty s, Ty a, Ty t, Ty b, IEnumerable<TreeValued> children) : base(children)
        {
            S = s;
            A = a;
            T = t;
            B = b;
        }

        public VQuad(Ty s, Ty a, Ty t, Ty b, IEnumerable<(double, TreeValued)> children) : base(children)
        {
            S = s;
            A = a;
            T = t;
            B = b;
        }

        public VQuad(Ty s, Ty a, Ty t, Ty b, IEnumerable<TreeValued> children, double intrinticPenalty) : base(children, intrinticPenalty)
        {
            S = s;
            A = a;
            T = t;
            B = b;
        }

        public VQuad(Ty s, Ty a, Ty t, Ty b, IEnumerable<(double, TreeValued)> children, double intrinticPenalty) : base(children, intrinticPenalty)
        {
            S = s;
            A = a;
            T = t;
            B = b;
        }

        public VQuad(Ty s, Ty a, Ty t, Ty b, double intrinticPenalty, double temporaryPenalty) : base(intrinticPenalty, temporaryPenalty)
        {
            S = s;
            A = a;
            T = t;
            B = b;
        }

        public VQuad(Ty s, Ty a, Ty t, Ty b, IEnumerable<TreeValued> children, double intrinticPenalty, double temporaryPenalty) : base(children, intrinticPenalty, temporaryPenalty)
        {
            S = s;
            A = a;
            T = t;
            B = b;
        }

        public VQuad(Ty s, Ty a, Ty t, Ty b, IEnumerable<(double, TreeValued)> children, double intrinticPenalty, double temporaryPenalty) : base(children, intrinticPenalty, temporaryPenalty)
        {
            S = s;
            A = a;
            T = t;
            B = b;
        }

        public override string ToString() => B.ToString() + "; " + T.ToString() + "; " + A.ToString() + "; " + S.ToString();

        public Ty S { get; }
        public Ty A { get; }
        public Ty T { get; }
        public Ty B { get; }
    }

    // immutable provided type parameter is not mutable reference type
    // no validation done on inputs
    // uses default comparisons
    public class Quad<Ty> : IQuad<Ty>
    {
        public Quad(Ty s, Ty a, Ty t, Ty b)
        {
            S = s;
            A = a;
            T = t;
            B = b;
        }

        public override string ToString() => B.ToString() + "; " + T.ToString() + "; " + A.ToString() + "; " + S.ToString();

        public Ty S { get; }
        public Ty A { get; }
        public Ty T { get; }
        public Ty B { get; }
    }

}
