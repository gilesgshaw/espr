namespace mus.Chorale
{

    // immutable, provided 'VoicingC' is
    // currently vunerable to invalid inputs
    // represents a chord voicing relative to an unknown 'note'
    // by convention pitch = ('note' +s Root) +c Voicing
    // so this does NOT narrow to Quad<IntervalC>
    public class Unit : Quad<IntervalS>
    {
        public IntervalS ToRoot { get; }
        public VoicingC Voicing { get; }

        public Unit(IntervalS toRoot, VoicingC voicing) : base(
            toRoot + voicing.S.Residue,
            toRoot + voicing.A.Residue,
            toRoot + voicing.T.Residue,
            toRoot + voicing.B.Residue)
        {
            ToRoot = toRoot;
            Voicing = voicing;
        }
    }

}
