using BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal;

namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat
{
    public static class Defs
    {
        public const string NotAvailable = "n/a";
        public const string Micro = "\u03BC"; // the Greek letter in Unicode, not to be confused with ANSI "Latin-1 Supplement" character 'µ' "\u00B5"
        public const string Celsius = "\u00B0C";// "°C";
        public const string Ohm = "\u03A9"; // Greek letter omega: Ω
    }

    /// <summary>
    /// see https://bids-specification.readthedocs.io/en/stable/99-appendices/08-coordinate-systems.html#eeg-specific-coordinate-systems
    /// </summary>
    public enum CoordinateSystem { BESA = 1, Captrak = 2 }

    /// <summary>
    /// see https://bids-specification.readthedocs.io/en/stable/99-appendices/02-licenses.html
    /// </summary>
    public enum LicenseType
    {
        /// <summary>
        /// Public Domain. No license required for any purpose; the work is not subject to copyright in any jurisdiction.
        /// </summary>
        PD = 1,
        /// <summary>
        /// Open Data Commons Public Domain Dedication and License. License to assign public domain like permissions without giving up the copyright: http://opendatacommons.org/licenses/pddl/
        /// </summary>
        PDDL = 2,
        /// <summary>
        /// Creative Commons Zero 1.0 Universal. Use this if you are a holder of copyright or database rights, and you wish to waive all your interests in your work worldwide: http://opendatacommons.org/licenses/cc0/
        /// </summary>
        CC0 = 3,
    };

    /// <summary>
    /// see https://bids-specification.readthedocs.io/en/stable/99-appendices/05-units.html
    /// </summary>
    public enum Unit
    {
        None = 0,

        /// <summary>
        /// metre: length
        /// </summary>
        m = 1,

        /// <summary>
        /// kilogram: mass
        /// </summary>
        kg = 2,

        /// <summary>
        /// second: time
        /// </summary>
        s = 3,

        /// <summary>
        /// ampere: electric current
        /// </summary>
        A = 4,

        /// <summary>
        /// kelvin: thermodynamic temperature
        /// </summary>
        K = 5,

        /// <summary>
        /// mole: amount of substance
        /// </summary>
        mol = 6,

        /// <summary>
        /// candela: luminous intensity
        /// </summary>
        cd = 7,

        /// <summary>
        /// radian: angle
        /// </summary>
        rad = 8,

        /// <summary>
        /// steradian: solid angle
        /// </summary>
        sr = 9,

        /// <summary>
        /// hertz: frequency
        /// </summary>
        Hz = 10,

        /// <summary>
        /// newton: force, weight
        /// </summary>
        N = 11,

        /// <summary>
        /// pascal: pressure, stress
        /// </summary>
        Pa = 12,

        /// <summary>
        /// joule: energy, work, heat
        /// </summary>
        J = 13,

        /// <summary>
        /// watt: power, radiant flux
        /// </summary>
        W = 14,

        /// <summary>
        /// coulomb: electric charge or quantity of electricity
        /// </summary>
        C = 15,

        /// <summary>
        /// volt: voltage (electrical potential), emf
        /// </summary>
        V = 16,

        /// <summary>
        /// farad: capacitance
        /// </summary>
        F = 17,

        /// <summary>
        /// ohm: resistance, impedance, reactance
        /// </summary>
        [CustomEnumText(Defs.Ohm)]
        Ohm = 18,

        /// <summary>
        /// siemens: electrical conductance
        /// </summary>
        S = 19,

        /// <summary>
        /// weber: magnetic flux
        /// </summary>
        Wb = 21,

        /// <summary>
        /// tesla: magnetic flux density
        /// </summary>
        T = 22,

        /// <summary>
        /// henry: inductance
        /// </summary>
        H = 23,

        /// <summary>
        /// degree: °C temperature relative to 273.15 K
        /// </summary>
        [CustomEnumText(Defs.Celsius)]
        Celsius = 24,

        /// <summary>
        /// lumen: luminous flux
        /// </summary>
        lm = 25,

        /// <summary>
        /// lux: illuminance
        /// </summary>
        lx = 26,

        /// <summary>
        /// becquerel: radioactivity (decays per unit time)
        /// </summary>
        Bq = 27,

        /// <summary>
        /// gray: absorbed dose (of ionizing radiation)
        /// </summary>
        Gy = 28,

        /// <summary>
        /// sievert: equivalent dose (of ionizing radiation)
        /// </summary>
        Sv = 29,

        /// <summary>
        /// katal: catalytic activity
        /// </summary>
        kat = 30,
    }

    public enum Multiple
    {
        /// <summary>
        /// deca: 10^1
        /// </summary>
        da = 1,

        /// <summary>
        /// hecto: 10^2
        /// </summary>
        h = 2,

        /// <summary>
        /// kilo: 10^3
        /// </summary>
        k = 3,

        /// <summary>
        /// mega: 10^6
        /// </summary>
        M = 6,

        /// <summary>
        /// giga: 10^9
        /// </summary>
        G = 9,

        /// <summary>
        /// tera: 10^12
        /// </summary>
        T = 12,

        /// <summary>
        /// peta: 10^15
        /// </summary>
        P = 15,

        /// <summary>
        /// exa: 10^18
        /// </summary>
        E = 18,

        /// <summary>
        /// zetta: 10^21
        /// </summary>
        Z = 21,

        /// <summary>
        /// yotta: 10^24
        /// </summary>
        Y = 24,

        /// <summary>
        /// None: 10^0
        /// </summary>
        None = 0,

        /// <summary>
        /// deci: 10^-1
        /// </summary>
        d = -1,

        /// <summary>
        /// centi: 10^-2
        /// </summary>
        c = -2,

        /// <summary>
        /// milli: 10^-3
        /// </summary>
        m = -3,

        /// <summary>
        /// micro: 10^-6
        /// </summary>
        [CustomEnumText(Defs.Micro)]
        mi = -6,

        /// <summary>
        ///  nano: 10^-9
        /// </summary>
        n = -9,

        /// <summary>
        /// pico: 10^-12
        /// </summary>
        p = -12,

        /// <summary>
        /// femto: 10^-15
        /// </summary>
        f = -15,

        /// <summary>
        /// atto: 10^-18
        /// </summary>
        a = -18,

        /// <summary>
        /// zepto: 10^-21
        /// </summary>
        z = -21,

        /// <summary>
        /// yocto: 10^-24
        /// </summary>
        y = -24,
    }

    public enum ChannelType
    {
        /// <summary>
        /// Audio signal
        /// </summary>
        AUDIO = 1,

        /// <summary>
        /// Electroencephalogram channel
        /// </summary>
        EEG = 2,

        /// <summary>
        /// Generic electrooculogram(eye), different from HEOG and VEOG
        /// </summary>
        EOG = 3,

        /// <summary>
        /// Electrocardiogram(heart)
        /// </summary>
        ECG = 4,

        /// <summary>
        /// Electromyogram(muscle)
        /// </summary>
        EMG = 5,

        /// <summary>
        /// Eye tracker gaze
        /// </summary>
        EYEGAZE = 6,

        /// <summary>
        /// Galvanic skin response
        /// </summary>
        GSR = 7,

        /// <summary>
        /// Horizontal EOG(eye)
        /// </summary>
        HEOG = 8,

        /// <summary>
        /// Miscellaneous
        /// </summary>
        MISC = 9,

        /// <summary>
        /// Eye tracker pupil diameter
        /// </summary>
        PUPIL = 10,

        /// <summary>
        /// Reference channel
        /// </summary>
        REF = 11,

        /// <summary>
        /// Respiration
        /// </summary>
        RESP = 12,

        /// <summary>
        /// System time showing elapsed time since trial started
        /// </summary>
        SYSCLOCK = 13,

        /// <summary>
        /// Temperature
        /// </summary>
        TEMP = 14,

        /// <summary>
        /// System triggers
        /// </summary>
        TRIG = 15,

        /// <summary>
        /// Vertical EOG(eye)
        /// </summary>
        VEOG = 16,
    };
}
