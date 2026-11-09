(function (F, w) {
    "object" === typeof exports && "undefined" !== typeof module ? module.exports = w() : "function" === typeof define && define.amd ? define(w) : F.formula = w()
})(this, function () {
    var F = function (w) {
        var k = function () {
            var g = {};
            g.nil = Error("#NULL!");
            g.div0 = Error("#DIV/0!");
            g.value = Error("#VALUE!");
            g.ref = Error("#REF!");
            g.name = Error("#NAME?");
            g.num = Error("#NUM!");
            g.na = Error("#N/A");
            g.error = Error("#ERROR!");
            g.data = Error("#GETTING_DATA");
            return g
        }(),
            e = function () {
                var g = {
                    flattenShallow: function (a) {
                        return a && a.reduce ?
                            a.reduce(function (c, d) {
                                var f = Array.isArray(c),
                                    h = Array.isArray(d);
                                return f && h ? c.concat(d) : f ? (c.push(d), c) : h ? [c].concat(d) : [c, d]
                            }) : a
                    },
                    isFlat: function (a) {
                        if (!a) return !1;
                        for (var c = 0; c < a.length; ++c)
                            if (Array.isArray(a[c])) return !1;
                        return !0
                    },
                    flatten: function () {
                        for (var a = g.argsToArray.apply(null, arguments); !g.isFlat(a);) a = g.flattenShallow(a);
                        return a
                    },
                    argsToArray: function (a) {
                        var c = [];
                        g.arrayEach(a, function (d) {
                            c.push(d)
                        });
                        return c
                    },
                    numbers: function () {
                        return this.flatten.apply(null, arguments).filter(function (a) {
                            return "number" ===
                                typeof a
                        })
                    },
                    cleanFloat: function (a) {
                        return Math.round(1E14 * a) / 1E14
                    },
                    parseBool: function (a) {
                        if ("boolean" === typeof a || a instanceof Error) return a;
                        if ("number" === typeof a) return 0 !== a;
                        if ("string" === typeof a) {
                            var c = a.toUpperCase();
                            if ("TRUE" === c) return !0;
                            if ("FALSE" === c) return !1
                        }
                        return a instanceof Date && !isNaN(a) ? !0 : k.value
                    },
                    parseNumber: function (a) {
                        return void 0 === a || "" === a ? k.value : isNaN(a) ? k.value : parseFloat(a)
                    },
                    parseNumberArray: function (a) {
                        var c;
                        if (!a || 0 === (c = a.length)) return k.value;
                        for (var d; c--;) {
                            d =
                                g.parseNumber(a[c]);
                            if (d === k.value) return d;
                            a[c] = d
                        }
                        return a
                    },
                    parseMatrix: function (a) {
                        if (!a || 0 === a.length) return k.value;
                        for (var c, d = 0; d < a.length; d++)
                            if (c = g.parseNumberArray(a[d]), a[d] = c, c instanceof Error) return c;
                        return a
                    }
                },
                    b = new Date(Date.UTC(1900, 0, 1));
                g.parseDate = function (a) {
                    if (!isNaN(a)) {
                        if (a instanceof Date) return new Date(a);
                        a = parseInt(a, 10);
                        return 0 > a ? k.num : 60 >= a ? new Date(b.getTime() + 864E5 * (a - 1)) : new Date(b.getTime() + 864E5 * (a - 2))
                    }
                    return "string" !== typeof a || (a = new Date(a), isNaN(a)) ? k.value :
                        a
                };
                g.parseDateArray = function (a) {
                    for (var c = a.length, d; c--;) {
                        d = this.parseDate(a[c]);
                        if (d === k.value) return d;
                        a[c] = d
                    }
                    return a
                };
                g.anyIsError = function () {
                    for (var a = arguments.length; a--;)
                        if (arguments[a] instanceof Error) return !0;
                    return !1
                };
                g.arrayValuesToNumbers = function (a) {
                    for (var c = a.length, d; c--;) d = a[c], "number" !== typeof d && (!0 === d ? a[c] = 1 : !1 === d ? a[c] = 0 : "string" === typeof d && (d = this.parseNumber(d), a[c] = d instanceof Error ? 0 : d));
                    return a
                };
                g.rest = function (a, c) {
                    return a && "function" === typeof a.slice ? a.slice(c ||
                        1) : a
                };
                g.initial = function (a, c) {
                    return a && "function" === typeof a.slice ? a.slice(0, a.length - (c || 1)) : a
                };
                g.arrayEach = function (a, c) {
                    for (var d = -1, f = a.length; ++d < f && !1 !== c(a[d], d, a););
                    return a
                };
                g.transpose = function (a) {
                    return a ? a[0].map(function (c, d) {
                        return a.map(function (f) {
                            return f[d]
                        })
                    }) : k.value
                };
                return g
            }(),
            y = {};
        y.datetime = function () {
            function g(d) {
                return (d - a) / 864E5 + (-22038912E5 < d ? 2 : 1)
            }
            var b = {},
                a = new Date(1900, 0, 1),
                c = [
                    [],
                    [1, 2, 3, 4, 5, 6, 7],
                    [7, 1, 2, 3, 4, 5, 6],
                    [6, 0, 1, 2, 3, 4, 5],
                    [],
                    [],
                    [],
                    [],
                    [],
                    [],
                    [],
                    [7, 1, 2, 3, 4, 5, 6],
                    [6, 7, 1, 2, 3, 4, 5],
                    [5, 6, 7, 1, 2, 3, 4],
                    [4, 5, 6, 7, 1, 2, 3],
                    [3, 4, 5, 6, 7, 1, 2],
                    [2, 3, 4, 5, 6, 7, 1],
                    [1, 2, 3, 4, 5, 6, 7]
                ];
            b.DATE = function (d, f, h) {
                d = e.parseNumber(d);
                f = e.parseNumber(f);
                h = e.parseNumber(h);
                return e.anyIsError(d, f, h) ? k.value : 0 > d || 0 > f || 0 > h ? k.num : new Date(d, f - 1, h)
            };
            b.DATEVALUE = function (d) {
                if ("string" !== typeof d) return k.value;
                d = Date.parse(d);
                return isNaN(d) ? k.value : -22038912E5 >= d ? (d - a) / 864E5 + 1 : (d - a) / 864E5 + 2
            };
            b.DAY = function (d) {
                d = e.parseDate(d);
                return d instanceof Error ? d : d.getDate()
            };
            b.DAYS = function (d, f) {
                d = e.parseDate(d);
                f = e.parseDate(f);
                return d instanceof Error ? d : f instanceof Error ? f : g(d) - g(f)
            };
            b.DAYS360 = function (d, f, h) { };
            b.EDATE = function (d, f) {
                d = e.parseDate(d);
                if (d instanceof Error) return d;
                if (isNaN(f)) return k.value;
                f = parseInt(f, 10);
                d.setMonth(d.getMonth() + f);
                return g(d)
            };
            b.EOMONTH = function (d, f) {
                d = e.parseDate(d);
                if (d instanceof Error) return d;
                if (isNaN(f)) return k.value;
                f = parseInt(f, 10);
                return g(new Date(d.getFullYear(), d.getMonth() + f + 1, 0))
            };
            b.HOUR = function (d) {
                d = e.parseDate(d);
                return d instanceof Error ? d : d.getHours()
            };
            b.INTERVAL = function (d) {
                if ("number" !== typeof d && "string" !== typeof d) return k.value;
                d = parseInt(d, 10);
                var f = Math.floor(d / 94608E4);
                d %= 94608E4;
                var h = Math.floor(d / 2592E3);
                d %= 2592E3;
                var l = Math.floor(d / 86400);
                d %= 86400;
                var m = Math.floor(d / 3600);
                d %= 3600;
                var n = Math.floor(d / 60);
                d %= 60;
                return "P" + (0 < f ? f + "Y" : "") + (0 < h ? h + "M" : "") + (0 < l ? l + "D" : "") + "T" + (0 < m ? m + "H" : "") + (0 < n ? n + "M" : "") + (0 < d ? d + "S" : "")
            };
            b.ISOWEEKNUM = function (d) {
                d = e.parseDate(d);
                if (d instanceof Error) return d;
                d.setHours(0, 0, 0);
                d.setDate(d.getDate() + 4 - (d.getDay() ||
                    7));
                var f = new Date(d.getFullYear(), 0, 1);
                return Math.ceil(((d - f) / 864E5 + 1) / 7)
            };
            b.MINUTE = function (d) {
                d = e.parseDate(d);
                return d instanceof Error ? d : d.getMinutes()
            };
            b.MONTH = function (d) {
                d = e.parseDate(d);
                return d instanceof Error ? d : d.getMonth() + 1
            };
            b.NETWORKDAYS = function (d, f, h) { };
            b.NETWORKDAYS.INTL = function (d, f, h, l) { };
            b.NOW = function () {
                return new Date
            };
            b.SECOND = function (d) {
                d = e.parseDate(d);
                return d instanceof Error ? d : d.getSeconds()
            };
            b.TIME = function (d, f, h) {
                d = e.parseNumber(d);
                f = e.parseNumber(f);
                h = e.parseNumber(h);
                return e.anyIsError(d, f, h) ? k.value : 0 > d || 0 > f || 0 > h ? k.num : (3600 * d + 60 * f + h) / 86400
            };
            b.TIMEVALUE = function (d) {
                d = e.parseDate(d);
                return d instanceof Error ? d : (3600 * d.getHours() + 60 * d.getMinutes() + d.getSeconds()) / 86400
            };
            b.TODAY = function () {
                return new Date
            };
            b.WEEKDAY = function (d, f) {
                d = e.parseDate(d);
                if (d instanceof Error) return d;
                void 0 === f && (f = 1);
                d = d.getDay();
                return c[f][d]
            };
            b.WEEKNUM = function (d, f) { };
            b.WORKDAY = function (d, f, h) { };
            b.WORKDAY.INTL = function (d, f, h, l) { };
            b.YEAR = function (d) {
                d = e.parseDate(d);
                return d instanceof
                    Error ? d : d.getFullYear()
            };
            b.YEARFRAC = function (d, f, h) { };
            return b
        }();
        y.database = function () {
            function g(a, c) {
                for (var d = {}, f = 1; f < a[0].length; ++f) d[f] = !0;
                var h = c[0].length;
                for (f = 1; f < c.length; ++f) c[f].length > h && (h = c[f].length);
                for (f = 1; f < a.length; ++f)
                    for (var l = 1; l < a[f].length; ++l) {
                        for (var m = !1, n = !1, p = 0; p < c.length; ++p) {
                            var q = c[p];
                            if (!(q.length < h) && a[f][0] === q[0]) {
                                n = !0;
                                for (var t = 1; t < q.length; ++t) m = m || eval(a[f][l] + q[t])
                            }
                        }
                        n && (d[l] = d[l] && m)
                    }
                c = [];
                for (h = 0; h < a[0].length; ++h) d[h] && c.push(h - 1);
                return c
            }
            var b = {
                FINDFIELD: function (a,
                    c) {
                    for (var d = null, f = 0; f < a.length; f++)
                        if (a[f][0] === c) {
                            d = f;
                            break
                        } return null == d ? k.value : d
                },
                DAVERAGE: function (a, c, d) {
                    if (isNaN(c) && "string" !== typeof c) return k.value;
                    d = g(a, d);
                    "string" === typeof c ? (c = b.FINDFIELD(a, c), a = e.rest(a[c])) : a = e.rest(a[c]);
                    for (var f = c = 0; f < d.length; f++) c += a[d[f]];
                    return 0 === d.length ? k.div0 : c / d.length
                },
                DCOUNT: function (a, c, d) { },
                DCOUNTA: function (a, c, d) { },
                DGET: function (a, c, d) {
                    if (isNaN(c) && "string" !== typeof c) return k.value;
                    d = g(a, d);
                    "string" === typeof c ? (c = b.FINDFIELD(a, c), a = e.rest(a[c])) :
                        a = e.rest(a[c]);
                    return 0 === d.length ? k.value : 1 < d.length ? k.num : a[d[0]]
                },
                DMAX: function (a, c, d) {
                    if (isNaN(c) && "string" !== typeof c) return k.value;
                    d = g(a, d);
                    "string" === typeof c ? (c = b.FINDFIELD(a, c), a = e.rest(a[c])) : a = e.rest(a[c]);
                    c = a[d[0]];
                    for (var f = 1; f < d.length; f++) c < a[d[f]] && (c = a[d[f]]);
                    return c
                },
                DMIN: function (a, c, d) {
                    if (isNaN(c) && "string" !== typeof c) return k.value;
                    d = g(a, d);
                    "string" === typeof c ? (c = b.FINDFIELD(a, c), a = e.rest(a[c])) : a = e.rest(a[c]);
                    c = a[d[0]];
                    for (var f = 1; f < d.length; f++) c > a[d[f]] && (c = a[d[f]]);
                    return c
                },
                DPRODUCT: function (a, c, d) {
                    if (isNaN(c) && "string" !== typeof c) return k.value;
                    d = g(a, d);
                    if ("string" === typeof c) {
                        c = b.FINDFIELD(a, c);
                        var f = e.rest(a[c])
                    } else f = e.rest(a[c]);
                    a = [];
                    for (c = 0; c < d.length; c++) a[c] = f[d[c]];
                    if (d = a)
                        for (a = [], c = 0; c < d.length; ++c) d[c] && a.push(d[c]);
                    else a = d;
                    d = 1;
                    for (c = 0; c < a.length; c++) d *= a[c];
                    return d
                },
                DSTDEV: function (a, c, d) { },
                DSTDEVP: function (a, c, d) { },
                DSUM: function (a, c, d) { },
                DVAR: function (a, c, d) { },
                DVARP: function (a, c, d) { },
                MATCH: function (a, c, d) {
                    if (!a && !c) return k.na;
                    2 === arguments.length && (d =
                        1);
                    if (!(c instanceof Array) || -1 !== d && 0 !== d && 1 !== d) return k.na;
                    for (var f, h, l = 0; l < c.length; l++)
                        if (1 === d) {
                            if (c[l] === a) return l + 1;
                            c[l] < a && (h ? c[l] > h && (f = l + 1, h = c[l]) : (f = l + 1, h = c[l]))
                        } else if (0 === d)
                            if ("string" === typeof a) {
                                if (a = a.replace(/\?/g, "."), c[l].toLowerCase().match(a.toLowerCase())) return l + 1
                            } else {
                                if (c[l] === a) return l + 1
                            }
                        else if (-1 === d) {
                            if (c[l] === a) return l + 1;
                            c[l] > a && (h ? c[l] < h && (f = l + 1, h = c[l]) : (f = l + 1, h = c[l]))
                        }
                    return f ? f : k.na
                }
            };
            return b
        }();
        y.engineering = function () {
            var g = {
                BESSELI: function (b, a) { },
                BESSELJ: function (b,
                    a) { },
                BESSELK: function (b, a) { },
                BESSELY: function (b, a) { },
                BIN2DEC: function (b) {
                    if (!/^[01]{1,10}$/.test(b)) return k.num;
                    var a = parseInt(b, 2);
                    b = b.toString();
                    return 10 === b.length && "1" === b.substring(0, 1) ? parseInt(b.substring(1), 2) - 512 : a
                },
                BIN2HEX: function (b, a) {
                    if (!/^[01]{1,10}$/.test(b)) return k.num;
                    var c = b.toString();
                    if (10 === c.length && "1" === c.substring(0, 1)) return (0xfffffffe00 + parseInt(c.substring(1), 2)).toString(16);
                    b = parseInt(b, 2).toString(16);
                    if (void 0 === a) return b;
                    if (isNaN(a)) return k.value;
                    if (0 > a) return k.num;
                    a = Math.floor(a);
                    return a >= b.length ? REPT("0", a - b.length) + b : k.num
                },
                BIN2OCT: function (b, a) {
                    if (!/^[01]{1,10}$/.test(b)) return k.num;
                    var c = b.toString();
                    if (10 === c.length && "1" === c.substring(0, 1)) return (1073741312 + parseInt(c.substring(1), 2)).toString(8);
                    b = parseInt(b, 2).toString(8);
                    if (void 0 === a) return b;
                    if (isNaN(a)) return k.value;
                    if (0 > a) return k.num;
                    a = Math.floor(a);
                    return a >= b.length ? REPT("0", a - b.length) + b : k.num
                },
                BITAND: function (b, a) {
                    b = e.parseNumber(b);
                    a = e.parseNumber(a);
                    return e.anyIsError(b, a) ? k.value : 0 >
                        b || 0 > a || Math.floor(b) !== b || Math.floor(a) !== a || 0xffffffffffff < b || 0xffffffffffff < a ? k.num : b & a
                },
                BITLSHIFT: function (b, a) {
                    b = e.parseNumber(b);
                    a = e.parseNumber(a);
                    return e.anyIsError(b, a) ? k.value : 0 > b || Math.floor(b) !== b || 0xffffffffffff < b || 53 < Math.abs(a) ? k.num : 0 <= a ? b << a : b >> -a
                },
                BITOR: function (b, a) {
                    b = e.parseNumber(b);
                    a = e.parseNumber(a);
                    return e.anyIsError(b, a) ? k.value : 0 > b || 0 > a || Math.floor(b) !== b || Math.floor(a) !== a || 0xffffffffffff < b || 0xffffffffffff < a ? k.num : b | a
                },
                BITRSHIFT: function (b, a) {
                    b = e.parseNumber(b);
                    a = e.parseNumber(a);
                    return e.anyIsError(b, a) ? k.value : 0 > b || Math.floor(b) !== b || 0xffffffffffff < b || 53 < Math.abs(a) ? k.num : 0 <= a ? b >> a : b << -a
                },
                BITXOR: function (b, a) {
                    b = e.parseNumber(b);
                    a = e.parseNumber(a);
                    return e.anyIsError(b, a) ? k.value : 0 > b || 0 > a || Math.floor(b) !== b || Math.floor(a) !== a || 0xffffffffffff < b || 0xffffffffffff < a ? k.num : b ^ a
                },
                COMPLEX: function (b, a, c) {
                    b = e.parseNumber(b);
                    a = e.parseNumber(a);
                    if (e.anyIsError(b, a)) return b;
                    c = void 0 === c ? "i" : c;
                    return "i" !== c && "j" !== c ? k.value : 0 === b && 0 === a ? 0 : 0 === b ? 1 === a ? c : a.toString() + c : 0 === a ? b.toString() :
                        b.toString() + (0 < a ? "+" : "") + (1 === a ? c : a.toString() + c)
                },
                CONVERT: function (b, a, c) {
                    b = e.parseNumber(b);
                    if (b instanceof Error) return b;
                    for (var d = [
                        ["a.u. of action", "?", null, "action", !1, !1, 1.05457168181818E-34],
                        ["a.u. of charge", "e", null, "electric_charge", !1, !1, 1.60217653141414E-19],
                        ["a.u. of energy", "Eh", null, "energy", !1, !1, 4.35974417757576E-18],
                        ["a.u. of length", "a?", null, "length", !1, !1, 5.29177210818182E-11],
                        ["a.u. of mass", "m?", null, "mass", !1, !1, 9.10938261616162E-31],
                        ["a.u. of time", "?/Eh", null, "time", !1,
                            !1, 2.41888432650516E-17
                        ],
                        ["admiralty knot", "admkn", null, "speed", !1, !0, .514773333],
                        ["ampere", "A", null, "electric_current", !0, !1, 1],
                        ["ampere per meter", "A/m", null, "magnetic_field_intensity", !0, !1, 1],
                        ["\u00e5ngstr\u00f6m", "\u00c5", ["ang"], "length", !1, !0, 1E-10],
                        ["are", "ar", null, "area", !1, !0, 100],
                        ["astronomical unit", "ua", null, "length", !1, !1, 1.49597870691667E-11],
                        ["bar", "bar", null, "pressure", !1, !1, 1E5],
                        ["barn", "b", null, "area", !1, !1, 1E-28],
                        ["becquerel", "Bq", null, "radioactivity", !0, !1, 1],
                        ["bit", "bit", ["b"],
                            "information", !1, !0, 1
                        ],
                        ["btu", "BTU", ["btu"], "energy", !1, !0, 1055.05585262],
                        ["byte", "byte", null, "information", !1, !0, 8],
                        ["candela", "cd", null, "luminous_intensity", !0, !1, 1],
                        ["candela per square metre", "cd/m?", null, "luminance", !0, !1, 1],
                        ["coulomb", "C", null, "electric_charge", !0, !1, 1],
                        ["cubic \u00e5ngstr\u00f6m", "ang3", ["ang^3"], "volume", !1, !0, 1E-30],
                        ["cubic foot", "ft3", ["ft^3"], "volume", !1, !0, .028316846592],
                        ["cubic inch", "in3", ["in^3"], "volume", !1, !0, 1.6387064E-5],
                        ["cubic light-year", "ly3", ["ly^3"], "volume",
                            !1, !0, 8.46786664623715E-47
                        ],
                        ["cubic metre", "m?", null, "volume", !0, !0, 1],
                        ["cubic mile", "mi3", ["mi^3"], "volume", !1, !0, 4.16818182544058E9],
                        ["cubic nautical mile", "Nmi3", ["Nmi^3"], "volume", !1, !0, 6352182208],
                        ["cubic Pica", "Pica3", ["Picapt3", "Pica^3", "Picapt^3"], "volume", !1, !0, 7.58660370370369E-8],
                        ["cubic yard", "yd3", ["yd^3"], "volume", !1, !0, .764554857984],
                        ["cup", "cup", null, "volume", !1, !0, 2.365882365E-4],
                        ["dalton", "Da", ["u"], "mass", !1, !1, 1.66053886282828E-27],
                        ["day", "d", ["day"], "time", !1, !0, 86400],
                        ["degree",
                            "\u00b0", null, "angle", !1, !1, .0174532925199433
                        ],
                        ["degrees Rankine", "Rank", null, "temperature", !1, !0, .555555555555556],
                        ["dyne", "dyn", ["dy"], "force", !1, !0, 1E-5],
                        ["electronvolt", "eV", ["ev"], "energy", !1, !0, 1.60217656514141],
                        ["ell", "ell", null, "length", !1, !0, 1.143],
                        ["erg", "erg", ["e"], "energy", !1, !0, 1E-7],
                        ["farad", "F", null, "electric_capacitance", !0, !1, 1],
                        ["fluid ounce", "oz", null, "volume", !1, !0, 2.95735295625E-5],
                        ["foot", "ft", null, "length", !1, !0, .3048],
                        ["foot-pound", "flb", null, "energy", !1, !0, 1.3558179483314],
                        ["gal", "Gal", null, "acceleration", !1, !1, .01],
                        ["gallon", "gal", null, "volume", !1, !0, .003785411784],
                        ["gauss", "G", ["ga"], "magnetic_flux_density", !1, !0, 1],
                        ["grain", "grain", null, "mass", !1, !0, 6.47989E-5],
                        ["gram", "g", null, "mass", !1, !0, .001],
                        ["gray", "Gy", null, "absorbed_dose", !0, !1, 1],
                        ["gross registered ton", "GRT", ["regton"], "volume", !1, !0, 2.8316846592],
                        ["hectare", "ha", null, "area", !1, !0, 1E4],
                        ["henry", "H", null, "inductance", !0, !1, 1],
                        ["hertz", "Hz", null, "frequency", !0, !1, 1],
                        ["horsepower", "HP", ["h"], "power", !1, !0,
                            745.69987158227
                        ],
                        ["horsepower-hour", "HPh", ["hh", "hph"], "energy", !1, !0, 2684519.538],
                        ["hour", "h", ["hr"], "time", !1, !0, 3600],
                        ["imperial gallon (U.K.)", "uk_gal", null, "volume", !1, !0, .00454609],
                        ["imperial hundredweight", "lcwt", ["uk_cwt", "hweight"], "mass", !1, !0, 50.802345],
                        ["imperial quart (U.K)", "uk_qt", null, "volume", !1, !0, .0011365225],
                        ["imperial ton", "brton", ["uk_ton", "LTON"], "mass", !1, !0, 1016.046909],
                        ["inch", "in", null, "length", !1, !0, .0254],
                        ["international acre", "uk_acre", null, "area", !1, !0, 4046.8564224],
                        ["IT calorie", "cal", null, "energy", !1, !0, 4.1868],
                        ["joule", "J", null, "energy", !0, !0, 1],
                        ["katal", "kat", null, "catalytic_activity", !0, !1, 1],
                        ["kelvin", "K", ["kel"], "temperature", !0, !0, 1],
                        ["kilogram", "kg", null, "mass", !0, !0, 1],
                        ["knot", "kn", null, "speed", !1, !0, .514444444444444],
                        ["light-year", "ly", null, "length", !1, !0, 9460730472580800],
                        ["litre", "L", ["l", "lt"], "volume", !1, !0, .001],
                        ["lumen", "lm", null, "luminous_flux", !0, !1, 1],
                        ["lux", "lx", null, "illuminance", !0, !1, 1],
                        ["maxwell", "Mx", null, "magnetic_flux", !1, !1, 1E-18],
                        ["measurement ton", "MTON", null, "volume", !1, !0, 1.13267386368],
                        ["meter per hour", "m/h", ["m/hr"], "speed", !1, !0, 2.7777777777778E-4],
                        ["meter per second", "m/s", ["m/sec"], "speed", !0, !0, 1],
                        ["meter per second squared", "m?s??", null, "acceleration", !0, !1, 1],
                        ["parsec", "pc", ["parsec"], "length", !1, !0, 0x6da012f958ee1c],
                        ["meter squared per second", "m?/s", null, "kinematic_viscosity", !0, !1, 1],
                        ["metre", "m", null, "length", !0, !0, 1],
                        ["miles per hour", "mph", null, "speed", !1, !0, .44704],
                        ["millimetre of mercury", "mmHg", null, "pressure",
                            !1, !1, 133.322
                        ],
                        ["minute", "?", null, "angle", !1, !1, 2.90888208665722E-4],
                        ["minute", "min", ["mn"], "time", !1, !0, 60],
                        ["modern teaspoon", "tspm", null, "volume", !1, !0, 5E-6],
                        ["mole", "mol", null, "amount_of_substance", !0, !1, 1],
                        ["morgen", "Morgen", null, "area", !1, !0, 2500],
                        ["n.u. of action", "?", null, "action", !1, !1, 1.05457168181818E-34],
                        ["n.u. of mass", "m?", null, "mass", !1, !1, 9.10938261616162E-31],
                        ["n.u. of speed", "c?", null, "speed", !1, !1, 299792458],
                        ["n.u. of time", "?/(me?c??)", null, "time", !1, !1, 1.28808866778687E-21],
                        ["nautical mile",
                            "M", ["Nmi"], "length", !1, !0, 1852
                        ],
                        ["newton", "N", null, "force", !0, !0, 1],
                        ["\u0153rsted", "Oe ", null, "magnetic_field_intensity", !1, !1, 79.5774715459477],
                        ["ohm", "\u03a9", null, "electric_resistance", !0, !1, 1],
                        ["ounce mass", "ozm", null, "mass", !1, !0, .028349523125],
                        ["pascal", "Pa", null, "pressure", !0, !1, 1],
                        ["pascal second", "Pa?s", null, "dynamic_viscosity", !0, !1, 1],
                        ["pferdest\u00e4rke", "PS", null, "power", !1, !0, 735.49875],
                        ["phot", "ph", null, "illuminance", !1, !1, 1E-4],
                        ["pica (1/6 inch)", "pica", null, "length", !1, !0, 3.5277777777778E-4],
                        ["pica (1/72 inch)", "Pica", ["Picapt"], "length", !1, !0, .00423333333333333],
                        ["poise", "P", null, "dynamic_viscosity", !1, !1, .1],
                        ["pond", "pond", null, "force", !1, !0, .00980665],
                        ["pound force", "lbf", null, "force", !1, !0, 4.4482216152605],
                        ["pound mass", "lbm", null, "mass", !1, !0, .45359237],
                        ["quart", "qt", null, "volume", !1, !0, 9.46352946E-4],
                        ["radian", "rad", null, "angle", !0, !1, 1],
                        ["second", "?", null, "angle", !1, !1, 4.84813681109536E-6],
                        ["second", "s", ["sec"], "time", !0, !0, 1],
                        ["short hundredweight", "cwt", ["shweight"], "mass", !1,
                            !0, 45.359237
                        ],
                        ["siemens", "S", null, "electrical_conductance", !0, !1, 1],
                        ["sievert", "Sv", null, "equivalent_dose", !0, !1, 1],
                        ["slug", "sg", null, "mass", !1, !0, 14.59390294],
                        ["square \u00e5ngstr\u00f6m", "ang2", ["ang^2"], "area", !1, !0, 1E-20],
                        ["square foot", "ft2", ["ft^2"], "area", !1, !0, .09290304],
                        ["square inch", "in2", ["in^2"], "area", !1, !0, 6.4516E-4],
                        ["square light-year", "ly2", ["ly^2"], "area", !1, !0, 8.95054210748189E31],
                        ["square meter", "m?", null, "area", !0, !0, 1],
                        ["square mile", "mi2", ["mi^2"], "area", !1, !0, 2589988.110336],
                        ["square nautical mile", "Nmi2", ["Nmi^2"], "area", !1, !0, 3429904],
                        ["square Pica", "Pica2", ["Picapt2", "Pica^2", "Picapt^2"], "area", !1, !0, 1.792111111111E-5],
                        ["square yard", "yd2", ["yd^2"], "area", !1, !0, .83612736],
                        ["statute mile", "mi", null, "length", !1, !0, 1609.344],
                        ["steradian", "sr", null, "solid_angle", !0, !1, 1],
                        ["stilb", "sb", null, "luminance", !1, !1, 1E-4],
                        ["stokes", "St", null, "kinematic_viscosity", !1, !1, 1E-4],
                        ["stone", "stone", null, "mass", !1, !0, 6.35029318],
                        ["tablespoon", "tbs", null, "volume", !1, !0, 1.47868E-5],
                        ["teaspoon",
                            "tsp", null, "volume", !1, !0, 4.92892E-6
                        ],
                        ["tesla", "T", null, "magnetic_flux_density", !0, !0, 1],
                        ["thermodynamic calorie", "c", null, "energy", !1, !0, 4.184],
                        ["ton", "ton", null, "mass", !1, !0, 907.18474],
                        ["tonne", "t", null, "mass", !1, !1, 1E3],
                        ["U.K. pint", "uk_pt", null, "volume", !1, !0, 5.6826125E-4],
                        ["U.S. bushel", "bushel", null, "volume", !1, !0, .03523907],
                        ["U.S. oil barrel", "barrel", null, "volume", !1, !0, .158987295],
                        ["U.S. pint", "pt", ["us_pt"], "volume", !1, !0, 4.73176473E-4],
                        ["U.S. survey mile", "survey_mi", null, "length", !1, !0,
                            1609.347219
                        ],
                        ["U.S. survey/statute acre", "us_acre", null, "area", !1, !0, 4046.87261],
                        ["volt", "V", null, "voltage", !0, !1, 1],
                        ["watt", "W", null, "power", !0, !0, 1],
                        ["watt-hour", "Wh", ["wh"], "energy", !1, !0, 3600],
                        ["weber", "Wb", null, "magnetic_flux", !0, !1, 1],
                        ["yard", "yd", null, "length", !1, !0, .9144],
                        ["year", "yr", null, "time", !1, !0, 31557600]
                    ], f = {
                        Yi: ["yobi", 80, 1.2089258196146292E24, "Yi", "yotta"],
                        Zi: ["zebi", 70, 1.1805916207174113E21, "Zi", "zetta"],
                        Ei: ["exbi", 60, 0x1000000000000000, "Ei", "exa"],
                        Pi: ["pebi", 50, 0x4000000000000,
                            "Pi", "peta"
                        ],
                        Ti: ["tebi", 40, 1099511627776, "Ti", "tera"],
                        Gi: ["gibi", 30, 1073741824, "Gi", "giga"],
                        Mi: ["mebi", 20, 1048576, "Mi", "mega"],
                        ki: ["kibi", 10, 1024, "ki", "kilo"]
                    }, h = {
                        Y: ["yotta", 1E24, "Y"],
                        Z: ["zetta", 1E21, "Z"],
                        E: ["exa", 1E18, "E"],
                        P: ["peta", 1E15, "P"],
                        T: ["tera", 1E12, "T"],
                        G: ["giga", 1E9, "G"],
                        M: ["mega", 1E6, "M"],
                        k: ["kilo", 1E3, "k"],
                        h: ["hecto", 100, "h"],
                        e: ["dekao", 10, "e"],
                        d: ["deci", .1, "d"],
                        c: ["centi", .01, "c"],
                        m: ["milli", .001, "m"],
                        u: ["micro", 1E-6, "u"],
                        n: ["nano", 1E-9, "n"],
                        p: ["pico", 1E-12, "p"],
                        f: ["femto", 1E-15, "f"],
                        a: ["atto", 1E-18, "a"],
                        z: ["zepto", 1E-21, "z"],
                        y: ["yocto", 1E-24, "y"]
                    }, l = null, m = null, n = a, p = c, q = 1, t = 1, u, r = 0; r < d.length; r++) {
                        u = null === d[r][2] ? [] : d[r][2];
                        if (d[r][1] === n || 0 <= u.indexOf(n)) l = d[r];
                        if (d[r][1] === p || 0 <= u.indexOf(p)) m = d[r]
                    }
                    if (null === l)
                        for (u = f[a.substring(0, 2)], r = h[a.substring(0, 1)], "da" === a.substring(0, 2) && (r = ["dekao", 10, "da"]), u ? (q = u[2], n = a.substring(2)) : r && (q = r[1], n = a.substring(r[2].length)), a = 0; a < d.length; a++)
                            if (u = null === d[a][2] ? [] : d[a][2], d[a][1] === n || 0 <= u.indexOf(n)) l = d[a];
                    if (null === m)
                        for (f =
                            f[c.substring(0, 2)], h = h[c.substring(0, 1)], "da" === c.substring(0, 2) && (h = ["dekao", 10, "da"]), f ? (t = f[2], p = c.substring(2)) : h && (t = h[1], p = c.substring(h[2].length)), c = 0; c < d.length; c++)
                            if (u = null === d[c][2] ? [] : d[c][2], d[c][1] === p || 0 <= u.indexOf(p)) m = d[c];
                    return null === l || null === m || l[3] !== m[3] ? k.na : b * l[6] * q / (m[6] * t)
                },
                DEC2BIN: function (b, a) {
                    b = e.parseNumber(b);
                    if (b instanceof Error) return b;
                    if (!/^-?[0-9]{1,3}$/.test(b) || -512 > b || 511 < b) return k.num;
                    if (0 > b) return "1" + REPT("0", 9 - (512 + b).toString(2).length) + (512 + b).toString(2);
                    b = parseInt(b, 10).toString(2);
                    if ("undefined" === typeof a) return b;
                    if (isNaN(a)) return k.value;
                    if (0 > a) return k.num;
                    a = Math.floor(a);
                    return a >= b.length ? REPT("0", a - b.length) + b : k.num
                },
                DEC2HEX: function (b, a) {
                    b = e.parseNumber(b);
                    if (b instanceof Error) return b;
                    if (!/^-?[0-9]{1,12}$/.test(b) || -549755813888 > b || 549755813887 < b) return k.num;
                    if (0 > b) return (1099511627776 + b).toString(16);
                    b = parseInt(b, 10).toString(16);
                    if ("undefined" === typeof a) return b;
                    if (isNaN(a)) return k.value;
                    if (0 > a) return k.num;
                    a = Math.floor(a);
                    return a >=
                        b.length ? REPT("0", a - b.length) + b : k.num
                },
                DEC2OCT: function (b, a) {
                    b = e.parseNumber(b);
                    if (b instanceof Error) return b;
                    if (!/^-?[0-9]{1,9}$/.test(b) || -536870912 > b || 536870911 < b) return k.num;
                    if (0 > b) return (1073741824 + b).toString(8);
                    b = parseInt(b, 10).toString(8);
                    if ("undefined" === typeof a) return b;
                    if (isNaN(a)) return k.value;
                    if (0 > a) return k.num;
                    a = Math.floor(a);
                    return a >= b.length ? REPT("0", a - b.length) + b : k.num
                },
                DELTA: function (b, a) {
                    a = void 0 === a ? 0 : a;
                    b = e.parseNumber(b);
                    a = e.parseNumber(a);
                    return e.anyIsError(b, a) ? k.value :
                        b === a ? 1 : 0
                },
                ERF: function (b, a) { }
            };
            g.ERF.PRECISE = function () { };
            g.ERFC = function (b) { };
            g.ERFC.PRECISE = function () { };
            g.GESTEP = function (b, a) {
                a = a || 0;
                b = e.parseNumber(b);
                return e.anyIsError(a, b) ? b : b >= a ? 1 : 0
            };
            g.HEX2BIN = function (b, a) {
                if (!/^[0-9A-Fa-f]{1,10}$/.test(b)) return k.num;
                var c = 10 === b.length && "f" === b.substring(0, 1).toLowerCase() ? !0 : !1;
                b = c ? parseInt(b, 16) - 1099511627776 : parseInt(b, 16);
                if (-512 > b || 511 < b) return k.num;
                if (c) return "1" + REPT("0", 9 - (512 + b).toString(2).length) + (512 + b).toString(2);
                c = b.toString(2);
                if (void 0 ===
                    a) return c;
                if (isNaN(a)) return k.value;
                if (0 > a) return k.num;
                a = Math.floor(a);
                return a >= c.length ? REPT("0", a - c.length) + c : k.num
            };
            g.HEX2DEC = function (b) {
                if (!/^[0-9A-Fa-f]{1,10}$/.test(b)) return k.num;
                b = parseInt(b, 16);
                return 549755813888 <= b ? b - 1099511627776 : b
            };
            g.HEX2OCT = function (b, a) {
                if (!/^[0-9A-Fa-f]{1,10}$/.test(b)) return k.num;
                b = parseInt(b, 16);
                if (536870911 < b && 0xffe0000000 > b) return k.num;
                if (0xffe0000000 <= b) return (b - 0xffc0000000).toString(8);
                b = b.toString(8);
                if (void 0 === a) return b;
                if (isNaN(a)) return k.value;
                if (0 > a) return k.num;
                a = Math.floor(a);
                return a >= b.length ? REPT("0", a - b.length) + b : k.num
            };
            g.IMABS = function (b) {
                var a = g.IMREAL(b);
                b = g.IMAGINARY(b);
                return e.anyIsError(a, b) ? k.value : Math.sqrt(Math.pow(a, 2) + Math.pow(b, 2))
            };
            g.IMAGINARY = function (b) {
                if (void 0 === b || !0 === b || !1 === b) return k.value;
                if (0 === b || "0" === b) return 0;
                if (0 <= ["i", "j"].indexOf(b)) return 1;
                b = b.replace("+i", "+1i").replace("-i", "-1i").replace("+j", "+1j").replace("-j", "-1j");
                var a = b.indexOf("+"),
                    c = b.indexOf("-");
                0 === a && (a = b.indexOf("+", 1));
                0 === c &&
                    (c = b.indexOf("-", 1));
                var d = b.substring(b.length - 1, b.length);
                d = "i" === d || "j" === d;
                return 0 <= a || 0 <= c ? d ? 0 <= a ? isNaN(b.substring(0, a)) || isNaN(b.substring(a + 1, b.length - 1)) ? k.num : Number(b.substring(a + 1, b.length - 1)) : isNaN(b.substring(0, c)) || isNaN(b.substring(c + 1, b.length - 1)) ? k.num : -Number(b.substring(c + 1, b.length - 1)) : k.num : d ? isNaN(b.substring(0, b.length - 1)) ? k.num : b.substring(0, b.length - 1) : isNaN(b) ? k.num : 0
            };
            g.IMARGUMENT = function (b) {
                var a = g.IMREAL(b);
                b = g.IMAGINARY(b);
                return e.anyIsError(a, b) ? k.value : 0 === a && 0 ===
                    b ? k.div0 : 0 === a && 0 < b ? Math.PI / 2 : 0 === a && 0 > b ? -Math.PI / 2 : 0 === b && 0 < a ? 0 : 0 === b && 0 > a ? -Math.PI : 0 < a ? Math.atan(b / a) : 0 > a && 0 <= b ? Math.atan(b / a) + Math.PI : Math.atan(b / a) - Math.PI
            };
            g.IMCONJUGATE = function (b) {
                var a = g.IMREAL(b),
                    c = g.IMAGINARY(b);
                if (e.anyIsError(a, c)) return k.value;
                var d = b.substring(b.length - 1);
                return 0 !== c ? g.COMPLEX(a, -c, "i" === d || "j" === d ? d : "i") : b
            };
            g.IMCOS = function (b) {
                var a = g.IMREAL(b),
                    c = g.IMAGINARY(b);
                if (e.anyIsError(a, c)) return k.value;
                b = b.substring(b.length - 1);
                return g.COMPLEX(Math.cos(a) * (Math.exp(c) +
                    Math.exp(-c)) / 2, -Math.sin(a) * (Math.exp(c) - Math.exp(-c)) / 2, "i" === b || "j" === b ? b : "i")
            };
            g.IMCOSH = function (b) {
                var a = g.IMREAL(b),
                    c = g.IMAGINARY(b);
                if (e.anyIsError(a, c)) return k.value;
                b = b.substring(b.length - 1);
                return g.COMPLEX(Math.cos(c) * (Math.exp(a) + Math.exp(-a)) / 2, Math.sin(c) * (Math.exp(a) - Math.exp(-a)) / 2, "i" === b || "j" === b ? b : "i")
            };
            g.IMCOT = function (b) {
                var a = g.IMREAL(b),
                    c = g.IMAGINARY(b);
                return e.anyIsError(a, c) ? k.value : g.IMDIV(g.IMCOS(b), g.IMSIN(b))
            };
            g.IMDIV = function (b, a) {
                var c = g.IMREAL(b),
                    d = g.IMAGINARY(b),
                    f = g.IMREAL(a),
                    h = g.IMAGINARY(a);
                if (e.anyIsError(c, d, f, h)) return k.value;
                b = b.substring(b.length - 1);
                var l = a.substring(a.length - 1);
                a = "i";
                "j" === b ? a = "j" : "j" === l && (a = "j");
                if (0 === f && 0 === h) return k.num;
                b = f * f + h * h;
                return g.COMPLEX((c * f + d * h) / b, (d * f - c * h) / b, a)
            };
            g.IMEXP = function (b) {
                var a = g.IMREAL(b),
                    c = g.IMAGINARY(b);
                if (e.anyIsError(a, c)) return k.value;
                b = b.substring(b.length - 1);
                a = Math.exp(a);
                return g.COMPLEX(a * Math.cos(c), a * Math.sin(c), "i" === b || "j" === b ? b : "i")
            };
            g.IMLN = function (b) {
                var a = g.IMREAL(b),
                    c = g.IMAGINARY(b);
                if (e.anyIsError(a, c)) return k.value;
                b = b.substring(b.length - 1);
                return g.COMPLEX(Math.log(Math.sqrt(a * a + c * c)), Math.atan(c / a), "i" === b || "j" === b ? b : "i")
            };
            g.IMLOG10 = function (b) {
                var a = g.IMREAL(b),
                    c = g.IMAGINARY(b);
                if (e.anyIsError(a, c)) return k.value;
                b = b.substring(b.length - 1);
                return g.COMPLEX(Math.log(Math.sqrt(a * a + c * c)) / Math.log(10), Math.atan(c / a) / Math.log(10), "i" === b || "j" === b ? b : "i")
            };
            g.IMLOG2 = function (b) {
                var a = g.IMREAL(b),
                    c = g.IMAGINARY(b);
                if (e.anyIsError(a, c)) return k.value;
                b = b.substring(b.length - 1);
                return g.COMPLEX(Math.log(Math.sqrt(a *
                    a + c * c)) / Math.log(2), Math.atan(c / a) / Math.log(2), "i" === b || "j" === b ? b : "i")
            };
            g.IMPOWER = function (b, a) {
                a = e.parseNumber(a);
                var c = g.IMREAL(b),
                    d = g.IMAGINARY(b);
                if (e.anyIsError(a, c, d)) return k.value;
                c = b.substring(b.length - 1);
                c = "i" === c || "j" === c ? c : "i";
                d = Math.pow(g.IMABS(b), a);
                b = g.IMARGUMENT(b);
                return g.COMPLEX(d * Math.cos(a * b), d * Math.sin(a * b), c)
            };
            g.IMPRODUCT = function () {
                for (var b = arguments[0], a = 1; a < arguments.length; a++) {
                    var c = g.IMREAL(b);
                    b = g.IMAGINARY(b);
                    var d = g.IMREAL(arguments[a]),
                        f = g.IMAGINARY(arguments[a]);
                    if (e.anyIsError(c, b, d, f)) return k.value;
                    b = g.COMPLEX(c * d - b * f, c * f + b * d)
                }
                return b
            };
            g.IMREAL = function (b) {
                if (void 0 === b || !0 === b || !1 === b) return k.value;
                if (0 === b || "0" === b || 0 <= "i +i 1i +1i -i -1i j +j 1j +1j -j -1j".split(" ").indexOf(b)) return 0;
                var a = b.indexOf("+"),
                    c = b.indexOf("-");
                0 === a && (a = b.indexOf("+", 1));
                0 === c && (c = b.indexOf("-", 1));
                var d = b.substring(b.length - 1, b.length);
                d = "i" === d || "j" === d;
                return 0 <= a || 0 <= c ? d ? 0 <= a ? isNaN(b.substring(0, a)) || isNaN(b.substring(a + 1, b.length - 1)) ? k.num : Number(b.substring(0,
                    a)) : isNaN(b.substring(0, c)) || isNaN(b.substring(c + 1, b.length - 1)) ? k.num : Number(b.substring(0, c)) : k.num : d ? isNaN(b.substring(0, b.length - 1)) ? k.num : 0 : isNaN(b) ? k.num : b
            };
            g.IMSEC = function (b) {
                if (!0 === b || !1 === b) return k.value;
                var a = g.IMREAL(b),
                    c = g.IMAGINARY(b);
                return e.anyIsError(a, c) ? k.value : g.IMDIV("1", g.IMCOS(b))
            };
            g.IMSECH = function (b) {
                var a = g.IMREAL(b),
                    c = g.IMAGINARY(b);
                return e.anyIsError(a, c) ? k.value : g.IMDIV("1", g.IMCOSH(b))
            };
            g.IMSIN = function (b) {
                var a = g.IMREAL(b),
                    c = g.IMAGINARY(b);
                if (e.anyIsError(a, c)) return k.value;
                b = b.substring(b.length - 1);
                return g.COMPLEX(Math.sin(a) * (Math.exp(c) + Math.exp(-c)) / 2, Math.cos(a) * (Math.exp(c) - Math.exp(-c)) / 2, "i" === b || "j" === b ? b : "i")
            };
            g.IMSINH = function (b) {
                var a = g.IMREAL(b),
                    c = g.IMAGINARY(b);
                if (e.anyIsError(a, c)) return k.value;
                b = b.substring(b.length - 1);
                return g.COMPLEX(Math.cos(c) * (Math.exp(a) - Math.exp(-a)) / 2, Math.sin(c) * (Math.exp(a) + Math.exp(-a)) / 2, "i" === b || "j" === b ? b : "i")
            };
            g.IMSQRT = function (b) {
                var a = g.IMREAL(b),
                    c = g.IMAGINARY(b);
                if (e.anyIsError(a, c)) return k.value;
                a = b.substring(b.length -
                    1);
                a = "i" === a || "j" === a ? a : "i";
                c = Math.sqrt(g.IMABS(b));
                b = g.IMARGUMENT(b);
                return g.COMPLEX(c * Math.cos(b / 2), c * Math.sin(b / 2), a)
            };
            g.IMCSC = function (b) {
                if (!0 === b || !1 === b) return k.value;
                var a = g.IMREAL(b),
                    c = g.IMAGINARY(b);
                return e.anyIsError(a, c) ? k.num : g.IMDIV("1", g.IMSIN(b))
            };
            g.IMCSCH = function (b) {
                if (!0 === b || !1 === b) return k.value;
                var a = g.IMREAL(b),
                    c = g.IMAGINARY(b);
                return e.anyIsError(a, c) ? k.num : g.IMDIV("1", g.IMSINH(b))
            };
            g.IMSUB = function (b, a) {
                var c = this.IMREAL(b),
                    d = this.IMAGINARY(b),
                    f = this.IMREAL(a),
                    h = this.IMAGINARY(a);
                if (e.anyIsError(c, d, f, h)) return k.value;
                b = b.substring(b.length - 1);
                a = a.substring(a.length - 1);
                var l = "i";
                "j" === b ? l = "j" : "j" === a && (l = "j");
                return this.COMPLEX(c - f, d - h, l)
            };
            g.IMSUM = function () {
                for (var b = e.flatten(arguments), a = b[0], c = 1; c < b.length; c++) {
                    var d = this.IMREAL(a);
                    a = this.IMAGINARY(a);
                    var f = this.IMREAL(b[c]),
                        h = this.IMAGINARY(b[c]);
                    if (e.anyIsError(d, a, f, h)) return k.value;
                    a = this.COMPLEX(d + f, a + h)
                }
                return a
            };
            g.IMTAN = function (b) {
                if (!0 === b || !1 === b) return k.value;
                var a = g.IMREAL(b),
                    c = g.IMAGINARY(b);
                return e.anyIsError(a,
                    c) ? k.value : this.IMDIV(this.IMSIN(b), this.IMCOS(b))
            };
            g.OCT2BIN = function (b, a) {
                if (!/^[0-7]{1,10}$/.test(b)) return k.num;
                var c = 10 === b.length && "7" === b.substring(0, 1) ? !0 : !1;
                b = c ? parseInt(b, 8) - 1073741824 : parseInt(b, 8);
                if (-512 > b || 511 < b) return k.num;
                if (c) return "1" + REPT("0", 9 - (512 + b).toString(2).length) + (512 + b).toString(2);
                c = b.toString(2);
                if ("undefined" === typeof a) return c;
                if (isNaN(a)) return k.value;
                if (0 > a) return k.num;
                a = Math.floor(a);
                return a >= c.length ? REPT("0", a - c.length) + c : k.num
            };
            g.OCT2DEC = function (b) {
                if (!/^[0-7]{1,10}$/.test(b)) return k.num;
                b = parseInt(b, 8);
                return 536870912 <= b ? b - 1073741824 : b
            };
            g.OCT2HEX = function (b, a) {
                if (!/^[0-7]{1,10}$/.test(b)) return k.num;
                b = parseInt(b, 8);
                if (536870912 <= b) return "ff" + (b + 3221225472).toString(16);
                b = b.toString(16);
                if (void 0 === a) return b;
                if (isNaN(a)) return k.value;
                if (0 > a) return k.num;
                a = Math.floor(a);
                return a >= b.length ? REPT("0", a - b.length) + b : k.num
            };
            return g
        }();
        y.financial = function () {
            function g(c) {
                return c && c.getTime && !isNaN(c.getTime())
            }

            function b(c) {
                return c instanceof Date ? c : new Date(c)
            }
            var a = {
                ACCRINT: function (c,
                    d, f, h, l, m, n) {
                    c = b(c);
                    d = b(d);
                    f = b(f);
                    return g(c) && g(d) && g(f) ? 0 >= h || 0 >= l || -1 === [1, 2, 4].indexOf(m) || -1 === [0, 1, 2, 3, 4].indexOf(n) || f <= c ? "#NUM!" : (l || 0) * h * YEARFRAC(c, f, n || 0) : "#VALUE!"
                },
                ACCRINTM: null,
                AMORDEGRC: null,
                AMORLINC: null,
                COUPDAYBS: null,
                COUPDAYS: null,
                COUPDAYSNC: null,
                COUPNCD: null,
                COUPNUM: null,
                COUPPCD: null,
                CUMIPMT: function (c, d, f, h, l, m) {
                    c = e.parseNumber(c);
                    d = e.parseNumber(d);
                    f = e.parseNumber(f);
                    if (e.anyIsError(c, d, f)) return k.value;
                    if (0 >= c || 0 >= d || 0 >= f || 1 > h || 1 > l || h > l || 0 !== m && 1 !== m) return k.num;
                    d = a.PMT(c, d,
                        f, 0, m);
                    var n = 0;
                    1 === h && 0 === m && (n = -f, h++);
                    for (; h <= l; h++) n = 1 === m ? n + (a.FV(c, h - 2, d, f, 1) - d) : n + a.FV(c, h - 1, d, f, 0);
                    return n * c
                },
                CUMPRINC: function (c, d, f, h, l, m) {
                    c = e.parseNumber(c);
                    d = e.parseNumber(d);
                    f = e.parseNumber(f);
                    if (e.anyIsError(c, d, f)) return k.value;
                    if (0 >= c || 0 >= d || 0 >= f || 1 > h || 1 > l || h > l || 0 !== m && 1 !== m) return k.num;
                    d = a.PMT(c, d, f, 0, m);
                    var n = 0;
                    1 === h && (n = 0 === m ? d + f * c : d, h++);
                    for (; h <= l; h++) n = 0 < m ? n + (d - (a.FV(c, h - 2, d, f, 1) - d) * c) : n + (d - a.FV(c, h - 1, d, f, 0) * c);
                    return n
                },
                DB: function (c, d, f, h, l) {
                    l = void 0 === l ? 12 : l;
                    c = e.parseNumber(c);
                    d = e.parseNumber(d);
                    f = e.parseNumber(f);
                    h = e.parseNumber(h);
                    l = e.parseNumber(l);
                    if (e.anyIsError(c, d, f, h, l)) return k.value;
                    if (0 > c || 0 > d || 0 > f || 0 > h || -1 === [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12].indexOf(l) || h > f) return k.num;
                    if (d >= c) return 0;
                    d = (1 - Math.pow(d / c, 1 / f)).toFixed(3);
                    for (var m = l = c * d * l / 12, n = 0, p = h === f ? f - 1 : h, q = 2; q <= p; q++) n = (c - m) * d, m += n;
                    return 1 === h ? l : h === f ? (c - m) * d : n
                },
                DDB: function (c, d, f, h, l) {
                    l = void 0 === l ? 2 : l;
                    c = e.parseNumber(c);
                    d = e.parseNumber(d);
                    f = e.parseNumber(f);
                    h = e.parseNumber(h);
                    l = e.parseNumber(l);
                    if (e.anyIsError(c,
                        d, f, h, l)) return k.value;
                    if (0 > c || 0 > d || 0 > f || 0 > h || 0 >= l || h > f) return k.num;
                    if (d >= c) return 0;
                    for (var m = 0, n = 0, p = 1; p <= h; p++) n = Math.min(l / f * (c - m), c - d - m), m += n;
                    return n
                },
                DISC: null,
                DOLLARDE: function (c, d) {
                    c = e.parseNumber(c);
                    d = e.parseNumber(d);
                    if (e.anyIsError(c, d)) return k.value;
                    if (0 > d) return k.num;
                    if (0 <= d && 1 > d) return k.div0;
                    d = parseInt(d, 10);
                    var f = parseInt(c, 10);
                    f += c % 1 * Math.pow(10, Math.ceil(Math.log(d) / Math.LN10)) / d;
                    c = Math.pow(10, Math.ceil(Math.log(d) / Math.LN2) + 1);
                    return f = Math.round(f * c) / c
                },
                DOLLARFR: function (c,
                    d) {
                    c = e.parseNumber(c);
                    d = e.parseNumber(d);
                    if (e.anyIsError(c, d)) return k.value;
                    if (0 > d) return k.num;
                    if (0 <= d && 1 > d) return k.div0;
                    d = parseInt(d, 10);
                    var f = parseInt(c, 10);
                    return f += c % 1 * Math.pow(10, -Math.ceil(Math.log(d) / Math.LN10)) * d
                },
                DURATION: null,
                EFFECT: function (c, d) {
                    c = e.parseNumber(c);
                    d = e.parseNumber(d);
                    if (e.anyIsError(c, d)) return k.value;
                    if (0 >= c || 1 > d) return k.num;
                    d = parseInt(d, 10);
                    return Math.pow(1 + c / d, d) - 1
                },
                FV: function (c, d, f, h, l) {
                    h = h || 0;
                    l = l || 0;
                    c = e.parseNumber(c);
                    d = e.parseNumber(d);
                    f = e.parseNumber(f);
                    h =
                        e.parseNumber(h);
                    l = e.parseNumber(l);
                    if (e.anyIsError(c, d, f, h, l)) return k.value;
                    0 === c ? c = h + f * d : (d = Math.pow(1 + c, d), c = 1 === l ? h * d + f * (1 + c) * (d - 1) / c : h * d + f * (d - 1) / c);
                    return -c
                },
                FVSCHEDULE: function (c, d) {
                    c = e.parseNumber(c);
                    d = e.parseNumberArray(e.flatten(d));
                    if (e.anyIsError(c, d)) return k.value;
                    for (var f = d.length, h = 0; h < f; h++) c *= 1 + d[h];
                    return c
                },
                INTRATE: null,
                IPMT: function (c, d, f, h, l, m) {
                    l = l || 0;
                    m = m || 0;
                    c = e.parseNumber(c);
                    d = e.parseNumber(d);
                    f = e.parseNumber(f);
                    h = e.parseNumber(h);
                    l = e.parseNumber(l);
                    m = e.parseNumber(m);
                    if (e.anyIsError(c,
                        d, f, h, l, m)) return k.value;
                    f = a.PMT(c, f, h, l, m);
                    return (1 === d ? 1 === m ? 0 : -h : 1 === m ? a.FV(c, d - 2, f, h, 1) - f : a.FV(c, d - 1, f, h, 0)) * c
                },
                IRR: function (c, d) {
                    d = d || 0;
                    c = e.parseNumberArray(e.flatten(c));
                    d = e.parseNumber(d);
                    if (e.anyIsError(c, d)) return k.value;
                    for (var f = function (q, t, u) {
                        u += 1;
                        for (var r = q[0], v = 1; v < q.length; v++) r += q[v] / Math.pow(u, (t[v] - t[0]) / 365);
                        return r
                    }, h = function (q, t, u) {
                        u += 1;
                        for (var r = 0, v = 1; v < q.length; v++) {
                            var C = (t[v] - t[0]) / 365;
                            r -= C * q[v] / Math.pow(u, C + 1)
                        }
                        return r
                    }, l = [], m = !1, n = !1, p = 0; p < c.length; p++) l[p] = 0 === p ?
                        0 : l[p - 1] + 365, 0 < c[p] && (m = !0), 0 > c[p] && (n = !0);
                    if (!m || !n) return k.num;
                    d = void 0 === d ? .1 : d;
                    m = !0;
                    do p = f(c, l, d), m = d - p / h(c, l, d), n = Math.abs(m - d), d = m, m = 1E-10 < n && 1E-10 < Math.abs(p); while (m);
                    return d
                },
                ISPMT: function (c, d, f, h) {
                    c = e.parseNumber(c);
                    d = e.parseNumber(d);
                    f = e.parseNumber(f);
                    h = e.parseNumber(h);
                    return e.anyIsError(c, d, f, h) ? k.value : h * c * (d / f - 1)
                },
                MDURATION: null,
                MIRR: function (c, d, f) {
                    c = e.parseNumberArray(e.flatten(c));
                    d = e.parseNumber(d);
                    f = e.parseNumber(f);
                    if (e.anyIsError(c, d, f)) return k.value;
                    for (var h = c.length,
                        l = [], m = [], n = 0; n < h; n++) 0 > c[n] ? l.push(c[n]) : m.push(c[n]);
                    c = -a.NPV(f, m) * Math.pow(1 + f, h - 1);
                    d = a.NPV(d, l) * (1 + d);
                    return Math.pow(c / d, 1 / (h - 1)) - 1
                },
                NOMINAL: function (c, d) {
                    c = e.parseNumber(c);
                    d = e.parseNumber(d);
                    if (e.anyIsError(c, d)) return k.value;
                    if (0 >= c || 1 > d) return k.num;
                    d = parseInt(d, 10);
                    return (Math.pow(c + 1, 1 / d) - 1) * d
                },
                NPER: function (c, d, f, h, l) {
                    l = void 0 === l ? 0 : l;
                    h = void 0 === h ? 0 : h;
                    c = e.parseNumber(c);
                    d = e.parseNumber(d);
                    f = e.parseNumber(f);
                    h = e.parseNumber(h);
                    l = e.parseNumber(l);
                    return e.anyIsError(c, d, f, h, l) ? k.value :
                        Math.log((d * (1 + c * l) - h * c) / (f * c + d * (1 + c * l))) / Math.log(1 + c)
                },
                NPV: function () {
                    var c = e.parseNumberArray(e.flatten(arguments));
                    if (c instanceof Error) return c;
                    for (var d = c[0], f = 0, h = 1; h < c.length; h++) f += c[h] / Math.pow(1 + d, h);
                    return f
                },
                ODDFPRICE: null,
                ODDFYIELD: null,
                ODDLPRICE: null,
                ODDLYIELD: null,
                PDURATION: function (c, d, f) {
                    c = e.parseNumber(c);
                    d = e.parseNumber(d);
                    f = e.parseNumber(f);
                    return e.anyIsError(c, d, f) ? k.value : 0 >= c ? k.num : (Math.log(f) - Math.log(d)) / Math.log(1 + c)
                },
                PMT: function (c, d, f, h, l) {
                    h = h || 0;
                    l = l || 0;
                    c = e.parseNumber(c);
                    d = e.parseNumber(d);
                    f = e.parseNumber(f);
                    h = e.parseNumber(h);
                    l = e.parseNumber(l);
                    if (e.anyIsError(c, d, f, h, l)) return k.value;
                    0 === c ? c = (f + h) / d : (d = Math.pow(1 + c, d), c = 1 === l ? (h * c / (d - 1) + f * c / (1 - 1 / d)) / (1 + c) : h * c / (d - 1) + f * c / (1 - 1 / d));
                    return -c
                },
                PPMT: function (c, d, f, h, l, m) {
                    l = l || 0;
                    m = m || 0;
                    c = e.parseNumber(c);
                    f = e.parseNumber(f);
                    h = e.parseNumber(h);
                    l = e.parseNumber(l);
                    m = e.parseNumber(m);
                    return e.anyIsError(c, f, h, l, m) ? k.value : a.PMT(c, f, h, l, m) - a.IPMT(c, d, f, h, l, m)
                },
                PRICE: null,
                PRICEDISC: null,
                PRICEMAT: null,
                PV: function (c, d, f, h, l) {
                    h =
                        h || 0;
                    l = l || 0;
                    c = e.parseNumber(c);
                    d = e.parseNumber(d);
                    f = e.parseNumber(f);
                    h = e.parseNumber(h);
                    l = e.parseNumber(l);
                    return e.anyIsError(c, d, f, h, l) ? k.value : 0 === c ? -f * d - h : ((1 - Math.pow(1 + c, d)) / c * f * (1 + c * l) - h) / Math.pow(1 + c, d)
                },
                RATE: function (c, d, f, h, l, m) {
                    m = void 0 === m ? .01 : m;
                    h = void 0 === h ? 0 : h;
                    l = void 0 === l ? 0 : l;
                    c = e.parseNumber(c);
                    d = e.parseNumber(d);
                    f = e.parseNumber(f);
                    h = e.parseNumber(h);
                    l = e.parseNumber(l);
                    m = e.parseNumber(m);
                    if (e.anyIsError(c, d, f, h, l, m)) return k.value;
                    for (var n = 0, p = !1; 100 > n && !p;) {
                        var q = Math.pow(m + 1, c),
                            t = Math.pow(m + 1, c - 1);
                        q = m - (h + q * f + d * (q - 1) * (m * l + 1) / m) / (c * t * f - d * (q - 1) * (m * l + 1) / Math.pow(m, 2) + (c * d * t * (m * l + 1) / m + d * (q - 1) * l / m));
                        1E-6 > Math.abs(q - m) && (p = !0);
                        n++;
                        m = q
                    }
                    return p ? m : Number.NaN + m
                },
                RECEIVED: null,
                RRI: function (c, d, f) {
                    c = e.parseNumber(c);
                    d = e.parseNumber(d);
                    f = e.parseNumber(f);
                    return e.anyIsError(c, d, f) ? k.value : 0 === c || 0 === d ? k.num : Math.pow(f / d, 1 / c) - 1
                },
                SLN: function (c, d, f) {
                    c = e.parseNumber(c);
                    d = e.parseNumber(d);
                    f = e.parseNumber(f);
                    return e.anyIsError(c, d, f) ? k.value : 0 === f ? k.num : (c - d) / f
                },
                SYD: function (c, d, f, h) {
                    c =
                        e.parseNumber(c);
                    d = e.parseNumber(d);
                    f = e.parseNumber(f);
                    h = e.parseNumber(h);
                    if (e.anyIsError(c, d, f, h)) return k.value;
                    if (0 === f || 1 > h || h > f) return k.num;
                    h = parseInt(h, 10);
                    return (c - d) * (f - h + 1) * 2 / (f * (f + 1))
                },
                TBILLEQ: function (c, d, f) {
                    c = e.parseDate(c);
                    d = e.parseDate(d);
                    f = e.parseNumber(f);
                    return e.anyIsError(c, d, f) ? k.value : 0 >= f || c > d || 31536E6 < d - c ? k.num : 365 * f / (360 - f * DAYS360(c, d, !1))
                },
                TBILLPRICE: function (c, d, f) {
                    c = e.parseDate(c);
                    d = e.parseDate(d);
                    f = e.parseNumber(f);
                    return e.anyIsError(c, d, f) ? k.value : 0 >= f || c > d || 31536E6 <
                        d - c ? k.num : 100 * (1 - f * DAYS360(c, d, !1) / 360)
                },
                TBILLYIELD: function (c, d, f) {
                    c = e.parseDate(c);
                    d = e.parseDate(d);
                    f = e.parseNumber(f);
                    return e.anyIsError(c, d, f) ? k.value : 0 >= f || c > d || 31536E6 < d - c ? k.num : 360 * (100 - f) / (f * DAYS360(c, d, !1))
                },
                VDB: null,
                XIRR: function (c, d, f) {
                    c = e.parseNumberArray(e.flatten(c));
                    d = e.parseDateArray(e.flatten(d));
                    f = e.parseNumber(f);
                    if (e.anyIsError(c, d, f)) return k.value;
                    for (var h = function (q, t, u) {
                        u += 1;
                        for (var r = q[0], v = 1; v < q.length; v++) r += q[v] / Math.pow(u, DAYS(t[v], t[0]) / 365);
                        return r
                    }, l = function (q,
                        t, u) {
                        u += 1;
                        for (var r = 0, v = 1; v < q.length; v++) {
                            var C = DAYS(t[v], t[0]) / 365;
                            r -= C * q[v] / Math.pow(u, C + 1)
                        }
                        return r
                    }, m = !1, n = !1, p = 0; p < c.length; p++) 0 < c[p] && (m = !0), 0 > c[p] && (n = !0);
                    if (!m || !n) return k.num;
                    f = f || .1;
                    m = !0;
                    do p = h(c, d, f), m = f - p / l(c, d, f), n = Math.abs(m - f), f = m, m = 1E-10 < n && 1E-10 < Math.abs(p); while (m);
                    return f
                },
                XNPV: function (c, d, f) {
                    c = e.parseNumber(c);
                    d = e.parseNumberArray(e.flatten(d));
                    f = e.parseDateArray(e.flatten(f));
                    if (e.anyIsError(c, d, f)) return k.value;
                    for (var h = 0, l = 0; l < d.length; l++) h += d[l] / Math.pow(1 + c, DAYS(f[l],
                        f[0]) / 365);
                    return h
                },
                YIELD: null,
                YIELDDISC: null,
                YIELDMAT: null
            };
            return a
        }();
        y.information = function () {
            var g = {
                CELL: null,
                ERROR: {}
            };
            g.ERROR.TYPE = function (b) {
                switch (b) {
                    case k.nil:
                        return 1;
                    case k.div0:
                        return 2;
                    case k.value:
                        return 3;
                    case k.ref:
                        return 4;
                    case k.name:
                        return 5;
                    case k.num:
                        return 6;
                    case k.na:
                        return 7;
                    case k.data:
                        return 8
                }
                return k.na
            };
            g.INFO = null;
            g.ISBLANK = function (b) {
                return null === b || void 0 === b || "" === b
            };
            g.ISBINARY = function (b) {
                return /^[01]{1,10}$/.test(b)
            };
            g.ISERR = function (b) {
                return 0 <= [k.value,
                k.ref, k.div0, k.num, k.name, k.nil
                ].indexOf(b) || "number" === typeof b && (isNaN(b) || !isFinite(b))
            };
            g.ISERROR = function (b) {
                return g.ISERR(b) || b === k.na
            };
            g.ISEVEN = function (b) {
                return Math.floor(Math.abs(b)) & 1 ? !1 : !0
            };
            g.ISFORMULA = null;
            g.ISLOGICAL = function (b) {
                return !0 === b || !1 === b
            };
            g.ISNA = function (b) {
                return b === k.na
            };
            g.ISNONTEXT = function (b) {
                return "string" !== typeof b
            };
            g.ISNUMBER = function (b) {
                return "number" === typeof b && !isNaN(b) && isFinite(b)
            };
            g.ISODD = function (b) {
                return Math.floor(Math.abs(b)) & 1 ? !0 : !1
            };
            g.ISREF = null;
            g.ISTEXT = function (b) {
                return "string" === typeof b
            };
            g.N = function (b) {
                return this.ISNUMBER(b) ? b : b instanceof Date ? b.getTime() : !0 === b ? 1 : !1 === b ? 0 : this.ISERROR(b) ? b : 0
            };
            g.NA = function () {
                return k.na
            };
            g.SHEET = null;
            g.SHEETS = null;
            g.TYPE = function (b) {
                if (this.ISNUMBER(b)) return 1;
                if (this.ISTEXT(b)) return 2;
                if (this.ISLOGICAL(b)) return 4;
                if (this.ISERROR(b)) return 16;
                if (Array.isArray(b)) return 64
            };
            return g
        }();
        y.logical = function () {
            return {
                AND: function () {
                    for (var g = e.flatten(arguments), b = !0, a = 0; a < g.length; a++) g[a] || (b = !1);
                    return b
                },
                CHOOSE: function () {
                    if (2 > arguments.length) return k.na;
                    var g = arguments[0];
                    return 1 > g || 254 < g || arguments.length < g + 1 ? k.value : arguments[g]
                },
                FALSE: function () {
                    return !1
                },
                IF: function (g, b, a) {
                    return g ? b : a
                },
                IFERROR: function (g, b) {
                    return ISERROR(g) ? b : g
                },
                IFNA: function (g, b) {
                    return g === k.na ? b : g
                },
                NOT: function (g) {
                    return !g
                },
                OR: function () {
                    for (var g = e.flatten(arguments), b = !1, a = 0; a < g.length; a++) g[a] && (b = !0);
                    return b
                },
                TRUE: function () {
                    return !0
                },
                XOR: function () {
                    for (var g = e.flatten(arguments), b = 0, a = 0; a < g.length; a++) g[a] &&
                        b++;
                    return Math.floor(Math.abs(b)) & 1 ? !0 : !1
                },
                SWITCH: function () {
                    if (0 < arguments.length) {
                        var g = arguments[0],
                            b = arguments.length - 1,
                            a = Math.floor(b / 2),
                            c = !1;
                        b = 0 === b % 2 ? null : arguments[arguments.length - 1];
                        if (a)
                            for (var d = 0; d < a; d++)
                                if (g === arguments[2 * d + 1]) {
                                    var f = arguments[2 * d + 2];
                                    c = !0;
                                    break
                                } !c && b && (f = b)
                    }
                    return f
                }
            }
        }();
        y.math = function () {
            var g = {
                ABS: function (a) {
                    a = e.parseNumber(a);
                    return a instanceof Error ? a : Math.abs(e.parseNumber(a))
                },
                ACOS: function (a) {
                    a = e.parseNumber(a);
                    return a instanceof Error ? a : Math.acos(a)
                },
                ACOSH: function (a) {
                    a =
                        e.parseNumber(a);
                    return a instanceof Error ? a : Math.log(a + Math.sqrt(a * a - 1))
                },
                ACOT: function (a) {
                    a = e.parseNumber(a);
                    return a instanceof Error ? a : Math.atan(1 / a)
                },
                ACOTH: function (a) {
                    a = e.parseNumber(a);
                    return a instanceof Error ? a : .5 * Math.log((a + 1) / (a - 1))
                },
                AGGREGATE: null,
                ARABIC: function (a) {
                    if (!/^M*(?:D?C{0,3}|C[MD])(?:L?X{0,3}|X[CL])(?:V?I{0,3}|I[XV])$/.test(a)) return k.value;
                    var c = 0;
                    a.replace(/[MDLV]|C[MD]?|X[CL]?|I[XV]?/g, function (d) {
                        c += {
                            M: 1E3,
                            CM: 900,
                            D: 500,
                            CD: 400,
                            C: 100,
                            XC: 90,
                            L: 50,
                            XL: 40,
                            X: 10,
                            IX: 9,
                            V: 5,
                            IV: 4,
                            I: 1
                        }[d]
                    });
                    return c
                },
                ASIN: function (a) {
                    a = e.parseNumber(a);
                    return a instanceof Error ? a : Math.asin(a)
                },
                ASINH: function (a) {
                    a = e.parseNumber(a);
                    return a instanceof Error ? a : Math.log(a + Math.sqrt(a * a + 1))
                },
                ATAN: function (a) {
                    a = e.parseNumber(a);
                    return a instanceof Error ? a : Math.atan(a)
                },
                ATAN2: function (a, c) {
                    a = e.parseNumber(a);
                    c = e.parseNumber(c);
                    return e.anyIsError(a, c) ? k.value : Math.atan2(a, c)
                },
                ATANH: function (a) {
                    a = e.parseNumber(a);
                    return a instanceof Error ? a : Math.log((1 + a) / (1 - a)) / 2
                },
                BASE: function (a, c, d) {
                    d = d || 0;
                    a = e.parseNumber(a);
                    c = e.parseNumber(c);
                    d = e.parseNumber(d);
                    if (e.anyIsError(a, c, d)) return k.value;
                    d = void 0 === d ? 0 : d;
                    a = a.toString(c);
                    return Array(Math.max(d + 1 - a.length, 0)).join("0") + a
                },
                CEILING: function (a, c, d) {
                    c = void 0 === c ? 1 : c;
                    d = void 0 === d ? 0 : d;
                    a = e.parseNumber(a);
                    c = e.parseNumber(c);
                    d = e.parseNumber(d);
                    if (e.anyIsError(a, c, d)) return k.value;
                    if (0 === c) return 0;
                    c = Math.abs(c);
                    return 0 <= a ? Math.ceil(a / c) * c : 0 === d ? -1 * Math.floor(Math.abs(a) / c) * c : -1 * Math.ceil(Math.abs(a) / c) * c
                }
            };
            g.CEILING.MATH = g.CEILING;
            g.CEILING.PRECISE = g.CEILING;
            g.COMBIN =
                function (a, c) {
                    a = e.parseNumber(a);
                    c = e.parseNumber(c);
                    return e.anyIsError(a, c) ? k.value : g.FACT(a) / (g.FACT(c) * g.FACT(a - c))
                };
            g.COMBINA = function (a, c) {
                a = e.parseNumber(a);
                c = e.parseNumber(c);
                return e.anyIsError(a, c) ? k.value : 0 === a && 0 === c ? 1 : g.COMBIN(a + c - 1, a - 1)
            };
            g.COS = function (a) {
                a = e.parseNumber(a);
                return a instanceof Error ? a : Math.cos(a)
            };
            g.COSH = function (a) {
                a = e.parseNumber(a);
                return a instanceof Error ? a : (Math.exp(a) + Math.exp(-a)) / 2
            };
            g.COT = function (a) {
                a = e.parseNumber(a);
                return a instanceof Error ? a : 1 / Math.tan(a)
            };
            g.COTH = function (a) {
                a = e.parseNumber(a);
                if (a instanceof Error) return a;
                a = Math.exp(2 * a);
                return (a + 1) / (a - 1)
            };
            g.CSC = function (a) {
                a = e.parseNumber(a);
                return a instanceof Error ? a : 1 / Math.sin(a)
            };
            g.CSCH = function (a) {
                a = e.parseNumber(a);
                return a instanceof Error ? a : 2 / (Math.exp(a) - Math.exp(-a))
            };
            g.DECIMAL = function (a, c) {
                return 1 > arguments.length ? k.value : parseInt(a, c)
            };
            g.DEGREES = function (a) {
                a = e.parseNumber(a);
                return a instanceof Error ? a : 180 * a / Math.PI
            };
            g.EVEN = function (a) {
                a = e.parseNumber(a);
                return a instanceof Error ?
                    a : g.CEILING(a, -2, -1)
            };
            g.EXP = Math.exp;
            var b = [];
            g.FACT = function (a) {
                a = e.parseNumber(a);
                if (a instanceof Error) return a;
                a = Math.floor(a);
                if (0 === a || 1 === a) return 1;
                0 < b[a] || (b[a] = g.FACT(a - 1) * a);
                return b[a]
            };
            g.FACTDOUBLE = function (a) {
                a = e.parseNumber(a);
                if (a instanceof Error) return a;
                a = Math.floor(a);
                return 0 >= a ? 1 : a * g.FACTDOUBLE(a - 2)
            };
            g.FLOOR = function (a, c, d) {
                c = void 0 === c ? 1 : c;
                d = void 0 === d ? 0 : d;
                a = e.parseNumber(a);
                c = e.parseNumber(c);
                d = e.parseNumber(d);
                if (e.anyIsError(a, c, d)) return k.value;
                if (0 === c) return 0;
                c = Math.abs(c);
                return 0 <= a ? Math.floor(a / c) * c : 0 === d ? -1 * Math.ceil(Math.abs(a) / c) * c : -1 * Math.floor(Math.abs(a) / c) * c
            };
            g.FLOOR.MATH = g.FLOOR;
            g.GCD = null;
            g.INT = function (a) {
                a = e.parseNumber(a);
                return a instanceof Error ? a : Math.floor(a)
            };
            g.LCM = function () {
                var a = e.parseNumberArray(e.flatten(arguments));
                if (a instanceof Error) return a;
                for (var c, d, f, h = 1; void 0 !== (f = a.pop());)
                    for (; 1 < f;) {
                        if (f % 2) {
                            c = 3;
                            for (d = Math.floor(Math.sqrt(f)); c <= d && f % c; c += 2);
                            d = c <= d ? c : f
                        } else d = 2;
                        f /= d;
                        h *= d;
                        for (c = a.length; c; 0 === a[--c] % d && 1 === (a[c] /= d) && a.splice(c,
                            1));
                    }
                return h
            };
            g.LN = function (a) {
                a = e.parseNumber(a);
                return a instanceof Error ? a : Math.log(a)
            };
            g.LOG = function (a, c) {
                a = e.parseNumber(a);
                c = void 0 === c ? 10 : e.parseNumber(c);
                return e.anyIsError(a, c) ? k.value : Math.log(a) / Math.log(c)
            };
            g.LOG10 = function (a) {
                a = e.parseNumber(a);
                return a instanceof Error ? a : Math.log(a) / Math.log(10)
            };
            g.MDETERM = null;
            g.MINVERSE = null;
            g.MMULT = null;
            g.MOD = function (a, c) {
                a = e.parseNumber(a);
                c = e.parseNumber(c);
                if (e.anyIsError(a, c)) return k.value;
                if (0 === c) return k.div0;
                a = Math.abs(a % c);
                return 0 <
                    c ? a : -a
            };
            g.MROUND = function (a, c) {
                a = e.parseNumber(a);
                c = e.parseNumber(c);
                return e.anyIsError(a, c) ? k.value : 0 > a * c ? k.num : Math.round(a / c) * c
            };
            g.MULTINOMIAL = function () {
                var a = e.parseNumberArray(e.flatten(arguments));
                if (a instanceof Error) return a;
                for (var c = 0, d = 1, f = 0; f < a.length; f++) c += a[f], d *= g.FACT(a[f]);
                return g.FACT(c) / d
            };
            g.MUNIT = null;
            g.ODD = function (a) {
                a = e.parseNumber(a);
                if (a instanceof Error) return a;
                var c = Math.ceil(Math.abs(a));
                c = c & 1 ? c : c + 1;
                return 0 < a ? c : -c
            };
            g.PI = function () {
                return Math.PI
            };
            g.POWER = function (a,
                c) {
                a = e.parseNumber(a);
                c = e.parseNumber(c);
                if (e.anyIsError(a, c)) return k.value;
                a = Math.pow(a, c);
                return isNaN(a) ? k.num : a
            };
            g.PRODUCT = function () {
                var a = e.parseNumberArray(e.flatten(arguments));
                if (a instanceof Error) return a;
                for (var c = 1, d = 0; d < a.length; d++) c *= a[d];
                return c
            };
            g.QUOTIENT = function (a, c) {
                a = e.parseNumber(a);
                c = e.parseNumber(c);
                return e.anyIsError(a, c) ? k.value : parseInt(a / c, 10)
            };
            g.RADIANS = function (a) {
                a = e.parseNumber(a);
                return a instanceof Error ? a : a * Math.PI / 180
            };
            g.RAND = function () {
                return Math.random()
            };
            g.RANDBETWEEN = function (a, c) {
                a = e.parseNumber(a);
                c = e.parseNumber(c);
                return e.anyIsError(a, c) ? k.value : a + Math.ceil((c - a + 1) * Math.random()) - 1
            };
            g.ROMAN = null;
            g.ROUND = function (a, c) {
                a = e.parseNumber(a);
                c = e.parseNumber(c);
                return e.anyIsError(a, c) ? k.value : Math.round(a * Math.pow(10, c)) / Math.pow(10, c)
            };
            g.ROUNDDOWN = function (a, c) {
                a = e.parseNumber(a);
                c = e.parseNumber(c);
                return e.anyIsError(a, c) ? k.value : (0 < a ? 1 : -1) * Math.floor(Math.abs(a) * Math.pow(10, c)) / Math.pow(10, c)
            };
            g.ROUNDUP = function (a, c) {
                a = e.parseNumber(a);
                c = e.parseNumber(c);
                return e.anyIsError(a, c) ? k.value : (0 < a ? 1 : -1) * Math.ceil(Math.abs(a) * Math.pow(10, c)) / Math.pow(10, c)
            };
            g.SEC = function (a) {
                a = e.parseNumber(a);
                return a instanceof Error ? a : 1 / Math.cos(a)
            };
            g.SECH = function (a) {
                a = e.parseNumber(a);
                return a instanceof Error ? a : 2 / (Math.exp(a) + Math.exp(-a))
            };
            g.SERIESSUM = function (a, c, d, f) {
                a = e.parseNumber(a);
                c = e.parseNumber(c);
                d = e.parseNumber(d);
                f = e.parseNumberArray(f);
                if (e.anyIsError(a, c, d, f)) return k.value;
                for (var h = f[0] * Math.pow(a, c), l = 1; l < f.length; l++) h += f[l] * Math.pow(a, c + l * d);
                return h
            };
            g.SIGN = function (a) {
                a = e.parseNumber(a);
                return a instanceof Error ? a : 0 > a ? -1 : 0 === a ? 0 : 1
            };
            g.SIN = function (a) {
                a = e.parseNumber(a);
                return a instanceof Error ? a : Math.sin(a)
            };
            g.SINH = function (a) {
                a = e.parseNumber(a);
                return a instanceof Error ? a : (Math.exp(a) - Math.exp(-a)) / 2
            };
            g.SQRT = function (a) {
                a = e.parseNumber(a);
                return a instanceof Error ? a : 0 > a ? k.num : Math.sqrt(a)
            };
            g.SQRTPI = function (a) {
                a = e.parseNumber(a);
                return a instanceof Error ? a : Math.sqrt(a * Math.PI)
            };
            g.SUBTOTAL = null;
            g.ADD = function (a, c) {
                if (2 !== arguments.length) return k.na;
                a = e.parseNumber(a);
                c = e.parseNumber(c);
                return e.anyIsError(a, c) ? k.value : a + c
            };
            g.MINUS = function (a, c) {
                if (2 !== arguments.length) return k.na;
                a = e.parseNumber(a);
                c = e.parseNumber(c);
                return e.anyIsError(a, c) ? k.value : a - c
            };
            g.DIVIDE = function (a, c) {
                if (2 !== arguments.length) return k.na;
                a = e.parseNumber(a);
                c = e.parseNumber(c);
                return e.anyIsError(a, c) ? k.value : 0 === c ? k.div0 : a / c
            };
            g.MULTIPLY = function (a, c) {
                if (2 !== arguments.length) return k.na;
                a = e.parseNumber(a);
                c = e.parseNumber(c);
                return e.anyIsError(a, c) ? k.value : a * c
            };
            g.GTE =
                function (a, c) {
                    if (2 !== arguments.length) return k.na;
                    a = e.parseNumber(a);
                    c = e.parseNumber(c);
                    return e.anyIsError(a, c) ? k.error : a >= c
                };
            g.LT = function (a, c) {
                if (2 !== arguments.length) return k.na;
                a = e.parseNumber(a);
                c = e.parseNumber(c);
                return e.anyIsError(a, c) ? k.error : a < c
            };
            g.LTE = function (a, c) {
                if (2 !== arguments.length) return k.na;
                a = e.parseNumber(a);
                c = e.parseNumber(c);
                return e.anyIsError(a, c) ? k.error : a <= c
            };
            g.EQ = function (a, c) {
                return 2 !== arguments.length ? k.na : a === c
            };
            g.NE = function (a, c) {
                return 2 !== arguments.length ? k.na :
                    a !== c
            };
            g.POW = function (a, c) {
                if (2 !== arguments.length) return k.na;
                a = e.parseNumber(a);
                c = e.parseNumber(c);
                return e.anyIsError(a, c) ? k.error : g.POWER(a, c)
            };
            g.SUM = function () {
                for (var a = 0, c = Object.keys(arguments), d = 0; d < c.length; ++d) {
                    var f = arguments[c[d]];
                    "number" === typeof f ? a += f : "string" === typeof f ? (f = parseFloat(f), !isNaN(f) && (a += f)) : Array.isArray(f) && (a += g.SUM.apply(null, f))
                }
                return a
            };
            g.SUMIF = function () {
                var a = e.argsToArray(arguments),
                    c = a.pop();
                a = e.parseNumberArray(e.flatten(a));
                if (a instanceof Error) return a;
                for (var d = 0, f = 0; f < a.length; f++) d += eval(a[f] + c) ? a[f] : 0;
                return d
            };
            g.SUMIFS = function () {
                var a = e.argsToArray(arguments),
                    c = e.parseNumberArray(e.flatten(a.shift()));
                if (c instanceof Error) return c;
                for (var d = c.length, f = a.length, h = 0, l = 0; l < d; l++) {
                    for (var m = c[l], n = "", p = 0; p < f; p += 2) n = isNaN(a[p][l]) ? n + ('"' + a[p][l] + '"' + a[p + 1]) : n + (a[p][l] + a[p + 1]), p !== f - 1 && (n += " && ");
                    n = n.slice(0, -4);
                    eval(n) && (h += m)
                }
                return h
            };
            g.SUMPRODUCT = null;
            g.SUMSQ = function () {
                var a = e.parseNumberArray(e.flatten(arguments));
                if (a instanceof Error) return a;
                for (var c = 0, d = a.length, f = 0; f < d; f++) c += ISNUMBER(a[f]) ? a[f] * a[f] : 0;
                return c
            };
            g.SUMX2MY2 = function (a, c) {
                a = e.parseNumberArray(e.flatten(a));
                c = e.parseNumberArray(e.flatten(c));
                if (e.anyIsError(a, c)) return k.value;
                for (var d = 0, f = 0; f < a.length; f++) d += a[f] * a[f] - c[f] * c[f];
                return d
            };
            g.SUMX2PY2 = function (a, c) {
                a = e.parseNumberArray(e.flatten(a));
                c = e.parseNumberArray(e.flatten(c));
                if (e.anyIsError(a, c)) return k.value;
                var d = 0;
                a = e.parseNumberArray(e.flatten(a));
                c = e.parseNumberArray(e.flatten(c));
                for (var f = 0; f < a.length; f++) d +=
                    a[f] * a[f] + c[f] * c[f];
                return d
            };
            g.SUMXMY2 = function (a, c) {
                a = e.parseNumberArray(e.flatten(a));
                c = e.parseNumberArray(e.flatten(c));
                if (e.anyIsError(a, c)) return k.value;
                var d = 0;
                a = e.flatten(a);
                c = e.flatten(c);
                for (var f = 0; f < a.length; f++) d += Math.pow(a[f] - c[f], 2);
                return d
            };
            g.TAN = function (a) {
                a = e.parseNumber(a);
                return a instanceof Error ? a : Math.tan(a)
            };
            g.TANH = function (a) {
                a = e.parseNumber(a);
                if (a instanceof Error) return a;
                a = Math.exp(2 * a);
                return (a - 1) / (a + 1)
            };
            g.TRUNC = function (a, c) {
                c = void 0 === c ? 0 : c;
                a = e.parseNumber(a);
                c =
                    e.parseNumber(c);
                return e.anyIsError(a, c) ? k.value : (0 < a ? 1 : -1) * Math.floor(Math.abs(a) * Math.pow(10, c)) / Math.pow(10, c)
            };
            return g
        }();
        y.misc = function () {
            var g = {
                UNIQUE: function () {
                    for (var b = [], a = 0; a < arguments.length; ++a) {
                        for (var c = !1, d = arguments[a], f = 0; f < b.length && !(c = b[f] === d); ++f);
                        c || b.push(d)
                    }
                    return b
                }
            };
            g.FLATTEN = e.flatten;
            g.ARGS2ARRAY = function () {
                return Array.prototype.slice.call(arguments, 0)
            };
            g.REFERENCE = function (b, a) {
                try {
                    var c = a.split(".");
                    for (a = 0; a < c.length; ++a) {
                        var d = c[a];
                        if ("]" === d[d.length - 1]) {
                            var f =
                                d.indexOf("["),
                                h = d.substring(f + 1, d.length - 1);
                            b = b[d.substring(0, f)][h]
                        } else b = b[d]
                    }
                    return b
                } catch (l) { }
            };
            g.JOIN = function (b, a) {
                return b.join(a)
            };
            g.NUMBERS = function () {
                return e.flatten(arguments).filter(function (b) {
                    return "number" === typeof b
                })
            };
            g.NUMERAL = null;
            return g
        }();
        y.text = function () {
            var g = {
                ASC: null,
                BAHTTEXT: null,
                CHAR: function (b) {
                    b = e.parseNumber(b);
                    return b instanceof Error ? b : String.fromCharCode(b)
                },
                CLEAN: function (b) {
                    return (b || "").replace(/[\0-\x1F]/g, "")
                },
                CODE: function (b) {
                    return (b || "").charCodeAt(0)
                },
                CONCATENATE: function () {
                    for (var b = e.flatten(arguments), a; - 1 < (a = b.indexOf(!0));) b[a] = "TRUE";
                    for (; - 1 < (a = b.indexOf(!1));) b[a] = "FALSE";
                    return b.join("")
                },
                DBCS: null,
                DOLLAR: null,
                EXACT: function (b, a) {
                    return b === a
                },
                FIND: function (b, a, c) {
                    return a ? a.indexOf(b, (void 0 === c ? 0 : c) - 1) + 1 : null
                },
                FIXED: null,
                HTML2TEXT: function (b) {
                    var a = "";
                    b && (b instanceof Array ? b.forEach(function (c) {
                        "" !== a && (a += "\n");
                        a += c.replace(/<(?:.|\n)*?>/gm, "")
                    }) : a = b.replace(/<(?:.|\n)*?>/gm, ""));
                    return a
                },
                LEFT: function (b, a) {
                    a = e.parseNumber(void 0 ===
                        a ? 1 : a);
                    return a instanceof Error || "string" !== typeof b ? k.value : b ? b.substring(0, a) : null
                },
                LEN: function (b) {
                    return 0 === arguments.length ? k.error : "string" === typeof b ? b ? b.length : 0 : b.length ? b.length : k.value
                },
                LOWER: function (b) {
                    return "string" !== typeof b ? k.value : b ? b.toLowerCase() : b
                },
                MID: function (b, a, c) {
                    a = e.parseNumber(a);
                    c = e.parseNumber(c);
                    if (e.anyIsError(a, c) || "string" !== typeof b) return c;
                    --a;
                    return b.substring(a, a + c)
                },
                NUMBERVALUE: null,
                PRONETIC: null,
                PROPER: function (b) {
                    if (void 0 === b || 0 === b.length) return k.value;
                    !0 === b && (b = "TRUE");
                    !1 === b && (b = "FALSE");
                    if (isNaN(b) && "number" === typeof b) return k.value;
                    "number" === typeof b && (b = "" + b);
                    return b.replace(/\w\S*/g, function (a) {
                        return a.charAt(0).toUpperCase() + a.substr(1).toLowerCase()
                    })
                },
                REGEXEXTRACT: function (b, a) {
                    return (b = b.match(new RegExp(a))) ? b[1 < b.length ? b.length - 1 : 0] : null
                },
                REGEXMATCH: function (b, a, c) {
                    b = b.match(new RegExp(a));
                    return c ? b : !!b
                },
                REGEXREPLACE: function (b, a, c) {
                    return b.replace(new RegExp(a), c)
                },
                REPLACE: function (b, a, c, d) {
                    a = e.parseNumber(a);
                    c = e.parseNumber(c);
                    return e.anyIsError(a, c) || "string" !== typeof b || "string" !== typeof d ? k.value : b.substr(0, a - 1) + d + b.substr(a - 1 + c)
                },
                REPT: function (b, a) {
                    a = e.parseNumber(a);
                    return a instanceof Error ? a : Array(a + 1).join(b)
                },
                RIGHT: function (b, a) {
                    a = e.parseNumber(void 0 === a ? 1 : a);
                    return a instanceof Error ? a : b ? b.substring(b.length - a) : null
                },
                SEARCH: function (b, a, c) {
                    if ("string" !== typeof b || "string" !== typeof a) return k.value;
                    c = void 0 === c ? 0 : c;
                    b = a.toLowerCase().indexOf(b.toLowerCase(), c - 1) + 1;
                    return 0 === b ? k.value : b
                },
                SPLIT: function (b, a) {
                    return b.split(a)
                },
                SUBSTITUTE: function (b, a, c, d) {
                    if (b && a && c) {
                        if (void 0 === d) return b.replace(new RegExp(a, "g"), c);
                        for (var f = 0, h = 0; 0 < b.indexOf(a, f);)
                            if (f = b.indexOf(a, f + 1), h++, h === d) return b.substring(0, f) + c + b.substring(f + a.length)
                    } else return b
                },
                T: function (b) {
                    return "string" === typeof b ? b : ""
                },
                TEXT: null,
                TRIM: function (b) {
                    return "string" !== typeof b ? k.value : b.replace(/ +/g, " ").trim()
                }
            };
            g.UNICHAR = g.CHAR;
            g.UNICODE = g.CODE;
            g.UPPER = function (b) {
                return "string" !== typeof b ? k.value : b.toUpperCase()
            };
            g.VALUE = null;
            return g
        }();
        y.stats = function () {
            var g = {
                AVEDEV: null,
                AVERAGE: function () {
                    for (var b = e.numbers(e.flatten(arguments)), a = b.length, c = 0, d = 0, f = 0; f < a; f++) c += b[f], d += 1;
                    return c / d
                },
                AVERAGEA: function () {
                    for (var b = e.flatten(arguments), a = b.length, c = 0, d = 0, f = 0; f < a; f++) {
                        var h = b[f];
                        "number" === typeof h && (c += h);
                        !0 === h && c++;
                        null !== h && d++
                    }
                    return c / d
                },
                AVERAGEIF: function (b, a, c) {
                    c = c || b;
                    b = e.flatten(b);
                    c = e.parseNumberArray(e.flatten(c));
                    if (c instanceof Error) return c;
                    for (var d = 0, f = 0, h = 0; h < b.length; h++) eval(b[h] + a) && (f += c[h], d++);
                    return f / d
                },
                AVERAGEIFS: null,
                COUNT: function () {
                    return e.numbers(e.flatten(arguments)).length
                },
                COUNTA: function () {
                    var b = e.flatten(arguments);
                    return b.length - g.COUNTBLANK(b)
                },
                COUNTIN: function (b, a) {
                    for (var c = 0, d = 0; d < b.length; d++) b[d] === a && c++;
                    return c
                },
                COUNTBLANK: function () {
                    for (var b = e.flatten(arguments), a = 0, c, d = 0; d < b.length; d++) c = b[d], null !== c && "" !== c || a++;
                    return a
                },
                COUNTIF: function () {
                    var b = e.argsToArray(arguments),
                        a = b.pop();
                    b = e.flatten(b);
                    /[<>=!]/.test(a) || (a = '=="' + a + '"');
                    for (var c = 0, d = 0; d < b.length; d++) "string" !== typeof b[d] ? eval(b[d] + a) && c++ : eval('"' + b[d] + '"' + a) && c++;
                    return c
                },
                COUNTIFS: function () {
                    for (var b =
                        e.argsToArray(arguments), a = Array(e.flatten(b[0]).length), c = 0; c < a.length; c++) a[c] = !0;
                    for (c = 0; c < b.length; c += 2) {
                        var d = e.flatten(b[c]),
                            f = b[c + 1];
                        /[<>=!]/.test(f) || (f = '=="' + f + '"');
                        for (var h = 0; h < d.length; h++) a[h] = "string" !== typeof d[h] ? a[h] && eval(d[h] + f) : a[h] && eval('"' + d[h] + '"' + f)
                    }
                    for (c = b = 0; c < a.length; c++) a[c] && b++;
                    return b
                },
                COUNTUNIQUE: function () {
                    return UNIQUE.apply(null, e.flatten(arguments)).length
                },
                FISHER: function (b) {
                    b = e.parseNumber(b);
                    return b instanceof Error ? b : Math.log((1 + b) / (1 - b)) / 2
                },
                FISHERINV: function (b) {
                    b =
                        e.parseNumber(b);
                    if (b instanceof Error) return b;
                    b = Math.exp(2 * b);
                    return (b - 1) / (b + 1)
                },
                FREQUENCY: function (b, a) {
                    b = e.parseNumberArray(e.flatten(b));
                    a = e.parseNumberArray(e.flatten(a));
                    if (e.anyIsError(b, a)) return k.value;
                    for (var c = b.length, d = a.length, f = [], h = 0; h <= d; h++)
                        for (var l = f[h] = 0; l < c; l++) 0 === h ? b[l] <= a[0] && (f[0] += 1) : h < d ? b[l] > a[h - 1] && b[l] <= a[h] && (f[h] += 1) : h === d && b[l] > a[d - 1] && (f[d] += 1);
                    return f
                },
                LARGE: function (b, a) {
                    b = e.parseNumberArray(e.flatten(b));
                    a = e.parseNumber(a);
                    return e.anyIsError(b, a) ? b : b.sort(function (c,
                        d) {
                        return d - c
                    })[a - 1]
                },
                MAX: function () {
                    var b = e.numbers(e.flatten(arguments));
                    return 0 === b.length ? 0 : Math.max.apply(Math, b)
                },
                MAXA: function () {
                    var b = e.arrayValuesToNumbers(e.flatten(arguments));
                    return 0 === b.length ? 0 : Math.max.apply(Math, b)
                },
                MIN: function () {
                    var b = e.numbers(e.flatten(arguments));
                    return 0 === b.length ? 0 : Math.min.apply(Math, b)
                },
                MINA: function () {
                    var b = e.arrayValuesToNumbers(e.flatten(arguments));
                    return 0 === b.length ? 0 : Math.min.apply(Math, b)
                },
                MODE: {}
            };
            g.MODE.MULT = function () {
                var b = e.parseNumberArray(e.flatten(arguments));
                if (b instanceof Error) return b;
                for (var a = b.length, c = {}, d = [], f = 0, h, l = 0; l < a; l++) h = b[l], c[h] = c[h] ? c[h] + 1 : 1, c[h] > f && (f = c[h], d = []), c[h] === f && (d[d.length] = h);
                return d
            };
            g.MODE.SNGL = function () {
                var b = e.parseNumberArray(e.flatten(arguments));
                return b instanceof Error ? b : g.MODE.MULT(b).sort(function (a, c) {
                    return a - c
                })[0]
            };
            g.PERCENTILE = {};
            g.PERCENTILE.EXC = function (b, a) {
                b = e.parseNumberArray(e.flatten(b));
                a = e.parseNumber(a);
                if (e.anyIsError(b, a)) return k.value;
                b = b.sort(function (d, f) {
                    return d - f
                });
                var c = b.length;
                if (a <
                    1 / (c + 1) || a > 1 - 1 / (c + 1)) return k.num;
                a = a * (c + 1) - 1;
                c = Math.floor(a);
                return e.cleanFloat(a === c ? b[a] : b[c] + (a - c) * (b[c + 1] - b[c]))
            };
            g.PERCENTILE.INC = function (b, a) {
                b = e.parseNumberArray(e.flatten(b));
                a = e.parseNumber(a);
                if (e.anyIsError(b, a)) return k.value;
                b = b.sort(function (d, f) {
                    return d - f
                });
                a *= b.length - 1;
                var c = Math.floor(a);
                return e.cleanFloat(a === c ? b[a] : b[c] + (a - c) * (b[c + 1] - b[c]))
            };
            g.PERCENTRANK = {};
            g.PERCENTRANK.EXC = function (b, a, c) {
                c = void 0 === c ? 3 : c;
                b = e.parseNumberArray(e.flatten(b));
                a = e.parseNumber(a);
                c = e.parseNumber(c);
                if (e.anyIsError(b, a, c)) return k.value;
                b = b.sort(function (p, q) {
                    return p - q
                });
                var d = UNIQUE.apply(null, b),
                    f = b.length,
                    h = d.length;
                c = Math.pow(10, c);
                for (var l = 0, m = !1, n = 0; !m && n < h;) a === d[n] ? (l = (b.indexOf(d[n]) + 1) / (f + 1), m = !0) : a >= d[n] && (a < d[n + 1] || n === h - 1) && (l = (b.indexOf(d[n]) + 1 + (a - d[n]) / (d[n + 1] - d[n])) / (f + 1), m = !0), n++;
                return Math.floor(l * c) / c
            };
            g.PERCENTRANK.INC = function (b, a, c) {
                c = void 0 === c ? 3 : c;
                b = e.parseNumberArray(e.flatten(b));
                a = e.parseNumber(a);
                c = e.parseNumber(c);
                if (e.anyIsError(b, a, c)) return k.value;
                b = b.sort(function (p,
                    q) {
                    return p - q
                });
                var d = UNIQUE.apply(null, b),
                    f = b.length,
                    h = d.length;
                c = Math.pow(10, c);
                for (var l = 0, m = !1, n = 0; !m && n < h;) a === d[n] ? (l = b.indexOf(d[n]) / (f - 1), m = !0) : a >= d[n] && (a < d[n + 1] || n === h - 1) && (l = (b.indexOf(d[n]) + (a - d[n]) / (d[n + 1] - d[n])) / (f - 1), m = !0), n++;
                return Math.floor(l * c) / c
            };
            g.PERMUT = function (b, a) {
                b = e.parseNumber(b);
                a = e.parseNumber(a);
                return e.anyIsError(b, a) ? k.value : FACT(b) / FACT(b - a)
            };
            g.PERMUTATIONA = function (b, a) {
                b = e.parseNumber(b);
                a = e.parseNumber(a);
                return e.anyIsError(b, a) ? k.value : Math.pow(b, a)
            };
            g.PHI =
                function (b) {
                    b = e.parseNumber(b);
                    return b instanceof Error ? k.value : Math.exp(-.5 * b * b) / 2.5066282746310002
                };
            g.PROB = function (b, a, c, d) {
                if (void 0 === c) return 0;
                d = void 0 === d ? c : d;
                b = e.parseNumberArray(e.flatten(b));
                a = e.parseNumberArray(e.flatten(a));
                c = e.parseNumber(c);
                d = e.parseNumber(d);
                if (e.anyIsError(b, a, c, d)) return k.value;
                if (c === d) return 0 <= b.indexOf(c) ? a[b.indexOf(c)] : 0;
                for (var f = b.sort(function (n, p) {
                    return n - p
                }), h = f.length, l = 0, m = 0; m < h; m++) f[m] >= c && f[m] <= d && (l += a[b.indexOf(f[m])]);
                return l
            };
            g.QUARTILE = {};
            g.QUARTILE.EXC = function (b, a) {
                b = e.parseNumberArray(e.flatten(b));
                a = e.parseNumber(a);
                if (e.anyIsError(b, a)) return k.value;
                switch (a) {
                    case 1:
                        return g.PERCENTILE.EXC(b, .25);
                    case 2:
                        return g.PERCENTILE.EXC(b, .5);
                    case 3:
                        return g.PERCENTILE.EXC(b, .75);
                    default:
                        return k.num
                }
            };
            g.QUARTILE.INC = function (b, a) {
                b = e.parseNumberArray(e.flatten(b));
                a = e.parseNumber(a);
                if (e.anyIsError(b, a)) return k.value;
                switch (a) {
                    case 1:
                        return g.PERCENTILE.INC(b, .25);
                    case 2:
                        return g.PERCENTILE.INC(b, .5);
                    case 3:
                        return g.PERCENTILE.INC(b,
                            .75);
                    default:
                        return k.num
                }
            };
            g.RANK = {};
            g.RANK.AVG = function (b, a, c) {
                b = e.parseNumber(b);
                a = e.parseNumberArray(e.flatten(a));
                if (e.anyIsError(b, a)) return k.value;
                a = e.flatten(a);
                a = a.sort(c ? function (h, l) {
                    return h - l
                } : function (h, l) {
                    return l - h
                });
                c = a.length;
                for (var d = 0, f = 0; f < c; f++) a[f] === b && d++;
                return 1 < d ? (2 * a.indexOf(b) + d + 1) / 2 : a.indexOf(b) + 1
            };
            g.RANK.EQ = function (b, a, c) {
                b = e.parseNumber(b);
                a = e.parseNumberArray(e.flatten(a));
                if (e.anyIsError(b, a)) return k.value;
                a = a.sort(c ? function (d, f) {
                    return d - f
                } : function (d, f) {
                    return f -
                        d
                });
                return a.indexOf(b) + 1
            };
            g.RSQ = function (b, a) {
                b = e.parseNumberArray(e.flatten(b));
                a = e.parseNumberArray(e.flatten(a));
                return e.anyIsError(b, a) ? k.value : Math.pow(g.PEARSON(b, a), 2)
            };
            g.SMALL = function (b, a) {
                b = e.parseNumberArray(e.flatten(b));
                a = e.parseNumber(a);
                return e.anyIsError(b, a) ? b : b.sort(function (c, d) {
                    return c - d
                })[a - 1]
            };
            g.STANDARDIZE = function (b, a, c) {
                b = e.parseNumber(b);
                a = e.parseNumber(a);
                c = e.parseNumber(c);
                return e.anyIsError(b, a, c) ? k.value : (b - a) / c
            };
            g.STDEV = {};
            g.STDEV.P = function () {
                var b = g.VAR.P.apply(this,
                    arguments);
                return Math.sqrt(b)
            };
            g.STDEV.S = function () {
                var b = g.VAR.S.apply(this, arguments);
                return Math.sqrt(b)
            };
            g.STDEVA = function () {
                var b = g.VARA.apply(this, arguments);
                return Math.sqrt(b)
            };
            g.STDEVPA = function () {
                var b = g.VARPA.apply(this, arguments);
                return Math.sqrt(b)
            };
            g.VAR = {};
            g.VAR.P = function () {
                for (var b = e.numbers(e.flatten(arguments)), a = b.length, c = 0, d = g.AVERAGE(b), f = 0; f < a; f++) c += Math.pow(b[f] - d, 2);
                return c / a
            };
            g.VAR.S = function () {
                for (var b = e.numbers(e.flatten(arguments)), a = b.length, c = 0, d = g.AVERAGE(b),
                    f = 0; f < a; f++) c += Math.pow(b[f] - d, 2);
                return c / (a - 1)
            };
            g.VARA = function () {
                for (var b = e.flatten(arguments), a = b.length, c = 0, d = 0, f = g.AVERAGEA(b), h = 0; h < a; h++) {
                    var l = b[h];
                    c = "number" === typeof l ? c + Math.pow(l - f, 2) : !0 === l ? c + Math.pow(1 - f, 2) : c + Math.pow(0 - f, 2);
                    null !== l && d++
                }
                return c / (d - 1)
            };
            g.VARPA = function () {
                for (var b = e.flatten(arguments), a = b.length, c = 0, d = 0, f = g.AVERAGEA(b), h = 0; h < a; h++) {
                    var l = b[h];
                    c = "number" === typeof l ? c + Math.pow(l - f, 2) : !0 === l ? c + Math.pow(1 - f, 2) : c + Math.pow(0 - f, 2);
                    null !== l && d++
                }
                return c / d
            };
            g.WEIBULL = {};
            g.WEIBULL.DIST = function (b, a, c, d) {
                b = e.parseNumber(b);
                a = e.parseNumber(a);
                c = e.parseNumber(c);
                return e.anyIsError(b, a, c) ? k.value : d ? 1 - Math.exp(-Math.pow(b / c, a)) : Math.pow(b, a - 1) * Math.exp(-Math.pow(b / c, a)) * a / Math.pow(c, a)
            };
            g.Z = {};
            g.Z.TEST = function (b, a, c) {
                b = e.parseNumberArray(e.flatten(b));
                a = e.parseNumber(a);
                if (e.anyIsError(b, a)) return k.value;
                c = c || g.STDEV.S(b);
                var d = b.length;
                return 1 - g.NORM.S.DIST((g.AVERAGE(b) - a) / (c / Math.sqrt(d)), !0)
            };
            return g
        }();
        y.utils = function () {
            return {
                PROGRESS: function (g, b) {
                    return '<div style="width:' +
                        (g ? g : "0") + "%;height:4px;background-color:" + (b ? b : "red") + ';margin-top:1px;"></div>'
                },
                RATING: function (g) {
                    for (var b = '<div class="jrating">', a = 0; 5 > a; a++) b = a < g ? b + '<div class="jrating-selected"></div>' : b + "<div></div>";
                    return b + "</div>"
                }
            }
        }();
        for (var G = 0; G < Object.keys(y).length; G++)
            for (var B = y[Object.keys(y)[G]], z = Object.keys(B), x = 0; x < z.length; x++)
                if (B[z[x]])
                    if ("function" == typeof B[z[x]] || "object" == typeof B[z[x]]) {
                        if (w[z[x]] = B[z[x]], w[z[x]].toString = function () {
                            return "#ERROR"
                        }, "object" == typeof B[z[x]])
                            for (var J =
                                Object.keys(B[z[x]]), H = 0; H < J.length; H++) w[z[x]][J[H]].toString = function () {
                                    return "#ERROR"
                                }
                    } else w[z[x]] = function () {
                        return z[x] + "Not implemented"
                    };
                else w[z[x]] = function () {
                    return z[x] + "Not implemented"
                };
        var I = null,
            D = null,
            E = null;
        w.TABLE = function () {
            return E
        };
        w.COLUMN = w.COL = function () {
            return parseInt(I) + 1
        };
        w.ROW = function () {
            return parseInt(D) + 1
        };
        w.CELL = function () {
            return A.getColumnNameFromCoords(I, D)
        };
        w.VALUE = function (g, b, a) {
            return E.getValueFromCoords(parseInt(g) - 1, parseInt(b) - 1, a)
        };
        w.THISROWCELL = function (g) {
            return E.getValueFromCoords(parseInt(g) -
                1, parseInt(D))
        };
        var K = function (g, b) {
            for (var a = 0; a < g.length; a++) {
                var c = A.getTokensFromRange(g[a]);
                b = b.replace(g[a], "[" + c.join(",") + "]")
            }
            return b
        },
            A = function (g, b, a, c, d) {
                E = d;
                I = a;
                D = c;
                c = "";
                d = Object.keys(b);
                if (d.length) {
                    var f = {};
                    for (a = 0; a < d.length; a++)
                        if (h = d[a].replace(/!/g, "."), 0 < h.indexOf(".")) {
                            var h = h.split(".");
                            f[h[0]] = {}
                        } h = Object.keys(f);
                    for (a = 0; a < h.length; a++) c += "var " + h[a] + " = {};";
                    for (a = 0; a < d.length; a++) h = d[a].replace(/!/g, "."), f = b[d[a]], (isNaN(f) || null === f || "" === f) && (f = b[d[a]].match(/(('.*?'!)|(\w*!))?(\$?[A-Z]+\$?[0-9]*):(\$?[A-Z]+\$?[0-9]*)?/g)) &&
                        f.length && (b[d[a]] = K(f, b[d[a]])), c = 0 < h.indexOf(".") ? c + (h + " = " + b[d[a]] + ";\n") : c + ("var " + h + " = " + b[d[a]] + ";\n")
                }
                g = g.replace(/\$/g, "");
                g = g.replace(/!/g, ".");
                b = "";
                a = 0;
                d = ["=", "!", ">", "<"];
                for (h = 0; h < g.length; h++) '"' == g[h] && (a = 0 == a ? 1 : 0), 1 == a ? b += g[h] : (b += g[h].toUpperCase(), 0 < h && "=" == g[h] && -1 == d.indexOf(g[h - 1]) && -1 == d.indexOf(g[h + 1]) && (b += "="));
                b = b.replace(/\^/g, "**");
                b = b.replace(/<>/g, "!=");
                b = b.replace(/&/g, "+");
                g = b = b.replace(/\$/g, "");
                (f = g.match(/(('.*?'!)|(\w*!))?(\$?[A-Z]+\$?[0-9]*):(\$?[A-Z]+\$?[0-9]*)?/g)) &&
                    f.length && (g = K(f, g));
                return (new Function(c + "; return " + g))()
            };
        A.getColumnNameFromCoords = function (g, b) {
            g = parseInt(g);
            var a = "";
            701 < g ? (a += String.fromCharCode(64 + parseInt(g / 676)), a += String.fromCharCode(64 + parseInt(g % 676 / 26))) : 25 < g && (a += String.fromCharCode(64 + parseInt(g / 26)));
            a += String.fromCharCode(65 + g % 26);
            return a + (parseInt(b) + 1)
        };
        A.getCoordsFromColumnName = function (g) {
            var b = /^[a-zA-Z]+/.exec(g);
            if (b) {
                for (var a = 0, c = 0; c < b[0].length; c++) a += parseInt(b[0].charCodeAt(c) - 64) * Math.pow(26, b[0].length - 1 - c);
                a--;
                0 > a && (a = 0);
                g = parseInt(/[0-9]+$/.exec(g)) || null;
                0 < g && g--;
                return [a, g]
            }
        };
        A.getRangeFromTokens = function (g) {
            g = g.filter(function (d) {
                return "#REF!" != d
            });
            for (var b = "", a = "", c = 0; c < g.length; c++) 0 <= g[c].indexOf(".") ? b = "." : 0 <= g[c].indexOf("!") && (b = "!"), b && (a = g[c].split(b), g[c] = a[1], a = a[0] + b);
            g.sort(function (d, f) {
                d = Helpers.getCoordsFromColumnName(d);
                f = Helpers.getCoordsFromColumnName(f);
                return d[1] > f[1] ? 1 : d[1] < f[1] ? -1 : d[0] > f[0] ? 1 : d[0] < f[0] ? -1 : 0
            });
            return g.length ? a + (g[0] + ":" + g[g.length - 1]) : "#REF!"
        };
        A.getTokensFromRange =
            function (g) {
                if (0 < g.indexOf(".")) {
                    var b = g.split(".");
                    g = b[1];
                    b = b[0] + "."
                } else 0 < g.indexOf("!") ? (b = g.split("!"), g = b[1], b = b[0] + "!") : b = "";
                g = g.split(":");
                var a = A.getCoordsFromColumnName(g[0]),
                    c = A.getCoordsFromColumnName(g[1]);
                if (a[0] <= c[0]) {
                    g = a[0];
                    var d = c[0]
                } else g = c[0], d = a[0];
                if (null === a[1] && null == c[1])
                    for (var f = null, h = null, l = Object.keys(vars), m = 0; m < l.length; m++) {
                        var n = A.getCoordsFromColumnName(l[m]);
                        n[0] === a[0] && (null === f || n[1] < f) && (f = n[1]);
                        n[0] === c[0] && (null === h || n[1] > h) && (h = n[1])
                    } else a[1] <= c[1] ? (f =
                        a[1], h = c[1]) : (f = c[1], h = a[1]);
                for (a = []; f <= h; f++) {
                    c = [];
                    for (m = g; m <= d; m++) c.push(b + A.getColumnNameFromCoords(m, f));
                    a.push(c)
                }
                return a
            };
        A.setFormula = function (g) {
            for (var b = Object.keys(g), a = 0; a < b.length; a++) "function" == typeof g[b[a]] && (w[b[a]] = g[b[a]])
        };
        return A
    };
    return "undefined" !== typeof window ? F(window) : null
});

if (!jSuites && typeof (require) === 'function') {
    var jSuites = require('jsuites');
}

var $jscomp = $jscomp || {};
$jscomp.scope = {};
$jscomp.arrayIteratorImpl = function (B) {
    var I = 0;
    return function () {
        return I < B.length ? {
            done: !1,
            value: B[I++]
        } : {
            done: !0
        }
    }
};
$jscomp.arrayIterator = function (B) {
    return {
        next: $jscomp.arrayIteratorImpl(B)
    }
};
$jscomp.ASSUME_ES5 = !1;
$jscomp.ASSUME_NO_NATIVE_MAP = !1;
$jscomp.ASSUME_NO_NATIVE_SET = !1;
$jscomp.SIMPLE_FROUND_POLYFILL = !1;
$jscomp.ISOLATE_POLYFILLS = !1;
$jscomp.FORCE_POLYFILL_PROMISE = !1;
$jscomp.FORCE_POLYFILL_PROMISE_WHEN_NO_UNHANDLED_REJECTION = !1;
$jscomp.defineProperty = $jscomp.ASSUME_ES5 || "function" == typeof Object.defineProperties ? Object.defineProperty : function (B, I, K) {
    if (B == Array.prototype || B == Object.prototype) return B;
    B[I] = K.value;
    return B
};
$jscomp.getGlobal = function (B) {
    B = ["object" == typeof globalThis && globalThis, B, "object" == typeof window && window, "object" == typeof self && self, "object" == typeof global && global];
    for (var I = 0; I < B.length; ++I) {
        var K = B[I];
        if (K && K.Math == Math) return K
    }
    throw Error("Cannot find global object");
};
$jscomp.global = $jscomp.getGlobal(this);
$jscomp.IS_SYMBOL_NATIVE = "function" === typeof Symbol && "symbol" === typeof Symbol("x");
$jscomp.TRUST_ES6_POLYFILLS = !$jscomp.ISOLATE_POLYFILLS || $jscomp.IS_SYMBOL_NATIVE;
$jscomp.polyfills = {};
$jscomp.propertyToPolyfillSymbol = {};
$jscomp.POLYFILL_PREFIX = "$jscp$";
var $jscomp$lookupPolyfilledValue = function (B, I) {
    var K = $jscomp.propertyToPolyfillSymbol[I];
    if (null == K) return B[I];
    K = B[K];
    return void 0 !== K ? K : B[I]
};
$jscomp.polyfill = function (B, I, K, S) {
    I && ($jscomp.ISOLATE_POLYFILLS ? $jscomp.polyfillIsolated(B, I, K, S) : $jscomp.polyfillUnisolated(B, I, K, S))
};
$jscomp.polyfillUnisolated = function (B, I, K, S) {
    K = $jscomp.global;
    B = B.split(".");
    for (S = 0; S < B.length - 1; S++) {
        var P = B[S];
        if (!(P in K)) return;
        K = K[P]
    }
    B = B[B.length - 1];
    S = K[B];
    I = I(S);
    I != S && null != I && $jscomp.defineProperty(K, B, {
        configurable: !0,
        writable: !0,
        value: I
    })
};
$jscomp.polyfillIsolated = function (B, I, K, S) {
    var P = B.split(".");
    B = 1 === P.length;
    S = P[0];
    S = !B && S in $jscomp.polyfills ? $jscomp.polyfills : $jscomp.global;
    for (var Z = 0; Z < P.length - 1; Z++) {
        var M = P[Z];
        if (!(M in S)) return;
        S = S[M]
    }
    P = P[P.length - 1];
    K = $jscomp.IS_SYMBOL_NATIVE && "es6" === K ? S[P] : null;
    I = I(K);
    null != I && (B ? $jscomp.defineProperty($jscomp.polyfills, P, {
        configurable: !0,
        writable: !0,
        value: I
    }) : I !== K && (void 0 === $jscomp.propertyToPolyfillSymbol[P] && (K = 1E9 * Math.random() >>> 0, $jscomp.propertyToPolyfillSymbol[P] = $jscomp.IS_SYMBOL_NATIVE ?
        $jscomp.global.Symbol(P) : $jscomp.POLYFILL_PREFIX + K + "$" + P), $jscomp.defineProperty(S, $jscomp.propertyToPolyfillSymbol[P], {
            configurable: !0,
            writable: !0,
            value: I
        })))
};
$jscomp.initSymbol = function () { };
$jscomp.iteratorPrototype = function (B) {
    B = {
        next: B
    };
    B[Symbol.iterator] = function () {
        return this
    };
    return B
};
$jscomp.iteratorFromArray = function (B, I) {
    B instanceof String && (B += "");
    var K = 0,
        S = !1,
        P = {
            next: function () {
                if (!S && K < B.length) {
                    var Z = K++;
                    return {
                        value: I(Z, B[Z]),
                        done: !1
                    }
                }
                S = !0;
                return {
                    done: !0,
                    value: void 0
                }
            }
        };
    P[Symbol.iterator] = function () {
        return P
    };
    return P
};
$jscomp.polyfill("Array.prototype.values", function (B) {
    return B ? B : function () {
        return $jscomp.iteratorFromArray(this, function (I, K) {
            return K
        })
    }
}, "es8", "es3");
var _$_eb2b = ["use strict", "{", "}", "", "8.7.12", "Base", "https://jspreadsheet.com", "Unlicensed", "Jspreadsheet Pro\r\n", " edition ", "\r\n", "set", "=", "undefined", "number", "fullscreen", "block", "string", "readonly", "jss_hidden_index", "div", "right", "10px", "pointer", "/v8", ",", "localhost", ".", "v8", " (Expired)", "Licensed to: ", "Premium", "License required", "A valid license is required", "This version is not included on the scope of this license", "Your license is expired", "jspreadsheet", "img", "/jspreadsheet/", "logo.png",
    "none", "status", "data-x", "data-y", "px", "X", "goto", "jss_scroll", "jss_control", "hidden", "jss_freezed", "get", "colspan", "300px", "Jspreadsheet Pro", "text", "autocomplete", "dropdown", "GET", "json", "Jspreadsheet Alert: secureFormulas is set to false.", "setConfig", "insertRow", "deleteRow", "insertColumn", "deleteColumn", "moveRow", "moveColumn", "setMerge", "removeMerge", "setStyle", "resetStyle", "setWidth", "setHeight", "setHeader", "setComments", "orderBy", "setValue", "setProperty", "updateProperty", "renameWorksheet", "moveWorksheet",
    "onundo", "onredo", "object", "function", "contextMenu", "toolbar", "onevent", "Jspreadsheet: cursor not in the viewport", "keydown", "oneditionstart", "oncreateeditor", "jss_focus", "jss_formula", "oneditionend", "data-mask", "data-value", "inputmode", "0", "style", "jss_input", "contentEditable", "setRowId", "onchangerowid", "setBorder", "resetBorders", "updateCells", "setFormula", "updateData", "onbeforesave", "POST", "Authorization", "Bearer ", "onbeforesend", "disconected", ": ", "Sorry, something went wrong, refreshing your spreadsheet...",
    "_top", "onsave", "persistence", "?", "?worksheet=", "&worksheet=", "add", "remove", "jss_cursor", "selected", "main", "copying", "onblur", "onfocus", "onselection", " ", "'", "'!", "!", ":", "jss_border", "cloning", "jss_border_", "15", "-2000px", "#ccc", "transparent", "0px", "tr", "jss_group_container", "td", "&nbsp;", ".jss_group", "jss_group", "+", "width", "Jspreadsheet: This column is part of a merged cell.", "jss_dragging", "1px", "jss_helper_col", "Jspreadsheet: This row is part of a merged cell", "Jspreadsheet: Please clear your search before perform this action",
    "51px", "jss_helper_row", "No cell selected", "Invalid merged properties", "onmerge", "rowspan", "Column name", "data-title", "onchangeheader", "jss_filters_icon", "col", "oncreatecolumn", "setFooter", "onchangefooter", "setFooterValue", "onchangefootervalue", "resetFooter", "setNestedHeaders", "onchangenested", "jss_nested", "resetNestedHeaders", "setNestedCell", "onchangenestedcell", "center", "data-nested-x", "data-nested-y", "align", "calendar", "-", "00:00:00", '"', "g", '""', "\t", "oncopy", "copy", "color: initial", "onbeforepaste",
    "JSS: For better results, please allow pagination or tableOverflow.", "onpaste", "Export not allowed", "\ufeff", "text/csv;charset=utf-8;", ".csv", "a", "download", "setDefinedNames", "#1a237e", "#b71c1c", "#880e4f", "#1b5e20", "#ff6f00", "/", "*", "^", "(", "[", "|", "<", ">", ";", "data-token", "picker", "jss_picker", "click", "active", "focus", "input", "jss_object", "contenteditable", "blur", "last", '<b class="', '" data-token="', '">', "</b>", "data-single", "true", "b", "&", ")", "]", "&lt;", "&gt;", "var ", " = {};", " = ", ";\n", "; return ",
    "onbeforeformula", "#LOOP", "Reference loop detected", "#SELF", "Self Reference detected", "#ERROR", "Table does not exist or is not loaded yet", "CHART(", "#SPILL!", "#REF!", "$", "Something went wrong", "onafterchanges", "checkbox", "autonumber", "radio", "email", "url", "color", "progressbar", "rating", "onformulachain", "onbeforechange", "onchange", "oncreatecell", "values", "mergeCells", "jss_wrap", "title", "jss_unlocked", "onchangeproperty", "oncreaterow", "jss_row", "Jspreadsheet: Invalid origin or destination", "onmoverow",
    "Jspreadsheet: Insert row is not enabled in the table configuration", "onbeforeinsertrow", "onbeforeinsertrow returned false", "oninsertrow", "Jspreadsheet: Delete row is not enabled in the table configuration", "onbeforedeleterow", "onbeforedeleterow returned false", "meta", "cells", "comments", "ondeleterow", "Jspreadsheet: Invalid origin", "Jspreadsheet: Invalid destination", "onmovecolumn", "Jspreadsheet: Insert column is not enabled in the table configuration", "onbeforeinsertcolumn", "onbeforeinsertcolumn returned false",
    "col_", "oninsertcolumn", "Jspreadsheet: Delete column is not enabled in the table configuration", "Jspreadsheet: it is not possible to delete the last column", "onbeforedeletecolumn", "onbeforedeletecolumn returned false", "ondeletecolumn", "; ", "onchangestyle", "No cell provided", "onresetstyle", "onbeforecomments", "oncomments", "setMeta", "onchangemeta", "resetMeta", "onresetmeta", "arrow-down", "percent", "numeric", "onbeforesort", "This action will destroy any existing merged cells. Are you sure?", "onsort", "arrow-up",
    "jss_pagination", "Jspreadsheet: No pagination defined", "Jspreadsheet: pagination not defined", "onbeforechangepage", "onchangepage", "No records found", "jss_page", "jss_page_selected", "Showing page {0} of {1} entries", "jss_content", "wheel", "selectstart", "table", "thead", "tbody", "tfoot", "colgroup", "50", "jss_selectall", "jss", "cellpadding", "cellspacing", "unselectable", "yes", "jss_overflow", "jss_locked", "jss_corner", "on", "onselectstart", "return false", "Y", "jss_table", "jss_table_container", "Freeze columns cannot be greater than the number of available columns",
    "rows", "jss_container", "jss_toolbar", "jss_helper", "jss_loading", "textarea", "jss_textarea", "-1", "bottom", "Defined names are only available on the Premium edition.", "beforeinit", "Sheet", "Worksheet clash name: ", ". It is highly recommended to define a unique worksheetName on the initialization.", "jss_worksheet", "init", "To improve the performance, use tableOverflow or pagination.", "resize", "touchstart", "touchmove", "touchend", "onresizecolumn", "onresizerow", "label", "jss_filters_active", "Jspreadsheet: the filter is not enabled.",
    "onopenfilter", "Blanks", "onbeforefilter", "onfilter", "gi", "jss_filters_options", "INPUT", "Search", "jss_filters_search", "button", "Ok", "jss_filters_apply", "jss_filters", '<input type="checkbox"> ', "Add current selection to filter", '<input type="checkbox"> (', "Select all", "select", "jss_pagination_dropdown", "option", "Show", "entries", "jss_search", "onbeforesearch", "onsearch", "jss_search_container", "jss_contextmenu", "Mac", "&#8984;", "Ctrl", "tabs", "Rename this worksheet", "Delete this worksheet", "Are you sure?",
    "line", "nested", "Rename this cell", "header", "row", "cell", "Cut", "content_cut", " + X", "Copy", "content_copy", " + C", "Paste", "content_paste", " + V", "Insert a new column before", "Insert a new column after", "Delete selected columns", "Rename this column", "Create a new row", "Order ascending", "Order descending", "Insert a new row before", "Insert a new row after", "Delete selected rows", "Edit comments", "Add comments", "notes", "Comments", "Clear comments", "Save as", "save", " + S", "About", "info", "with-toolbar", "i",
    "undo", "redo", "divisor", "120px", "Verdana", "Arial", "Courier New", '<span style="font-family:', "</span>", "font-family", "48px", "format_size", "x-small", "small", "medium", "large", "x-large", '<span style="font-size:', "font-size", "format_align_left", "format_align_center", "format_align_right", "format_align_justify", '<i class="material-icons">', "</i>", "text-align", "_", "format_bold", "font-weight", "bold", "format_color_text", "format_color_fill", "background-color", "border_all", "border_outer", "border_inner", "border_horizontal",
    "border_vertical", "border_left", "border_top", "border_right", "border_bottom", "border_clear", "black", "border-left: ", "px solid ", "border-left: ; ", "border-right: ", "border-right: ; ", "border-top: ", "border-top: ; ", "border-bottom: ", "border-bottom: ; ", "material-icons", "color_lens", '<div style="height: ', 'px; width: 50px; background-color: black;"></div>', "80px", "1", "fullscreen_exit", "Toggle Fullscreen", "search", "search_off", "Toggle Search", "onbeforecreateworksheet", "There is one existing worksheet with the same name.",
    "createWorksheet", "oncreateworksheet", "deleteWorksheet", "ondeleteworksheet", "It was not possible to rename worksheet due conflict name", "onrenameworksheet", "onmoveworksheet", "onopenworksheet", "jss_", "TABLE", "Element is not a table", "colgroup > col", "data-celltype", "readOnly", "name", "id", ":scope > thead > tr", ":scope > tr, :scope > tbody > tr", "data-formula", "class", "styleBold", "; font-weight:bold;", "font-weight:bold;", "tfoot tr", "\r", "\n", "jss_ignore", "THEAD", "TBODY", "TFOOT", "selectall", "header-group",
    "jtabs-headers", "filters", "footer", "jtoolbar", "DIV", "html", "jss_nowrap", "insertHTML", "<br/>\n", "Process", "A", "mailto:", "_blank", "onclick", "jclose", "col-resize", "row-resize", "move", "TD", "Cloud plugin not loaded.", "onload", "jtabs", "jss_dialog", "jss_number", "decimal", "data-locale", "%", "jss_percent", "\\[Red\\]", "red", "\\(", "jss_notes", "145px", "tag", "jss_dropdown_tags", "jss_dropdown", "multiple", "image", "<div class='jss_dropdown_icon' style='background-color: ", "'></div>", "orange", "<div class='jss_dropdown_tag' style='background-color: ",
    "'>", "</div>", "timepicker", "YYYY-MM-DD", "square", "100px", "false", "FALSE", "disabled", "TRUE", "jss_progressbar", "min", "max", "jss_rating", " stars", "star", "href", "IMG", "round", "100%", "jss_image", "tabindex", "jpanel", "delete", "jfile", '<img src="', '" alt="">', "jss_richtext", "jss_tags", "40px", "#NOTFOUND", "<img src='", "' class='round small' />", "Jspreadsheet: worksheet not found "
];

(function (B, I) {
    typeof exports === _$_eb2b[84] && typeof module !== _$_eb2b[13] ? module.exports = I() : typeof define === _$_eb2b[85] && define.amd ? define(I) : B.jspreadsheet = B.jexcel = I()
})(this, function () {
    _$_eb2b[0];
    var B = function (k, b) {
        k = jSuites.translate(k);
        if (b && b.length)
            for (var c = 0; c < b.length; c++) k = k.replace(_$_eb2b[1] + c + _$_eb2b[2], b[c]);
        return k
    },
        I = function (k, b) {
            Array.isArray(k) && (k = k[0]);
            k = k.replace(new RegExp(/'/g), _$_eb2b[3]).toUpperCase();
            if (void 0 === b) return window[k] ? window[k] : null;
            window[k] = b
        },
        K = function () {
            var k = {
                version: _$_eb2b[4],
                edition: _$_eb2b[5],
                host: _$_eb2b[6],
                license: _$_eb2b[7],
                print: function () {
                    return [_$_eb2b[8] + this.edition + _$_eb2b[9] + this.version + _$_eb2b[10] + this.host + _$_eb2b[10] + this.license]
                }
            };
            return function () {
                return k
            }
        }(),
        S = function (k) {
            var b = parseInt(k.y);
            if (k.id) var c = parseInt(k.id);
            else k.value && (c = parseInt(k.value));
            0 < c && (this.rows[b].id = c, ta[_$_eb2b[11]].call(this, c))
        },
        P = function (k) {
            return (_$_eb2b[3] + k).substr(0, 1) === _$_eb2b[12]
        },
        Z = function (k) {
            var b = this.options.columns;
            return b[k] && b[k].name ?
                b[k].name : k
        },
        M = function (k, b, c) {
            var a;
            if (a = this.options.data[b]) {
                var d = 0 <= k ? Z.call(this, k) : k;
                typeof c === _$_eb2b[13] ? c = typeof d === _$_eb2b[14] ? a[k] : jSuites.path.call(a, _$_eb2b[3] + d) : (typeof d === _$_eb2b[14] ? a[k] = c : jSuites.path.call(a, _$_eb2b[3] + d, c), !P(c) && this.records[b][k] && (this.records[b][k].v = c))
            } else c = null;
            return c
        },
        Q = function (k) {
            return 0 == this.config.editable || k && 0 == k.options.editable ? !1 : (k = Ja.call(this)) ? 7 <= k ? !0 : !1 : !0
        },
        Ka = function (k) {
            var b = _$_eb2b[15];
            if (this.parent) var c = this.parent.config,
                a = this.parent.element,
                d = this;
            else c = this.config, a = this.element, d = r.current ? r.current : null;
            typeof k === _$_eb2b[13] && (k = !a.classList.contains(b));
            k ? (a.classList.add(b), c.fullscreen = !0) : (a.classList.remove(b), c.fullscreen = !1);
            d && (d.tbody.innerHTML = _$_eb2b[3], G.call(d), U.refresh.call(d), fa.update.call(d.parent, d))
        },
        La = function (k) {
            var b = this.loading;
            null === k && (k = b.style.display == _$_eb2b[16] ? !0 : !1);
            b.style.display = k ? _$_eb2b[16] : _$_eb2b[3];
            setTimeout(function () {
                b.style.display = _$_eb2b[3]
            }, 1E3)
        },
        Ga = function (k,
            b) {
            if (typeof k === _$_eb2b[17]) {
                var c = E.getCoordsFromColumnName(k);
                c = this.records[c[1]][c[0]];
                if (c.element) k = c.element;
                else return typeof b === _$_eb2b[13] ? c.readonly : c.readonly = b ? !0 : !1
            }
            c = _$_eb2b[18];
            k = k.classList;
            if (typeof b === _$_eb2b[13]) return k.contains(c) ? !0 : !1;
            b ? k.add(c) : k.remove(c)
        },
        ya = function (k) {
            var b = _$_eb2b[19],
                c = this.table.classList;
            if (typeof k == _$_eb2b[13]) return c.contains(b) ? !1 : !0;
            k ? c.remove(b) : c.add(b);
            0 < this.options.freezeColumns && ja[_$_eb2b[11]].call(this, this.options.freezeColumns);
            this.refreshBorders()
        },
        Ja = function (k) {
            if (void 0 === k) return this.status;
            var b = document.createElement(_$_eb2b[20]);
            b.style.textAlign = _$_eb2b[21];
            b.style.fontSize = _$_eb2b[22];
            b.style.cursor = _$_eb2b[23];
            b.onclick = function () {
                window.location.href = K().host + _$_eb2b[24]
            };
            var c = null,
                a = [74, 50, 48, 50, 48, 33].join(_$_eb2b[3]);
            (function (e) {
                c = 1;
                try {
                    if (e) {
                        var f = window.atob(e);
                        f = f.split(_$_eb2b[25]);
                        if (f[1]) {
                            f[1] = window.atob(f[1]);
                            var g = f[0];
                            e = a;
                            var h = f[1],
                                l = jSuites.sha512,
                                m = _$_eb2b[3],
                                p = _$_eb2b[3];
                            128 < e.length && (e =
                                l(e));
                            for (var n = 0; 128 > n; n++) {
                                var t = e[n] ? e[n].charCodeAt(0) : 0;
                                m += String.fromCharCode(54 ^ t);
                                p += String.fromCharCode(92 ^ t)
                            }
                            var v = l(p + l(m + h));
                            if (g != v) c = 3;
                            else if (f[1] = JSON.parse(f[1]), f[1].date) {
                                var u = window.location.hostname || _$_eb2b[26];
                                g = !1;
                                if (-1 === u.indexOf(_$_eb2b[27]))
                                    for (var y = 0; y < f[1].domain.length; y++) u === f[1].domain[y] && (g = !0);
                                else
                                    for (y = 0; y < f[1].domain.length; y++) - 1 !== u.indexOf(f[1].domain[y]) && u.substr(u.indexOf(f[1].domain[y])) == f[1].domain[y] && (g = !0);
                                !1 === g ? c = 4 : f[1].scope && 0 <= f[1].scope.indexOf(_$_eb2b[28]) ?
                                    (u = new Date, g = parseInt(u.getTime() / 1E3), y = K(), f[1].date < g ? (c = f[1].date + 2592E3 < g && !f[1].plan || f[1].date + 15552E3 < g ? 6 : 7, y.license = f[1].name + _$_eb2b[29]) : (c = 8, y.license = _$_eb2b[30] + f[1].name, y.edition = 3 == f[1].plan || 6 == f[1].plan ? _$_eb2b[31] : _$_eb2b[5])) : c = 5
                            } else c = 4
                        } else c = 2
                    }
                } catch (z) { }
            })(k);
            k = _$_eb2b[3];
            if (1 == c) k = _$_eb2b[32];
            else if (2 == c || 3 == c || 4 == c) k = _$_eb2b[33];
            else if (5 == c) k = _$_eb2b[34];
            else if (6 == c || 7 == c) k = _$_eb2b[35];
            b.appendChild(document.createTextNode(k));
            if (8 > c) try {
                if (typeof sessionStorage !==
                    _$_eb2b[13] && !sessionStorage.getItem(_$_eb2b[36])) {
                    sessionStorage.setItem(_$_eb2b[36], !0);
                    var d = document.createElement(_$_eb2b[37]);
                    d.src = K().host + _$_eb2b[38] + _$_eb2b[39];
                    d.style.display = _$_eb2b[40];
                    b.appendChild(d)
                }
            } catch (e) { }
            Object.defineProperty(this, _$_eb2b[41], {
                value: c,
                writable: !1,
                configurable: !1,
                enumerable: !1,
                extensible: !1
            });
            K().edition === _$_eb2b[31] && (this.edition = 1);
            return b
        },
        U = function () {
            var k = function () {
                if (this.edition) {
                    var d = this.edition.getAttribute(_$_eb2b[42]),
                        e = this.edition.getAttribute(_$_eb2b[43]);
                    ba.position.call(this, d, e)
                }
            },
                b = function () {
                    this.scrollX.scrollWidth < this.scrollX.scrollLeft + this.scrollX.offsetWidth + 25 ? (this.content.scrollLeft = this.content.scrollWidth, 0 < this.options.freezeColumns && ka.refresh.call(this)) : this.content.scrollLeft = 0;
                    k.call(this)
                },
                c = function () {
                    var d = this.tbody.lastChild.getAttribute(_$_eb2b[43]),
                        e = this.scrollY.scrollHeight - this.content.offsetHeight - this.scrollY.scrollTop;
                    this.content.scrollTop = d != (this.results ? this.rows[this.results[this.results.length - 1]].y : this.rows[this.rows.length -
                        1].y) && e ? 0 : this.content.scrollHeight;
                    k.call(this)
                },
                a = function () {
                    if (G.limited.call(this)) {
                        var d = ja.getWidth.call(this);
                        if (1 == this.parent.config.fullscreen) var e = this.content.offsetWidth,
                            f = this.content.offsetHeight;
                        else 1 == this.options.tableOverflow && (e = parseInt(this.options.tableWidth), f = this.options.pagination ? this.table.offsetHeight : parseInt(this.options.tableHeight));
                        this.width = a.width.call(this) + (d ? d : 1);
                        this.height = a.height.call(this);
                        this.tfoot.offsetHeight && (this.height += this.tfoot.offsetHeight +
                            26);
                        this.scrollX.firstChild.style.width = this.width + _$_eb2b[44];
                        this.scrollY.firstChild.style.height = this.height + _$_eb2b[44];
                        this.scrollX.style.width = e + _$_eb2b[44];
                        this.scrollY.style.height = f + _$_eb2b[44];
                        this.scrollX.style.display = this.width > e ? _$_eb2b[3] : _$_eb2b[40];
                        this.scrollY.style.display = this.height > f && !this.options.pagination ? _$_eb2b[3] : _$_eb2b[40]
                    } else this.scrollX.style.display = _$_eb2b[40], this.scrollY.style.display = _$_eb2b[40]
                };
            a.refresh = function () {
                this.scrollY.scrollTop = 0;
                this.scrollX.scrollLeft =
                    0;
                a.call(this)
            };
            a.updateX = function () {
                if (a.event) return !1;
                var d = parseInt(this.thead.lastChild.children[(this.options.freezeColumns || 0) + 1].getAttribute(_$_eb2b[42]));
                a.setX.call(this, a.width.call(this, d));
                b.call(this)
            };
            a.updateY = function () {
                if (a.event) return !1;
                if (this.tbody.firstChild) {
                    var d = parseInt(this.tbody.firstChild.getAttribute(_$_eb2b[43]));
                    a.setY.call(this, a.height.call(this, d));
                    c.call(this)
                }
            };
            a.setX = function (d) {
                a.ignore = !0;
                this.scrollX.scrollLeft = d
            };
            a.setY = function (d) {
                a.ignore = !0;
                this.scrollY.scrollTop =
                    d
            };
            a.update = function (d, e) {
                e == _$_eb2b[45] ? (d = a.width.call(this, null, d.target.scrollLeft / (d.target.scrollWidth - this.content.offsetWidth) * this.width), G.reset.call(this), G[_$_eb2b[46]].call(this, null, d), b.call(this)) : (d = a.height.call(this, null, d.target.scrollTop / (d.target.scrollHeight - this.content.offsetHeight) * (this.height - (this.tbody.offsetHeight - 5))), this.tbody.textContent = _$_eb2b[3], G[_$_eb2b[46]].call(this, d), c.call(this))
            };
            a.build = function (d) {
                var e = this,
                    f = document.createElement(_$_eb2b[20]),
                    g = document.createElement(_$_eb2b[20]);
                f.className = _$_eb2b[47] + d;
                g.className = _$_eb2b[48] + d;
                f.appendChild(g);
                d == _$_eb2b[45] ? f.style.height = _$_eb2b[22] : f.style.width = _$_eb2b[22];
                f.onscroll = function (h) {
                    if (a.ignore) return a.ignore = !1;
                    a.event = !0;
                    a.update.call(e, h, d);
                    a.event = !1
                };
                return f
            };
            a.width = function (d, e) {
                for (var f = this.options.columns, g = 0, h = this.options.freezeColumns || 0; h < this.options.columns.length; h++) {
                    if (d === h) return g;
                    !1 !== f[h].visible && f[h].type !== _$_eb2b[49] && (g += parseInt(f[h].width));
                    if (g >= e) return h
                }
                return e ? h : g
            };
            a.height = function (d,
                e) {
                var f = 0,
                    g = 0;
                this.options.defaultRowHeight || (this.options.defaultRowHeight = 23);
                for (var h = this.results ? this.results : this.rows, l = 0; l < h.length; l++) {
                    g = l;
                    this.results && (g = h[l]);
                    if (d === g) return f;
                    this.rows[g] && !1 !== this.rows[g].visible && (f = this.rows[g].height ? f + parseInt(this.rows[g].height) : this.rows[g].element && this.rows[g].element.offsetHeight ? f + parseInt(this.rows[g].element.offsetHeight) : f + parseInt(this.options.defaultRowHeight));
                    if (f >= e) return g
                }
                return e ? g : f
            };
            return a
        }(),
        G = function () {
            var k = function (c,
                a) {
                var d = this.thead.clientHeight;
                a || (a = 1);
                var e = !1,
                    f = !1,
                    g = c.getAttribute(_$_eb2b[42]),
                    h = c.getAttribute(_$_eb2b[43]);
                var l = this.records[h][g];
                l.merged && (g -= l.merged[0], h -= l.merged[1], c = this.records[h][g].element);
                h = this.content.getBoundingClientRect();
                l = c.getBoundingClientRect();
                var m = ja.getWidth.call(this),
                    p = h.left + m,
                    n = h.left + h.width;
                g = h.top + d;
                var t = h.top + h.height;
                var v = 0;
                h = l.top;
                h >= g && h <= t || (v++, e = !0);
                h = l.top + l.height;
                h >= g && h <= t || v++;
                if (v >= a) return 2 > a && (a = this.content.scrollTop, 0 == e ? t - g > l.height &&
                    (this.content.scrollTop += h - t + 3, U.ignore = !0, this.scrollY.scrollTop += h - t + 3) : (this.content.scrollTop = c.offsetTop - d, U.ignore = !0, this.scrollY.scrollTop -= a - this.content.scrollTop)), !1;
                if (!m || !c.classList.contains(_$_eb2b[50]))
                    if (v = 0, g = l.left, g >= p && g <= n || (v++, f = !0), g = l.left + l.width, g >= p && g < n || v++, v >= a) return 2 > a && (a = this.content.scrollLeft, 0 == f ? n - p > l.width && (this.content.scrollLeft += g - n + 3, U.ignore = !0, this.scrollX.scrollLeft += g - n + 3) : (this.content.scrollLeft = c.offsetLeft - p - 50, U.ignore = !0, this.scrollX.scrollLeft -=
                        a - this.content.scrollLeft)), !1;
                return !0
            },
                b = function () {
                    var c = 0;
                    this.results && (c = this.results[0]);
                    b.reset.call(this, !0);
                    b.renderX.call(this, 0);
                    b.renderY.call(this, c);
                    ja.headers.call(this);
                    0 < this.options.pagination && ua.update.call(this);
                    this.refreshBorders();
                    U.call(this)
                };
            b.limited = function () {
                return 1 == this.options.tableOverflow || 1 == this.parent.config.fullscreen
            };
            b.refresh = function () {
                if (!this.tbody.lastChild) return !1;
                var c = parseInt(this.thead.lastChild.lastChild.getAttribute(_$_eb2b[42])) + 1,
                    a = parseInt(this.tbody.lastChild.getAttribute(_$_eb2b[43]));
                this.results ? (a = this.results.indexOf(a), a < this.results.length - 1 && a++, a = this.results[a]) : a++;
                b.renderX.call(this, c);
                b.renderY.call(this, a);
                this.content.scrollTop = 0;
                this.content.scrollLeft = 0;
                b.adjustX.call(this, !1, !0);
                b.adjustY.call(this, !1, !0);
                U.call(this)
            };
            b.renderX = function (c, a, d) {
                var e = this.options.columns,
                    f, g = 0,
                    h = [],
                    l = (this.options.freezeColumns || 0) + 1;
                1 == this.parent.config.fullscreen ? g = this.content.offsetWidth : 1 == this.options.tableOverflow && (g = parseInt(this.options.tableWidth));
                if (0 < this.options.freezeColumns) {
                    for (var m =
                        f = 0; m < this.options.freezeColumns; m++) f += parseInt(this.options.columns[m].width);
                    g < f && (g = f + 100)
                }
                f = this.headerContainer.offsetWidth || 0;
                for (d || (d = 0); e[c] && (!g || f < g || 0 < d);) !1 !== e[c].visible && e[c].type !== _$_eb2b[49] && (f += parseInt(e[c].width)), 0 < d && d--, m = ca.getWidth.call(this, c, h), m > d && (d = m), a ? (b.renderColumn.call(this, c, l), c--) : (b.renderColumn.call(this, c), c++);
                ca.batch.call(this, h)
            };
            b.renderY = function (c, a, d) {
                var e = 0,
                    f = [];
                1 == this.parent.config.fullscreen ? e = this.content.offsetHeight : 1 == this.options.tableOverflow &&
                    (e = 0 < this.options.pagination ? 0 : parseInt(this.options.tableHeight));
                e && (e -= this.thead.offsetHeight + 4, this.tfoot.offsetHeight && (e -= this.tfoot.offsetHeight + 14));
                var g = this.tbody.offsetHeight;
                var h = this.options.freezeColumns || 0,
                    l = parseInt(this.headerContainer.lastChild.getAttribute(_$_eb2b[42]));
                h = this.headerContainer.children[h + 1] ? parseInt(this.headerContainer.children[h + 1].getAttribute(_$_eb2b[42])) : l;
                var m = this.results ? this.results.indexOf(c) : c,
                    p = parseInt(this.options.pagination) || 0,
                    n = this.tbody.children.length ||
                        0,
                    t = !1;
                for (d || (d = 0); this.options.data && this.options.data[c] && (!p || n < p || 0 < d) /*&& (!e || g <= e || 0 < d)*/;) {
                    var v = b.renderRow.call(this, c, h, l);
                    a ? (this.tbody.insertBefore(v, this.tbody.firstChild), m--) : (this.tbody.appendChild(v), m++);
                    this.rows[c].height ? (v.offsetHeight !== this.rows[c].height && (this.rows[c].height = v.offsetHeight, t = !0), g += this.rows[c].height) : (g = (t = v.offsetHeight) ? g + (this.rows[c].height = t) : g + (this.options.defaultRowHeight || 23), t = !0);
                    0 < d && d--;
                    c = ca.getHeight.call(this, c, f);
                    c > d && (d = c);
                    c = this.results ?
                        this.results[m] : m;
                    n++
                }
                ca.batch.call(this, f);
                t && U.call(this)
            };
            b.renderColumn = function (c, a, d) {
                this.headers[c] || Ca.create.call(this, c);
                if (this.options.nestedHeaders)
                    for (d = 0; d < this.options.nestedHeaders.length; d++) {
                        var e = oa.getColumns.call(this, d);
                        void 0 !== e[c] && oa.renderCell.call(this, e[c], d, a)
                    }
                if (this.options.footers)
                    for (d = 0; d < this.options.footers.length; d++) e = aa.create.call(this, c, d), null === a ? this.tfoot.children[d].appendChild(e) : this.tfoot.children[d].insertBefore(e, this.tfoot.children[d].children[a]);
                null === a ? (this.headerContainer.appendChild(this.headers[c]), this.colgroupContainer.appendChild(this.colgroup[c])) : (this.headerContainer.insertBefore(this.headers[c], this.headerContainer.children[a]), this.colgroupContainer.insertBefore(this.colgroup[c], this.colgroupContainer.children[a]));
                if (this.tbody.children.length)
                    for (d = 0; d < this.tbody.children.length; d++) e = parseInt(this.tbody.children[d].getAttribute(_$_eb2b[43])), (e = N[_$_eb2b[51]].call(this, c, e)) && (null === a ? this.tbody.children[d].appendChild(e) :
                        this.tbody.children[d].insertBefore(e, this.tbody.children[d].children[a]))
            };
            b.renderRow = function (c, a, d, e) {
                var f = this.options.freezeColumns || 0;
                null == a && (a = parseInt(this.headerContainer.children[f + 1].getAttribute(_$_eb2b[42])));
                null == d && (d = parseInt(this.headerContainer.lastChild.getAttribute(_$_eb2b[42])));
                var g = Aa.create.call(this, c, e);
                if (g.children.length)
                    for (; g.children[f + 1];) g.removeChild(g.lastChild);
                if (0 < f) {
                    for (var h = 0; h < f; h++) g.appendChild(N[_$_eb2b[51]].call(this, h, c));
                    ja.update.call(this, c)
                }
                for (h =
                    a; h <= d; h++) a = N[_$_eb2b[51]].call(this, h, c, e), g.appendChild(a);
                return g
            };
            b.resetY = function () {
                if (this.options.pagination) {
                    this.pageNumber || (this.pageNumber = 0);
                    var c = this.options.pagination * this.pageNumber
                } else c = 0;
                var a = 0;
                if (this.merged.rows[c]) {
                    for (a = this.options.pagination; this.merged.rows[c] && 0 <= c;) c--;
                    c++
                }
                this.tbody.textContent = _$_eb2b[3];
                this.results && this.results.length && (c = this.results[c]);
                (!this.results || 0 < this.results.length) && G.renderY.call(this, c, 0, a);
                0 < this.options.pagination && ua.update.call(this);
                U.call(this);
                this.refreshBorders()
            };
            b[_$_eb2b[46]] = function (c, a) {
                if (null !== c && 0 <= c)
                    if (0 < this.options.pagination) this.page(this.whichPage(c));
                    else if (this.rows[c] || (c = this.options.data.length - 1), !Aa.attached.call(this, c)) {
                        if (this.merged.rows[c]) {
                            for (; this.merged.rows[c] && 0 <= c;) c--;
                            c++
                        }
                        this.tbody.textContent = _$_eb2b[3];
                        b.renderY.call(this, c);
                        var d = this.results ? this.results.indexOf(c) : c;
                        0 < d && (this.results ? d = this.results[d - 1] : d--, b.renderY.call(this, d, !0));
                        U.updateY.call(this)
                    }
                if (null !== a && 0 <= a && (this.options.columns[a] ||
                    (a = this.options.columns.length - 1), c || (c = b.getY.call(this)), !N.attached.call(this, a, c))) {
                    if (this.merged.cols[a]) {
                        for (d = c = 0; d < this.rows.length; d++) this.records[d][a].merged && (c = this.records[d][a].merged[0]);
                        a -= c
                    }
                    b.reset.call(this);
                    b.renderX.call(this, a);
                    0 < a && b.renderX.call(this, a - 1, !0);
                    U.updateX.call(this)
                }
                this.refreshBorders()
            };
            b.pageUp = function (c, a, d) {
                var e = parseInt(this.tbody.firstChild.getAttribute(_$_eb2b[43]));
                if (a) {
                    c = null;
                    var f = this.results ? this.results[0] : 0
                } else this.results ? (f = this.results.indexOf(e),
                    0 < f && f--, f = this.results[f]) : (f = e, 0 < f && f--);
                if (f < e) {
                    if (null == c) this.tbody.textContent = _$_eb2b[3];
                    else {
                        e = c;
                        if (this.options.mergeCells) {
                            var g = parseInt(this.tbody.lastChild.getAttribute(_$_eb2b[43]));
                            e += ca.getHeight.call(this, g)
                        }
                        //for (g = 0; g < e; g++) this.tbody.removeChild(this.tbody.lastChild)
                    }
                    b.renderY.call(this, f, !a, c);
                    b.adjustY.call(this, !1);
                    d && d.preventDefault();
                    U.updateY.call(this)
                }
                this.refreshBorders()
            };
            b.pageDown = function (c, a, d) {
                var e = parseInt(this.tbody.lastChild.getAttribute(_$_eb2b[43]));
                if (a) {
                    c =
                        null;
                    var f = this.results ? this.results[this.results.length - 1] : this.rows.length - 1
                } else this.results ? (f = this.results.indexOf(e), f < this.results.length - 1 && f++, f = this.results[f]) : (f = e, e < this.options.data.length - 1 && f++);
                if (f > e) {
                    if (null == c) this.tbody.textContent = _$_eb2b[3];
                    else {
                        e = c;
                        if (this.options.mergeCells) {
                            var g = parseInt(this.tbody.firstChild.getAttribute(_$_eb2b[43]));
                            e += ca.getHeight.call(this, g)
                        }
                        //for (g = 0; g < e; g++) this.tbody.removeChild(this.tbody.firstChild)
                    }
                    b.renderY.call(this, f, a, c);
                    b.adjustY.call(this,
                        !0);
                    d && d.preventDefault();
                    U.updateY.call(this)
                }
                this.refreshBorders()
            };
            b.removeColumns = function (c, a) {
                var d = null === a ? this.headerContainer.lastChild : this.headerContainer.children[a];
                if (d) d = parseInt(d.getAttribute(_$_eb2b[42]));
                else return !1;
                if (this.options.mergeCells) {
                    for (var e = 0, f = d, g = 0; g < c; g++) {
                        var h = ca.getWidth.call(this, f);
                        h > e && (e = h);
                        null === a ? f-- : f++
                    }
                    c += e
                }
                c >= this.headerContainer.children.length && (c = this.headerContainer.children.length - 1);
                if ((e = this.options.nestedHeaders) && e.length)
                    for (f = 0; f < e.length; f++)
                        for (h =
                            oa.getColumns.call(this, f), g = 0; g < c; g++)
                            if (void 0 !== h[d + g] && this.nested.content[f][h[d + g]]) {
                                var l = this.nested.content[f][h[d + g]].element,
                                    m = l.getAttribute(_$_eb2b[52]);
                                0 < m && m--;
                                0 < m ? l.setAttribute(_$_eb2b[52], m) : l.parentNode && this.thead.children[f].removeChild(l)
                            } for (g = 0; g < c; g++) null === a ? (this.headerContainer.removeChild(this.headerContainer.lastChild), this.colgroupContainer.removeChild(this.colgroupContainer.lastChild)) : this.headerContainer.children[a] && (this.headerContainer.removeChild(this.headerContainer.children[a]),
                                this.colgroupContainer.removeChild(this.colgroupContainer.children[a]));
                for (f = 0; f < this.tbody.children.length; f++)
                    for (g = 0; g < c; g++) null === a ? this.tbody.children[f].removeChild(this.tbody.children[f].lastChild) : this.tbody.children[f].children[a] && this.tbody.children[f].removeChild(this.tbody.children[f].children[a]);
                for (f = 0; f < this.tfoot.children.length; f++)
                    for (g = 0; g < c; g++) null === a ? this.tfoot.children[f].removeChild(this.tfoot.children[f].lastChild) : this.tfoot.children[f].removeChild(this.tfoot.children[f].children[a])
            };
            b.pageLeft = function (c, a) {
                var d = this.options.freezeColumns || 0;
                if (a) {
                    c = null;
                    var e = d
                } else e = parseInt(this.thead.lastChild.children[d + 1].getAttribute(_$_eb2b[42])) - 1;
                e >= d && (null == c ? b.reset.call(this) : b.removeColumns.call(this, c, null), b.renderX.call(this, e, !a, c), b.adjustX.call(this, !1), U.updateX.call(this));
                this.refreshBorders()
            };
            b.pageRight = function (c, a) {
                var d = this.options.freezeColumns || 0;
                if (a) {
                    c = null;
                    var e = this.options.columns.length - 1
                } else e = parseInt(this.thead.lastChild.lastChild.getAttribute(_$_eb2b[42])) +
                    1;
                e < this.options.columns.length && (null == c ? b.reset.call(this) : b.removeColumns.call(this, c, d + 1), b.renderX.call(this, e, a, c), b.adjustX.call(this, !0), U.updateX.call(this));
                this.refreshBorders()
            };
            b.reset = function (c) {
                1 < this.headerContainer.children.length && (c = c ? 0 : this.options.freezeColumns || 0, b.removeColumns.call(this, this.headerContainer.children.length - (c + 1), c + 1))
            };
            b.getX = function () {
                if (this.thead.lastChild) return parseInt(this.thead.lastChild.children[1].getAttribute(_$_eb2b[42]))
            };
            b.getY = function () {
                if (this.tbody.firstChild) return parseInt(this.tbody.firstChild.getAttribute(_$_eb2b[43]))
            };
            b.adjustX = function (c, a) {
                var d = this.options.freezeColumns || 0;
                if (!0 === a) {
                    for (var e = a = 0; e < this.tbody.children.length; e++)
                        if (this.tbody.children[e].style.display !== _$_eb2b[40] && this.tbody.children[e].children[1] && k.call(this, this.tbody.children[e].children[1], 2)) {
                            a = e;
                            break
                        } e = [];
                    for (var f = d + 1; f < this.thead.lastChild.children.length; f++) this.tbody.children[a].children[f] && (k.call(this, this.tbody.children[a].children[f], 2) || e.push(f));
                    e.length && b.removeColumns.call(this, e.length, e[0])
                }
                d = c ? parseInt(this.tbody.firstChild.children[d +
                    1].getAttribute(_$_eb2b[42])) : parseInt(this.tbody.firstChild.lastChild.getAttribute(_$_eb2b[42]));
                c ? d-- : d++;
                b.renderX.call(this, d, c)
            };
            b.adjustY = function (c, a) {
                if (!0 === a) {
                    a = [];
                    for (var d = 0; d < this.tbody.children.length; d++) this.tbody.children[d].style.display !== _$_eb2b[40] && this.tbody.children[d].children[1] && (k.call(this, this.tbody.children[d].children[1], 2) || a.push(this.tbody.children[d].firstChild.getAttribute(_$_eb2b[43])));
                    if (a.length)
                        for (d = 0; d < a.length; d++) this.rows[a[d]].element.remove()
                }
                a = c ? parseInt(this.tbody.firstChild.getAttribute(_$_eb2b[43])) :
                    parseInt(this.tbody.lastChild.getAttribute(_$_eb2b[43]));
                this.results ? (a = this.results.indexOf(a), a = c ? this.results[a - 1] : this.results[a + 1]) : c ? a-- : a++;
                b.renderY.call(this, a, c)
            };
            b.isVisible = k;
            return b
        }(),
        Ma = function (k, b, c, a) {
            for (var d = this.options.columns, e = k, f, g = null; k < d.length - 1;)
                if (k++, !1 !== d[k].visible && d[k].type !== _$_eb2b[49]) {
                    f = this.records[b] && this.records[b][k].v === _$_eb2b[3] ? !1 : !0;
                    null === g && (g = f);
                    if (this.records[b] && this.records[b][k].element && this.records[b][k].element.style.display == _$_eb2b[40]) {
                        if (this.records[b][k].merged) {
                            if (k ==
                                k + this.records[b][k].merged[0] && (e = k, !c)) break;
                            f = !1
                        }
                    } else {
                        if (c && f !== g && !a) break;
                        if (c) e = k;
                        else return k
                    }
                    g = f
                } return e
        },
        Na = function (k, b, c, a) {
            for (var d = this.options.columns, e = k, f, g = null; 0 < k;)
                if (k--, !1 !== d[k].visible && d[k].type !== _$_eb2b[49]) {
                    f = this.records[b] && this.records[b][k].v === _$_eb2b[3] ? !1 : !0;
                    null === g && (g = f);
                    if (this.records[b] && this.records[b][k].element && this.records[b][k].element.style.display == _$_eb2b[40]) {
                        if (this.records[b][k].merged) {
                            e = k - this.records[b][k].merged[0];
                            if (!c) break;
                            f = !0
                        }
                    } else {
                        if (c &&
                            f !== g && !a) break;
                        if (c) e = k;
                        else return k
                    }
                    g = f
                } return e
        },
        Ta = function () {
            var k = function (c, a, d, e) {
                1 == c || 3 == c ? Aa.attached.call(this, d) || (0 < this.options.pagination ? this.page(this.whichPage(d)) : (1 == c ? G.pageUp.call(this, 1, e) : G.pageDown.call(this, 1, e), Aa.attached.call(this, d) || G[_$_eb2b[46]].call(this, d))) : N.attached.call(this, a, d) || (0 == c ? G.pageLeft.call(this, 1, e) : G.pageRight.call(this, 1, e), N.attached.call(this, a, d) || G[_$_eb2b[46]].call(this, d, a))
            },
                b = function (c) {
                    c.up = b.up;
                    c.down = b.down;
                    c.right = b.right;
                    c.left =
                        b.left;
                    c.last = b.last;
                    c.first = b.first
                };
            b.up = function (c, a, d) {
                if (c) var e = parseInt(this.selectedCell[2]),
                    f = parseInt(this.selectedCell[3]);
                else e = parseInt(this.selectedCell[0]), f = parseInt(this.selectedCell[1]);
                a: {
                    var g = e,
                        h = f,
                        l = h,
                        m = null;
                    if (this.results && (h = this.results.indexOf(h), -1 == h)) {
                        d = l;
                        break a
                    }
                    for (; 0 < h;) {
                        h--;
                        var p = this.results ? this.results[h] : h;
                        if (!1 !== this.rows[p].visible) {
                            var n = this.records[p][g].v === _$_eb2b[3] ? !1 : !0;
                            null === m && (m = n);
                            if (this.records[p][g].element && this.records[p][g].element.style.display ==
                                _$_eb2b[40]) {
                                if (this.records[p][g].merged) {
                                    l = p - this.records[p][g].merged[1];
                                    if (!a) break;
                                    n = !0
                                }
                            } else if (a && n !== m && !d) {
                                d = l;
                                break a
                            } else if (a) l = this.records[p][g].merged ? p - this.records[p][g].merged[1] : p;
                            else {
                                d = p;
                                break a
                            }
                            m = n
                        }
                    }
                    d = l
                }
                d != f && (f = d, c ? (c = this.selectedCell[0], d = this.selectedCell[1]) : (c = e, d = f, e = c, f = d), k.call(this, 1, e, f, a), this.updateSelectionFromCoords(c, d, e, f))
            };
            b.down = function (c, a, d) {
                if (c) var e = parseInt(this.selectedCell[2]),
                    f = parseInt(this.selectedCell[3]);
                else e = parseInt(this.selectedCell[0]),
                    f = parseInt(this.selectedCell[1]);
                a: {
                    var g = e,
                        h = f,
                        l = h,
                        m = null;
                    if (this.results) {
                        h = this.results.indexOf(h);
                        if (-1 == h) {
                            d = l;
                            break a
                        }
                        var p = this.results.length - 1
                    } else p = this.rows.length - 1;
                    for (; h < p;) {
                        h++;
                        var n = this.results ? this.results[h] : h;
                        if (!1 !== this.rows[n].visible) {
                            var t = this.records[n][g].v === _$_eb2b[3] ? !1 : !0;
                            null === m && (m = t);
                            if (this.records[n][g].element && this.records[n][g].element.style.display == _$_eb2b[40]) {
                                if (this.records[n][g].merged) {
                                    if (!this.records[n][g].merged[1] && (l = n, !a)) break;
                                    t = !0
                                }
                            } else if (a &&
                                t !== m && !d) {
                                d = l;
                                break a
                            } else if (a) l = n;
                            else {
                                d = n;
                                break a
                            }
                            m = t
                        }
                    }
                    d = l
                }
                d != f && (f = d, c ? (c = this.selectedCell[0], d = this.selectedCell[1]) : (c = e, d = f, e = c, f = d), k.call(this, 3, e, f, a), this.updateSelectionFromCoords(c, d, e, f))
            };
            b.right = function (c, a, d) {
                if (c) var e = parseInt(this.selectedCell[2]),
                    f = parseInt(this.selectedCell[3]);
                else e = parseInt(this.selectedCell[0]), f = parseInt(this.selectedCell[1]);
                d = Ma.call(this, e, f, a, d);
                d != e && (e = d, c ? (c = this.selectedCell[0], d = this.selectedCell[1]) : (c = e, d = f, e = c, f = d), k.call(this, 2, e, f, a), this.updateSelectionFromCoords(c,
                    d, e, f))
            };
            b.left = function (c, a, d) {
                if (c) var e = parseInt(this.selectedCell[2]),
                    f = parseInt(this.selectedCell[3]);
                else e = parseInt(this.selectedCell[0]), f = parseInt(this.selectedCell[1]);
                d = Na.call(this, e, f, a, d);
                d != e && (e = d, c ? (c = this.selectedCell[0], d = this.selectedCell[1]) : (c = e, d = f, e = c, f = d), k.call(this, 0, e, f, a), this.updateSelectionFromCoords(c, d, e, f))
            };
            b.first = function (c, a) {
                a ? b.up.call(this, c, !0, !0) : b.left.call(this, c, !0, !0)
            };
            b.last = function (c, a) {
                a ? b.down.call(this, c, !0, !0) : b.right.call(this, c, !0, !0)
            };
            return b
        }(),
        Ua = function () {
            return function (k) {
                k.save = Oa;
                k.fullscreen = Ka;
                k.setReadOnly = Ga;
                k.getReadOnly = Ga;
                k.isReadOnly = Ga;
                k.setPlugins = function (b) {
                    pa.call(this.parent, b)
                };
                k.showIndex = function () {
                    ya.call(this, !0)
                };
                k.hideIndex = function () {
                    ya.call(this, !1)
                };
                k[_$_eb2b[46]] = G[_$_eb2b[46]];
                k.undo = function () {
                    T.undo.call(k.parent)
                };
                k.redo = function () {
                    T.redo.call(k.parent)
                };
                k.isEditable = function () {
                    return Q.call(k.parent, k)
                };
                k.dispatch = function () {
                    return F.apply(this.parent, arguments)
                };
                k.setViewport = function (b, c) {
                    this.options.tableOverflow = !0;
                    b = parseInt(b);
                    c = parseInt(c);
                    this.content.style.overflow = _$_eb2b[49];
                    100 < b && (this.options.tableWidth = b + _$_eb2b[44], this.content.style.maxWidth = this.options.tableWidth);
                    100 < c && (this.options.tableHeight = parseInt(c) + _$_eb2b[44], this.content.style.maxHeight = this.options.tableHeight);
                    G.refresh.call(this);
                    fa.update.call(this.parent, this)
                };
                k.helpers = E;
                k.viewport = G
            }
        }(),
        Da = function () {
            var k = {
                logo: null,
                url: null,
                persistence: !1,
                sequence: !0,
                data: null,
                json: null,
                rows: [],
                columns: [],
                cells: {},
                role: null,
                nestedHeaders: null,
                defaultColWidth: 100,
                defaultRowHeight: 0,
                defaultColAlign: null,
                minSpareRows: 0,
                minSpareCols: 0,
                minDimensions: [0, 0],
                csv: null,
                csvFileName: _$_eb2b[36],
                csvHeaders: !0,
                csvDelimiter: _$_eb2b[25],
                columnSorting: !0,
                columnDrag: !0,
                columnResize: !0,
                rowResize: !0,
                rowDrag: !0,
                editable: !0,
                allowInsertRow: !0,
                allowManualInsertRow: !0,
                allowInsertColumn: !0,
                allowManualInsertColumn: !0,
                allowDeleteRow: !0,
                allowDeletingAllRows: !1,
                allowDeleteColumn: !0,
                allowRenameColumn: !0,
                allowComments: !0,
                selectionCopy: !0,
                mergeCells: {},
                search: !1,
                pagination: !1,
                paginationOptions: null,
                textOverflow: !1,
                tableOverflow: !1,
                tableHeight: _$_eb2b[53],
                tableWidth: null,
                comments: null,
                meta: null,
                style: {},
                freezeColumns: 0,
                orderBy: null,
                worksheetId: _$_eb2b[3],
                worksheetName: null,
                worksheetState: null,
                filters: !1,
                footers: null,
                formify: null,
                applyMaskOnFooters: !1,
                pluginOptions: null,
                locked: !1,
                selectUnLockedCells: !0,
                selectLockedCells: !0,
                wrap: !1
            },
                b = {
                    application: _$_eb2b[54],
                    cloud: null,
                    root: null,
                    definedNames: {},
                    sorting: null,
                    server: null,
                    toolbar: null,
                    editable: !0,
                    allowExport: !0,
                    includeHeadersOnDownload: !1,
                    forceUpdateOnPaste: !1,
                    loadingSpin: !1,
                    fullscreen: !1,
                    secureFormulas: !0,
                    parseFormulas: !0,
                    debugFormulas: !1,
                    editorFormulas: !0,
                    autoIncrement: !0,
                    autoCasting: !0,
                    stripHTML: !1,
                    tabs: !1,
                    allowDeleteWorksheet: !0,
                    allowRenameWorksheet: !0,
                    allowMoveWorksheet: !0,
                    moveDownOnEnter: !0,
                    onevent: null,
                    onclick: null,
                    onload: null,
                    onundo: null,
                    onredo: null,
                    onbeforesave: null,
                    onsave: null,
                    onbeforechange: null,
                    onchange: null,
                    onafterchanges: null,
                    oncopy: null,
                    onbeforepaste: null,
                    onpaste: null,
                    onbeforeinsertrow: null,
                    oninsertrow: null,
                    onbeforedeleterow: null,
                    ondeleterow: null,
                    onbeforeinsertcolumn: null,
                    oninsertcolumn: null,
                    onbeforedeletecolumn: null,
                    ondeletecolumn: null,
                    onmoverow: null,
                    onmovecolumn: null,
                    onresizerow: null,
                    onresizecolumn: null,
                    onselection: null,
                    onbeforecomments: null,
                    oncomments: null,
                    onbeforesort: null,
                    onsort: null,
                    onfocus: null,
                    onblur: null,
                    onmerge: null,
                    onchangeheader: null,
                    onchangefooter: null,
                    onchangefootervalue: null,
                    onchangenested: null,
                    onchangenestedcell: null,
                    oncreateeditor: null,
                    oneditionstart: null,
                    oneditionend: null,
                    onchangestyle: null,
                    onchangemeta: null,
                    onbeforechangepage: null,
                    onchangepage: null,
                    onbeforecreateworksheet: null,
                    oncreateworksheet: null,
                    onrenameworksheet: null,
                    ondeleteworksheet: null,
                    onmoveworksheet: null,
                    onopenworksheet: null,
                    onchangerowid: null,
                    onbeforesearch: null,
                    onsearch: null,
                    onbeforefilter: null,
                    onfilter: null,
                    onopenfilter: null,
                    oncreatecell: null,
                    oncreaterow: null,
                    oncreatecolumn: null,
                    onbeforeformula: null,
                    onformulachain: null,
                    onbeforesend: null,
                    updateTable: null,
                    contextMenu: null,
                    parseTableFirstRowAsHeader: !1,
                    parseTableAutoCellType: !1,
                    plugins: null,
                    about: !0,
                    license: null,
                    worksheets: null,
                    validations: null
                },
                c = function (e, f) {
                    f = JSON.parse(JSON.stringify(f));
                    var g = {},
                        h;
                    for (h in f) e && e.hasOwnProperty(h) ? g[h] = e[h] : g[h] = f[h];
                    return g
                },
                a = function (e) {
                    e && (typeof e == _$_eb2b[17] && (e = JSON.parse(e)), this.options.data = e);
                    this.options.json && (this.options.data = this.options.json);
                    this.options.data || (this.options.data = []);
                    e = this.options.columns.length;
                    if (this.options.data && typeof this.options.data[0] !== _$_eb2b[13]) {
                        var f = this.options.data[0].data && 0 < Object.keys(this.options.data[0].data).length ?
                            Object.keys(this.options.data[0].data) : Object.keys(this.options.data[0]);
                        f.length > e && (e = f.length)
                    }
                    this.options.minDimensions[0] > e && (e = this.options.minDimensions[0]);
                    e || (e = 8);
                    for (var g = 0; g < e; g++) this.options.columns[g] ? this.options.columns[g].type || (this.options.columns[g].type = _$_eb2b[55]) : this.options.columns[g] = {
                        type: _$_eb2b[55]
                    }, this.options.columns[g].type == _$_eb2b[56] && (this.options.columns[g].type = _$_eb2b[57], this.options.columns[g].autocomplete = !0), !this.options.columns[g].name && f && f[g] && Number(f[g]) !=
                    f[g] && (this.options.columns[g].name = f[g]), this.options.columns[g].width || (this.options.columns[g].width = this.options.defaultColWidth), this.options.columns[g].type != _$_eb2b[57] || this.options.columns[g].source || (this.options.columns[g].source = []), this.options.columns[g].type == _$_eb2b[57] && this.options.columns[g].url && jSuites.ajax({
                        url: this.options.columns[g].url,
                        index: g,
                        method: _$_eb2b[58],
                        dataType: _$_eb2b[59],
                        worksheet: this,
                        group: this.parent.name,
                        success: function (h) {
                            this.worksheet.options.columns[this.index].source =
                                h
                        }
                    })
                },
                d = function (e) {
                    e.getConfig = d[_$_eb2b[51]];
                    e.setConfig = d[_$_eb2b[11]]
                };
            d.spreadsheet = function (e) {
                this.config = c(e, b);
                1 != this.config.secureFormulas && console.log(_$_eb2b[60])
            };
            d.worksheet = function (e) {
                var f = this;
                f.options = c(e, k);
                f.options.csv ? jSuites.ajax({
                    url: f.options.csv,
                    method: _$_eb2b[58],
                    dataType: _$_eb2b[55],
                    group: f.parent.name,
                    success: function (g) {
                        g = E.parseCSV(g, f.options.csvDelimiter);
                        if (1 == f.options.csvHeaders && 0 < g.length)
                            for (var h = g.shift(), l = 0; l < h.length; l++) f.options.columns[l] || (f.options.columns[l] = {
                                type: _$_eb2b[55],
                                width: f.options.defaultColWidth
                            }), typeof f.options.columns[l].title === _$_eb2b[13] && (f.options.columns[l].title = h[l]);
                        a.call(f, g)
                    }
                }) : f.options.url ? jSuites.ajax({
                    url: f.options.url,
                    method: _$_eb2b[58],
                    dataType: _$_eb2b[59],
                    group: f.parent.name,
                    success: function (g) {
                        a.call(f, g)
                    }
                }) : a.call(f)
            };
            d[_$_eb2b[51]] = function () {
                return this.options
            };
            d[_$_eb2b[11]] = function (e) {
                typeof e == _$_eb2b[17] && (e = JSON.parse(e));
                if (this.parent) {
                    f = Object.keys(e);
                    for (g = 0; g < f.length; g++) typeof k[f[g]] !== _$_eb2b[13] &&
                        (this.options[f[g]] = e[f[g]]);
                    !0 === e.filters ? da.show.call(this) : !1 === e.filters && da.hide.call(this);
                    !0 === e.search ? qa.show.call(this) : !1 === e.search && qa.hide.call(this);
                    !0 === e.toolbar ? fa.show.call(this.parent) : !1 === e.toolbar && fa.hide.call(this.parent);
                    e.minDimensions && e.minDimensions[0] && (f = parseInt(e.minDimensions[0]) - this.headers.length, 0 < f && this.insertColumn(f), f = parseInt(e.minDimensions[1]) - this.rows.length, 0 < f && this.insertRow(f));
                    J.call(this, _$_eb2b[61], {
                        data: JSON.stringify(e)
                    })
                } else
                    for (var f = Object.keys(e),
                        g = 0; g < f.length; g++) typeof b[f[g]] !== _$_eb2b[13] && (this.config[f[g]] = e[f[g]])
            };
            return d
        }(),
        T = function (k) {
            k = function (b) {
                if (1 != this.ignoreHistory) {
                    var c = ++this.historyIndex;
                    this.history = this.history.slice(0, c + 1);
                    this.history[c] = b
                }
            };
            k.undo = function () {
                if (0 <= this.historyIndex) var b = this.history[this.historyIndex--];
                if (b) {
                    this.ignoreHistory = !0;
                    var c = b.worksheet;
                    c.openWorksheet();
                    if (b.action == _$_eb2b[62]) c.deleteRow(b.insertBefore ? b.rowNumber : b.rowNumber + 1, b.numOfRows);
                    else if (b.action == _$_eb2b[63]) c.insertRow(b.numOfRows,
                        b.rowNumber, 1, b.data), N.setAttributes.call(c, b.attributes);
                    else if (b.action == _$_eb2b[64]) c.deleteColumn(b.insertBefore ? b.columnNumber : b.columnNumber + 1, b.numOfColumns);
                    else if (b.action == _$_eb2b[65]) c.insertColumn(b.numOfColumns, b.columnNumber, 1, b.properties, b.data, b.extra), N.setAttributes.call(c, b.attributes);
                    else if (b.action == _$_eb2b[66]) c.moveRow(b.newValue, b.oldValue);
                    else if (b.action == _$_eb2b[67]) c.moveColumn(b.newValue, b.oldValue);
                    else if (b.action == _$_eb2b[68]) c.removeMerge(b.newValue, !0), c.setMerge(b.oldValue);
                    else if (b.action == _$_eb2b[69]) c.setMerge(b.oldValue);
                    else if (b.action == _$_eb2b[70]) c.setStyle(b.oldValue, null, null, 1);
                    else if (b.action == _$_eb2b[71]) c.setStyle(b.oldValue);
                    else if (b.action == _$_eb2b[72]) c.setWidth(b.column, b.oldValue);
                    else if (b.action == _$_eb2b[73]) c.setHeight(b.row, b.oldValue);
                    else if (b.action == _$_eb2b[74]) c.setHeader(b.column, b.oldValue);
                    else if (b.action == _$_eb2b[75]) c.setComments(b.oldValue);
                    else if (b.action == _$_eb2b[76]) c.orderBy(b.column, b.direction ? 0 : 1, b.oldValue);
                    else if (b.action ==
                        _$_eb2b[77] || b.action == _$_eb2b[78] || b.action == _$_eb2b[79]) {
                        var a = [];
                        for (var d = 0; d < b.records.length; d++) {
                            var e = {
                                x: b.records[d].x,
                                value: b.records[d].oldValue
                            };
                            void 0 !== b.records[d].y && (e.y = b.records[d].y);
                            b.records[d].oldStyle && (e.style = b.records[d].oldStyle);
                            a.push(e)
                        }
                        c[b.action](a)
                    } else b.action == _$_eb2b[80] ? c.renameWorksheet(b.index, b.oldValue) : b.action == _$_eb2b[81] && c.moveWorksheet(b.t, b.f);
                    b.selection && c.updateSelectionFromCoords(b.selection);
                    this.ignoreHistory = !1;
                    F.call(this, _$_eb2b[82], c, b)
                }
            };
            k.redo = function () {
                if (this.historyIndex < this.history.length - 1) var b = this.history[++this.historyIndex];
                if (b) {
                    this.ignoreHistory = !0;
                    var c = b.worksheet;
                    c.openWorksheet();
                    if (b.action == _$_eb2b[62]) c.insertRow(b.numOfRows, b.rowNumber, b.insertBefore, b.data);
                    else if (b.action == _$_eb2b[63]) c.deleteRow(b.rowNumber, b.numOfRows);
                    else if (b.action == _$_eb2b[64]) c.insertColumn(b.numOfColumns, b.columnNumber, b.insertBefore, b.properties, b.data);
                    else if (b.action == _$_eb2b[65]) c.deleteColumn(b.columnNumber, b.numOfColumns);
                    else if (b.action == _$_eb2b[66]) c.moveRow(b.oldValue, b.newValue);
                    else if (b.action == _$_eb2b[67]) c.moveColumn(b.oldValue, b.newValue);
                    else if (b.action == _$_eb2b[68]) c.setMerge(b.newValue);
                    else if (b.action == _$_eb2b[69]) c.removeMerge(b.newValue);
                    else if (b.action == _$_eb2b[70]) c.setStyle(b.newValue, null, null, 1);
                    else if (b.action == _$_eb2b[71]) c.resetStyle(b.cells);
                    else if (b.action == _$_eb2b[72]) c.setWidth(b.column, b.newValue);
                    else if (b.action == _$_eb2b[73]) c.setHeight(b.row, b.newValue);
                    else if (b.action == _$_eb2b[74]) c.setHeader(b.column,
                        b.newValue);
                    else if (b.action == _$_eb2b[75]) c.setComments(b.newValue);
                    else if (b.action == _$_eb2b[76]) c.orderBy(b.column, b.direction, b.newValue);
                    else if (b.action == _$_eb2b[77] || b.action == _$_eb2b[78] || b.action == _$_eb2b[79]) c[b.action](b.records);
                    else b.action == _$_eb2b[80] ? c.renameWorksheet(b.index, b.newValue) : b.action == _$_eb2b[81] && c.moveWorksheet(b.f, b.t);
                    b.selection && c.updateSelectionFromCoords(b.selection);
                    this.ignoreHistory = !1;
                    F.call(this, _$_eb2b[83], c, b)
                }
            };
            return k
        }(),
        pa = function () {
            var k = function (b) {
                var c =
                    null;
                if (Array.isArray(b)) c = b;
                else if (typeof b == _$_eb2b[84]) {
                    c = [];
                    var a = Object.keys(b);
                    for (var d = 0; d < a.length; d++) c.push({
                        name: a[d],
                        plugin: b[a[d]],
                        options: {}
                    })
                }
                if (c && c.length)
                    for (d = 0; d < c.length; d++) {
                        if (c[d].name && c[d].plugin) {
                            var e = c[d].name;
                            var f = c[d].plugin;
                            var g = c[d].options
                        }
                        typeof f === _$_eb2b[85] && (this.plugins[e] = f.call(r, this, g, this.config), typeof f.license == _$_eb2b[85] && r.license && f.license(r.license))
                    }
            };
            k.execute = function (b, c) {
                if (this.plugins) {
                    var a = Object.keys(this.plugins);
                    if (a.length)
                        for (var d =
                            0; d < a.length; d++)
                            if (typeof this.plugins[a[d]][b] == _$_eb2b[85]) {
                                var e = this.plugins[a[d]][b].apply(this.plugins[a[d]], c);
                                e && (b == _$_eb2b[86] && (c[4] = e), b == _$_eb2b[87] && (c[0] = e))
                            } return c
                }
            };
            k.onevent = function (b) {
                return function () {
                    var c, a, d = this.plugins;
                    if (d) {
                        var e = Object.keys(d);
                        if (e.length)
                            for (a = 0; a < e.length; a++)
                                if (typeof d[e[a]][b] == _$_eb2b[85]) {
                                    var f = d[e[a]][b].apply(d[e[a]], arguments);
                                    typeof f !== _$_eb2b[13] && (c = f)
                                }
                    }
                    return c
                }
            }(_$_eb2b[88]);
            return k
        }(),
        ba = function () {
            var k = function (b) {
                b.getEditor = k[_$_eb2b[51]];
                b.openEditor = k.open;
                b.closeEditor = k.close
            };
            k[_$_eb2b[51]] = function (b, c) {
                b = Ha[_$_eb2b[51]].call(this, b, c);
                return [b.type && typeof b.type == _$_eb2b[84] ? b.type : r.editors[b.type], b]
            };
            k.open = function (b, c, a) {
                if (!Q.call(this.parent, this)) return !1;
                if (1 != b.classList.contains(_$_eb2b[18])) {
                    var d = parseInt(b.getAttribute(_$_eb2b[42])),
                        e = parseInt(b.getAttribute(_$_eb2b[43]));
                    if (!0 === this.options.locked) {
                        var f = E.getColumnNameFromCoords(d, e);
                        if (!this.options.cells || !this.options.cells[f] || !1 !== this.options.cells[f].locked) return !1
                    }
                    if (f =
                        this.records[e][d].merged) d -= f[0], e -= f[1], b = this.records[e][d].element;
                    if (!N.attached.call(this, d, e)) {
                        if (this.results && 0 < this.results.length) return console.error(_$_eb2b[89]), !1;
                        G[_$_eb2b[46]].call(this, e, d)
                    }
                    k.position.call(this, d, e);
                    f = this.parent.input;
                    if (113 === a.keyCode || a.type !== _$_eb2b[90]) f.selected = !0;
                    f.innerText = _$_eb2b[3];
                    F.call(this.parent, _$_eb2b[91], this, b, d, e);
                    c = 1 == c ? _$_eb2b[3] : M.call(this, d, e);
                    var g = k[_$_eb2b[51]].call(this, d, e),
                        h = !0;
                    typeof g[0].openEditor == _$_eb2b[85] && (!1 === g[0].openEditor(b,
                        c, d, e, this, g[1], a) ? h = !1 : F.call(this.parent, _$_eb2b[92], this, b, d, e, f, g[1]));
                    h && (1 == this.options.textOverflow && (this.records[e][d].element.style.overflow = _$_eb2b[49], 0 < d && (this.records[e][d - 1].element.style.overflow = _$_eb2b[49])), this.edition = b, f.classList.add(_$_eb2b[93]), 1 == this.parent.config.editorFormulas && P(c) && (f.classList.add(_$_eb2b[94]), la.parse.call(this, f), b = document.createTextNode(_$_eb2b[3]), f.appendChild(b), jSuites.focus(f)))
                }
            };
            k.close = function (b, c) {
                null === b && (b = this.edition);
                this.parent.input.classList.contains(_$_eb2b[94]) &&
                    la.close.call(this, this.parent.input);
                var a, d = parseInt(b.getAttribute(_$_eb2b[42])),
                    e = parseInt(b.getAttribute(_$_eb2b[43]));
                1 == this.options.textOverflow && (a = this.records[e][d + 1]) && a.element && a.element.innerText == _$_eb2b[3] && (this.records[e][d].element.style.overflow = _$_eb2b[3]);
                a = k[_$_eb2b[51]].call(this, d, e);
                if (1 == c) {
                    var f = null;
                    typeof a[0].closeEditor == _$_eb2b[85] && (a = a[0].closeEditor(b, !0, d, e, this, a[1]), void 0 !== a && (f = a));
                    null !== f && M.call(this, d, e) != f && this.setValue(b, f)
                } else a[0].closeEditor(b,
                    !1, d, e, this, a[1]);
                F.call(this.parent, _$_eb2b[95], this, b, d, e, f, c);
                a = this.parent.input;
                a.selected = !1;
                a.onblur && (a.onblur = null);
                a.children[0] && a.children[0].onblur && (a.children[0].onblur = null);
                a.update && (a.update = null);
                ba.build(this.parent);
                a.removeAttribute(_$_eb2b[96]);
                a.removeAttribute(_$_eb2b[97]);
                a.removeAttribute(_$_eb2b[98]);
                this.edition = a.mask = null;
                this.refreshBorders()
            };
            k.position = function (b, c) {
                var a = null;
                if (a = this.records[c][b].element) {
                    var d = this.parent.element.getBoundingClientRect();
                    a = a.getBoundingClientRect();
                    var e = this.parent.input;
                    e.x = b;
                    e.y = c;
                    0 == a.width && 0 == a.height ? e.style.opacity = _$_eb2b[99] : (e.setAttribute(_$_eb2b[100], _$_eb2b[3]), b = a.top - d.top + 1, c = a.left - d.left + 1, e.style.top = b + _$_eb2b[44], e.style.left = c + _$_eb2b[44], e.style.minWidth = a.width - 1 + _$_eb2b[44], e.style.minHeight = a.height - 1 + _$_eb2b[44], setTimeout(function () {
                        e.focus()
                    }, 0))
                }
            };
            k.build = function (b) {
                b.input || (b.input = document.createElement(_$_eb2b[20]));
                b.input.className = _$_eb2b[101];
                b.input.setAttribute(_$_eb2b[102], !0);
                b.input.innerText = _$_eb2b[3];
                b.input.oninput = function (c) {
                    1 == r.current.parent.config.editorFormulas && (P(c.target.innerText) ? (c.target.classList.add(_$_eb2b[94]), c.target.selected = !0) : c.target.classList.contains(_$_eb2b[94]) && la.close.call(r.current, this))
                };
                b.input.onclick = function () {
                    this.selected = !0
                };
                return b.input
            };
            return k
        }(),
        F = function (k) {
            var b;
            if (!this.ignoreEvents) {
                if (typeof this.config.onevent == _$_eb2b[85]) {
                    var c = this.config.onevent.apply(this, arguments);
                    typeof c !== _$_eb2b[13] && (b = c)
                }
                typeof this.config[k] == _$_eb2b[85] &&
                    (c = this.config[k].apply(this, Array.prototype.slice.call(arguments, 1)), typeof c !== _$_eb2b[13] && (b = c));
                c = pa.onevent.apply(this, arguments);
                typeof c !== _$_eb2b[13] && (b = c)
            }
            return b
        },
        ta = function () {
            var k = function (b) {
                b.sequence = 0;
                b.getNextSequence = k.next;
                b.getRowId = k.getId;
                b.setRowId = k.setId;
                b.getRowById = k.getRowById
            };
            k[_$_eb2b[51]] = function () {
                return this.sequence
            };
            k[_$_eb2b[11]] = function (b, c) {
                if (b > this.sequence || c) this.sequence = b
            };
            k.next = function () {
                return 1 == this.options.sequence ? ++this.sequence : null
            };
            k.reset =
                function () {
                    this.sequence = 0
                };
            k.getId = function (b) {
                if (this.rows[b] && this.rows[b].id) return this.rows[b].id;
                var c = this.getPrimaryKey();
                if (!1 !== c) return M.call(this, c, b)
            };
            k.setId = function (b, c) {
                if (void 0 !== c && 0 <= parseInt(b)) {
                    var a = {};
                    a[b] = c
                } else a = b;
                b = Object.keys(a);
                if (b.length) {
                    for (var d = 0; d < b.length; d++) c = a[b[d]], this.rows[b[d]].id = c, k[_$_eb2b[11]].call(this, c);
                    J.call(this, _$_eb2b[103], [a]);
                    F.call(this.parent, _$_eb2b[104], this, a)
                }
            };
            k.getRowById = function (b, c) {
                for (var a = 0; a < this.rows.length; a++)
                    if (k.getId.call(this,
                        a) == b) return !0 === c ? this.rows[a] : this.options.data[a];
                return !1
            };
            return k
        }(),
        Oa = function () {
            var k = function (c) {
                for (var a = [], d = {}, e = 0; e < c.length; e++) {
                    var f = c[e].x,
                        g = c[e].y,
                        h = ta.getId.call(this, g);
                    if (!h)
                        if (d[g]) h = d[g];
                        else if (h = ta.next.call(this)) d[g] = h;
                    a[g] || (a[g] = {
                        id: h,
                        row: g,
                        data: {}
                    });
                    f = Z.call(this, f);
                    a[g].data[f] = c[e].value
                }
                ta.setId.call(this, d);
                return a.filter(function (l) {
                    return null != l
                })
            },
                b = function (c) {
                    var a = c[0];
                    if (0 <= [_$_eb2b[105], _$_eb2b[106], _$_eb2b[103], _$_eb2b[107]].indexOf(a)) return !1;
                    var d = {};
                    a == _$_eb2b[77] || a == _$_eb2b[108] ? (a = _$_eb2b[109], d[a] = k.call(this, c[1].data)) : Array.isArray(c[1]) && 1 == c[1].length ? d[a] = c[1][0] : d[a] = c[1];
                    return d
                };
            return function (c, a, d, e) {
                var f = this.parent,
                    g = this;
                d = null;
                var h = b.call(this, a);
                if (h) a = h;
                else if (!1 === h) return !1;
                if (h = F.call(f, _$_eb2b[110], f, g, a)) a = h;
                else if (!1 === h) return !1;
                La.call(f, !0);
                jSuites.ajax({
                    url: c,
                    method: _$_eb2b[111],
                    dataType: _$_eb2b[59],
                    data: {
                        data: JSON.stringify(a)
                    },
                    queue: !0,
                    beforeSend: function (l) {
                        d && l.setRequestHeader(_$_eb2b[112], _$_eb2b[113] +
                            d);
                        F.call(f, _$_eb2b[114], g, l)
                    },
                    success: function (l) {
                        f.element.classList.contains(_$_eb2b[115]) && f.element.classList.remove(_$_eb2b[115]);
                        l && (l.message && (l.name = f.config.application, jSuites.notification.isVisible() ? console.log(l.name + _$_eb2b[116] + l.message) : jSuites.notification(l)), l.success ? typeof e === _$_eb2b[85] ? e(l) : l.data && Array.isArray(l.data) && N.updateAll.call(g, l.data) : l.error && (alert(_$_eb2b[117]), window.open(window.location.href, _$_eb2b[118])));
                        F.call(f, _$_eb2b[119], f, g, a, l)
                    },
                    error: function () {
                        f.element.classList.add(_$_eb2b[115])
                    }
                })
            }
        }(),
        J = function () {
            return function () {
                pa.execute.call(this.parent, _$_eb2b[120], [this, arguments[0], arguments[1]]);
                if (!this.parent.ignorePersistence) {
                    var k = null;
                    if (this.parent.config.server) {
                        var b = this.parent.worksheets.indexOf(this);
                        k = this.parent.config.server;
                        k = -1 == k.indexOf(_$_eb2b[121]) ? k + (_$_eb2b[122] + b) : k + (_$_eb2b[123] + b)
                    } else this.options.persistence && (k = typeof this.options.persistence == _$_eb2b[17] ? this.options.persistence : this.options.url);
                    k && Oa.call(this, k, arguments)
                }
            }
        }(),
        Ea = function (k) {
            if (this.selectedCell) {
                var b =
                    this.selectedCell[0],
                    c = this.selectedCell[1];
                this.records && this.records[c] && this.records[c][b] && this.records[c][b].element || (this.selectedCell = this.getHighlighted(), b = this.selectedCell[0], c = this.selectedCell[1]);
                var a = this.getHighlighted(),
                    d = a[0],
                    e = a[1],
                    f = a[2];
                a = a[3];
                var g = k ? _$_eb2b[124] : _$_eb2b[125];
                this.records[c] && this.records[c][b] && this.records[c][b].element && (this.records[c][b].element.classList[g](_$_eb2b[126]), this.cursor = k ? this.records[c][b].element : null);
                for (k = d; k <= f; k++)
                    if (this.headers[k]) this.headers[k].classList[g](_$_eb2b[127]);
                for (; e <= a; e++)
                    if (this.rows[e] && this.rows[e].element) this.rows[e].element.classList[g](_$_eb2b[127])
            }
        },
        za = function () {
            var k = function (b) {
                b.resetSelection = k.reset;
                b.updateSelection = k.fromElements;
                b.updateSelectionFromCoords = k[_$_eb2b[11]];
                b.selectAll = k.all;
                b.isSelected = k.isSelected;
                b.getHighlighted = k.getHighlighted;
                b.getRange = k.getRange
            };
            k.getHighlighted = function () {
                var b = this.borders;
                return (b = b.main) ? [b.x1, b.y1, b.x2, b.y2] : null
            };
            k.isSelected = function (b, c, a) {
                if (!a && (a = this.getHighlighted(), !a || null === a[0])) return !1;
                var d = a[0],
                    e = a[1],
                    f = a[2];
                a = a[3];
                f || (f = d);
                a || (a = e);
                return null == b ? c >= e && c <= a : null == c ? b >= d && b <= f : b >= d && b <= f && c >= e && c <= a
            };
            k.reset = function () {
                if (this.selectedCell) b = 1, this.edition && ba.close.call(this, this.edition, !0), this.resetBorders(_$_eb2b[128], !0), this.resetBorders(_$_eb2b[129], !0), this.selectedCell = null, 1 == b && F.call(this.parent, _$_eb2b[130], this);
                else var b = 0;
                return b
            };
            k.refresh = function () {
                this.selectedCell && k[_$_eb2b[11]].call(this, this.selectedCell)
            };
            k.isValid = function (b, c, a, d) {
                return this ? !(b >=
                    this.options.columns.length || c >= this.rows.length || a >= this.options.columns.length || d >= this.rows.length) : !1
            };
            k[_$_eb2b[11]] = function (b, c, a, d, e, f, g) {
                f || (f = _$_eb2b[128]);
                Array.isArray(b) && (d = b[3], a = b[2], c = b[1], b = b[0]);
                null == a && (a = b);
                null == d && (d = c);
                if (!k.isValid.call(this, b, c, a, d)) return !1;
                if (null != b) {
                    if (parseInt(b) < parseInt(a)) var h = parseInt(b),
                        l = parseInt(a);
                    else h = parseInt(a), l = parseInt(b);
                    if (parseInt(c) < parseInt(d)) var m = parseInt(c),
                        p = parseInt(d);
                    else m = parseInt(d), p = parseInt(c);
                    var n;
                    var t = {};
                    if (this.options.mergeCells) {
                        for (var v =
                            m; v <= p; v++)
                            for (var u = h; u <= l; u++)
                                if (n = this.records[v][u].merged) n = E.getColumnNameFromCoords(u - n[0], v - n[1]), t[n] = !0;
                        v = Object.keys(t);
                        if (v.length)
                            for (u = 0; u < v.length; u++) n = E.getCoordsFromColumnName(v[u]), t = this.options.mergeCells[v[u]], n[0] < h && (h = n[0]), n[0] + t[0] - 1 > l && (l = n[0] + t[0] - 1), n[1] < m && (m = n[1]), n[1] + t[1] - 1 > p && (p = n[1] + t[1] - 1)
                    }
                }
                f == _$_eb2b[128] && (this.selectedCell ? (Ea.call(this, !1), n = 1) : n = 0, this.selectedCell = [b, c, a, d], this.records[d][a].element && G.isVisible.call(this, this.records[d][a].element), ba.position.call(this,
                    b, c), 0 == n && F.call(this.parent, _$_eb2b[131], this));
                ka[_$_eb2b[11]].call(this, h, m, l, p, f, g);
                f == _$_eb2b[128] && (F.call(this.parent, _$_eb2b[132], this, h, m, l, p, e), fa.update.call(this.parent, this, this.selectedCell))
            };
            k.fromElements = function (b, c, a) {
                var d = b.getAttribute(_$_eb2b[42]);
                b = b.getAttribute(_$_eb2b[43]);
                if (c) {
                    var e = c.getAttribute(_$_eb2b[42]);
                    c = c.getAttribute(_$_eb2b[43])
                } else e = d, c = b;
                this.updateSelectionFromCoords(d, b, e, c, a)
            };
            k.all = function () {
                k[_$_eb2b[11]].call(this, 0, 0, this.options.columns.length -
                    1, this.records.length - 1)
            };
            k.getRange = function () {
                var b = this.selectedCell;
                if (!b) return _$_eb2b[3];
                var c = E.getColumnNameFromCoords(b[0], b[1]);
                b = E.getColumnNameFromCoords(b[2], b[3]);
                var a = this.options.worksheetName;
                a = -1 != a.indexOf(_$_eb2b[133]) ? _$_eb2b[134] + a + _$_eb2b[135] : a + _$_eb2b[136];
                return c == b ? a + c : a + r.helpers.getRangeFromTokens(r.helpers.getTokensFromRange(c + _$_eb2b[137] + b))
            };
            return k
        }(),
        ka = function () {
            var k = function (a) {
                a.borders = [];
                a.setBorder = k[_$_eb2b[11]];
                a.getBorder = k[_$_eb2b[51]];
                a.resetBorders =
                    k.reset;
                a.refreshBorders = k.refresh
            };
            k[_$_eb2b[51]] = function (a) {
                return this.borders[a]
            };
            var b = function (a, d, e, f, g, h) {
                g || (g = 0);
                if (!za.isValid.call(this, a, d, e, f)) return !1;
                if (this.borders[g]) l = this.borders[g];
                else {
                    var l = {};
                    l.element = document.createElement(_$_eb2b[20]);
                    l.element.classList.add(_$_eb2b[138]);
                    g == _$_eb2b[129] || g == _$_eb2b[139] || g == _$_eb2b[128] ? l.element.classList.add(_$_eb2b[140] + g) : (h = h ? h : jSuites.randomColor(!0), l.color = h, l.element.style.backgroundColor = h + _$_eb2b[141], l.element.style.borderColor =
                        h);
                    this.content.appendChild(l.element)
                }
                l.x1 = a;
                l.y1 = d;
                l.x2 = e;
                l.y2 = f;
                h = this.headers[a] ? this.headers[a].offsetLeft : 0;
                var m = this.headers[e] ? this.headers[e].offsetLeft : 0,
                    p = this.rows[d].element ? this.rows[d].element.offsetTop : 0,
                    n = this.rows[f].element ? this.rows[f].element.offsetTop : 0;
                l.element.style.top = _$_eb2b[142];
                l.element.style.left = _$_eb2b[142];
                l.active = 0;
                var t = this.options.freezeColumns || 0,
                    v = !1,
                    u = null,
                    y = null,
                    z = null,
                    D = null;
                this.tbody.firstChild && (y = parseInt(this.tbody.firstChild.getAttribute(_$_eb2b[43])),
                    D = parseInt(this.tbody.lastChild.getAttribute(_$_eb2b[43])));
                this.thead.lastChild && (u = parseInt(this.thead.lastChild.children[t + 1].getAttribute(_$_eb2b[42])), z = parseInt(this.thead.lastChild.lastChild.getAttribute(_$_eb2b[42])), this.thead.lastChild.children[1].style.display == _$_eb2b[40] && (u = Ma.call(this, u, y)), this.thead.lastChild.lastChild.style.display == _$_eb2b[40] && (z = Na.call(this, z, D)));
                null == u || e < u && a >= t || a > z || f < y || d > D || (h ? l.element.style.borderLeftColor = l.color ? l.color : _$_eb2b[3] : (h = this.thead.lastChild.children[t +
                    1].offsetLeft, l.element.style.borderLeftColor = g == _$_eb2b[129] ? _$_eb2b[143] : _$_eb2b[144]), m ? (m += this.headers[e].offsetWidth, l.element.style.borderRightColor = l.color ? l.color : _$_eb2b[3]) : (e > z ? (m = this.headers[z].offsetLeft + this.headers[z].offsetWidth, v = !0) : m = this.thead.lastChild.children[t].offsetLeft + this.thead.lastChild.children[t].offsetWidth, l.element.style.borderRightColor = g == _$_eb2b[129] ? _$_eb2b[143] : _$_eb2b[144]), m = m - h - 1, p ? l.element.style.borderTopColor = l.color ? l.color : _$_eb2b[3] : (p = this.tbody.firstChild.offsetTop,
                        l.element.style.borderTopColor = g == _$_eb2b[129] ? _$_eb2b[143] : _$_eb2b[144]), n ? (n += this.rows[f].element.offsetHeight, l.element.style.borderBottomColor = l.color ? l.color : _$_eb2b[3]) : (n = this.rows[D].element.offsetTop + this.rows[D].element.offsetHeight, l.element.style.borderBottomColor = g == _$_eb2b[129] ? _$_eb2b[143] : _$_eb2b[144], v = !0), n = n - p - 1, this.options.freezeColumns && (l.element.style.borderLeftWidth = _$_eb2b[3], l.element.style.borderRightWidth = _$_eb2b[3], d = 0 < this.content.scrollLeft ? ja.getWidth.call(this) :
                            0) && (a = a < this.options.freezeColumns, e = e < this.options.freezeColumns, a && e || !a && e || (a && !e ? (e = d + this.content.scrollLeft, e > h + m && (l.element.style.borderRightWidth = _$_eb2b[145], m += e - (h + m))) : (e = d + this.content.scrollLeft, e > h && (e -= h, e > m ? h = p = -2E3 : (h = h + e + 1, m -= e, l.element.style.borderLeftWidth = _$_eb2b[145]))))), l.element.style.top = p + _$_eb2b[44], l.element.style.left = h + _$_eb2b[44], l.element.style.width = m + _$_eb2b[44], l.element.style.height = n + _$_eb2b[44], l.t = p, l.l = h, l.w = m, l.h = n, l.active = 1);
                g == _$_eb2b[128] && (l.active &&
                    1 != v ? (this.corner.style.top = p + n - 2 + _$_eb2b[44], this.corner.style.left = h + m - 2 + _$_eb2b[44]) : (this.corner.style.top = _$_eb2b[142], this.corner.style.left = _$_eb2b[142]));
                this.borders[g] = l
            },
                c = function (a, d) {
                    a.element.style.top = _$_eb2b[142];
                    a.element.style.left = _$_eb2b[142];
                    a.active = 0;
                    d && (a.x1 = null, a.y1 = null, a.x2 = null, a.y2 = null)
                };
            k[_$_eb2b[11]] = function (a, d, e, f, g, h) {
                b.call(this, a, d, e, f, g, h);
                g == _$_eb2b[128] && (Ea.call(this, !0), J.call(this, _$_eb2b[105], {
                    x1: a,
                    y1: d,
                    x2: e,
                    y2: f
                }))
            };
            k.reset = function (a, d) {
                a && a != _$_eb2b[128] ||
                    (this.corner.style.top = _$_eb2b[142], this.corner.style.left = _$_eb2b[142], Ea.call(this, !1));
                if (a) this.borders[a] && c(this.borders[a], d);
                else
                    for (var e = Object.keys(this.borders), f = 0; f < e.length; f++) c(this.borders[e[f]], d);
                a && a != _$_eb2b[128] || J.call(this, _$_eb2b[106])
            };
            k.refresh = function (a) {
                var d = this.borders;
                if (a) d[a] && null != d[a].x1 && b.call(this, d[a].x1, d[a].y1, d[a].x2, d[a].y2, a);
                else {
                    a = Object.keys(d);
                    for (var e = 0; e < a.length; e++) d[a[e]] && null != d[a[e]].x1 && b.call(this, d[a[e]].x1, d[a[e]].y1, d[a[e]].x2, d[a[e]].y2,
                        a[e])
                }
                this.edition && (d = this.parent.input, ba.position.call(this, d.x, d.y))
            };
            k.destroy = function (a) {
                var d;
                this && (d = this.borders[a]) && (d.element.remove(), delete this.borders[a])
            };
            return k
        }(),
        Va = function () {
            var k = function (b) {
                b.setGroup = k[_$_eb2b[11]];
                b.unsetGroup = k.unset
            };
            k[_$_eb2b[11]] = function (b, c, a) {
                0 == b && (a = document.createElement(_$_eb2b[146]), a.classList.add(_$_eb2b[147]), this.thead.insertBefore(a, this.thead.firstChild), b = document.createElement(_$_eb2b[148]), b.innerHTML = _$_eb2b[149], a.appendChild(b),
                    b = document.createElement(_$_eb2b[148]), a.appendChild(b), c = this.headers[c]) && (c = c.querySelector(_$_eb2b[150]), c || (c = document.createElement(_$_eb2b[20]), c.classList.add(_$_eb2b[151]), c.innerText = _$_eb2b[152], b.appendChild(c)))
            };
            k.unset = function (b, c) { };
            return k
        }(),
        wa = function () {
            var k = {
                start: function (b) {
                    if (!Q.call(this.parent, this)) return !1;
                    if (null !== b.target.getAttribute(_$_eb2b[43])) {
                        var c = b.target.getAttribute(_$_eb2b[43]);
                        k.event = {
                            y: parseInt(c),
                            h: b.target.offsetHeight,
                            p: b.pageY
                        }
                    } else null !== b.target.getAttribute(_$_eb2b[42]) &&
                        (c = b.target.getAttribute(_$_eb2b[42]), k.event = {
                            x: parseInt(c),
                            w: b.target.offsetWidth,
                            p: b.pageX
                        })
                },
                end: function (b) {
                    var c = [];
                    if (null != k.event.y) {
                        var a = parseInt(this.rows[k.event.y].element.offsetHeight);
                        var d = this.getSelectedRows(!0);
                        b = d.indexOf(parseInt(k.event.y));
                        if (d.length && -1 < b)
                            for (var e = 0; e < d.length; e++) c.push(parseInt(this.rows[d[e]].element.offsetHeight));
                        else d = []; - 1 == b ? (d.push(k.event.y), c.push(k.event.h)) : c[b] = k.event.h;
                        this.setHeight(d, a, c)
                    } else if (null != k.event.x) {
                        a = parseInt(this.colgroup[k.event.x].getAttribute(_$_eb2b[153]));
                        d = this.getSelectedColumns();
                        b = d.indexOf(parseInt(k.event.x));
                        if (d.length && -1 < b)
                            for (e = 0; e < d.length; e++) c.push(parseInt(this.options.columns[d[e]].width));
                        else d = []; - 1 == b ? (d.push(k.event.x), c.push(k.event.w)) : c[b] = k.event.w;
                        this.setWidth(d, a, c)
                    }
                    k.event = null
                },
                update: function (b) {
                    null != k.event.y ? (b = b.pageY - k.event.p, 0 < k.event.h + b && (this.rows[k.event.y].element.style.height = k.event.h + b + _$_eb2b[44], this.refreshBorders())) : null != k.event.x && (b = b.pageX - k.event.p, 0 < k.event.w + b && (this.colgroup[k.event.x].setAttribute(_$_eb2b[153],
                        k.event.w + b), this.options.columns[k.event.x].width = k.event.w + b, 0 < this.options.freezeColumns && ja.width.call(this), this.refreshBorders()))
                },
                cancel: function () {
                    k.end.call(this)
                }
            };
            return k
        }(),
        Pa = function () {
            var k = {
                start: function (b) {
                    var c = this.parent.helper;
                    b = parseInt(b.target.getAttribute(_$_eb2b[42]));
                    if (this.merged.cols[b]) console.error(_$_eb2b[154]);
                    else {
                        k.event = {
                            x: b,
                            d: b
                        };
                        this.headers[b].classList.add(_$_eb2b[155]);
                        for (var a = 0; a < this.tbody.children.length; a++) {
                            var d = this.tbody.children[a].getAttribute(_$_eb2b[43]);
                            this.records[d][b].element.classList.add(_$_eb2b[155])
                        }
                        a = this.parent.element.getBoundingClientRect();
                        b = this.headers[b].getBoundingClientRect();
                        d = this.content.getBoundingClientRect();
                        c.style.display = _$_eb2b[16];
                        c.style.top = d.top - a.top + _$_eb2b[44];
                        c.style.left = b.left - d.left + _$_eb2b[44];
                        c.style.width = _$_eb2b[156];
                        c.style.height = d.height + _$_eb2b[44];
                        c.classList.add(_$_eb2b[157])
                    }
                },
                end: function () {
                    this.headers[k.event.x].classList.remove(_$_eb2b[155]);
                    for (var b = 0; b < this.tbody.children.length; b++) {
                        var c =
                            this.tbody.children[b].getAttribute(_$_eb2b[43]);
                        this.records[c][k.event.x].element.classList.remove(_$_eb2b[155])
                    }
                    null != k.event.d && k.event.x != k.event.d && (this.moveColumn(k.event.x, k.event.d), this.refreshBorders());
                    this.parent.helper.classList.remove(_$_eb2b[157]);
                    this.parent.helper.style.display = _$_eb2b[3];
                    k.event = null
                },
                cancel: function () {
                    k.event.d = null;
                    k.end.call(this)
                },
                update: function (b) {
                    var c = b.target.getAttribute(_$_eb2b[42]);
                    if (null != c)
                        if (c = parseInt(c), this.merged.cols[c]) console.error(_$_eb2b[154]);
                        else {
                            b = b.target.clientWidth / 2 > b.offsetX;
                            k.event.d = b ? k.event.x < c ? parseInt(c) - 1 : parseInt(c) : k.event.x > c ? parseInt(c) + 1 : parseInt(c);
                            c = this.headers[c].getBoundingClientRect();
                            var a = this.content.getBoundingClientRect();
                            this.parent.helper.style.left = (b ? c.left : c.right) - a.left + _$_eb2b[44]
                        }
                }
            };
            return k
        }(),
        ra = function () {
            var k = {
                start: function (b) {
                    if (Q.call(this.parent, this)) {
                        this.resetSelection();
                        var c = this.parent.helper;
                        if (null !== b.target.getAttribute(_$_eb2b[43])) {
                            var a = parseInt(b.target.getAttribute(_$_eb2b[43]));
                            this.merged.rows[a] ? console.error(_$_eb2b[158]) : this.results ? console.error(_$_eb2b[159]) : (k.event = {
                                y: a,
                                d: a
                            }, this.rows[a].element.classList.add(_$_eb2b[155]), b = this.parent.element.getBoundingClientRect(), a = this.rows[a].element.getBoundingClientRect(), c.style.display = _$_eb2b[16], c.style.top = a.top - b.top + _$_eb2b[44], c.style.left = _$_eb2b[160], c.style.width = this.content.offsetWidth - 50 + _$_eb2b[44], c.style.height = _$_eb2b[156], c.classList.add(_$_eb2b[161]))
                        } else if (null !== b.target.getAttribute(_$_eb2b[42]))
                            if (a =
                                parseInt(b.target.getAttribute(_$_eb2b[42])), this.merged.cols[a]) console.error(_$_eb2b[154]);
                            else {
                                k.event = {
                                    x: a,
                                    d: a
                                };
                                this.headers[a].classList.add(_$_eb2b[155]);
                                for (b = 0; b < this.tbody.children.length; b++) {
                                    var d = this.tbody.children[b].getAttribute(_$_eb2b[43]);
                                    this.records[d][a].element.classList.add(_$_eb2b[155])
                                }
                                b = this.parent.element.getBoundingClientRect();
                                a = this.headers[a].getBoundingClientRect();
                                d = this.content.getBoundingClientRect();
                                c.style.display = _$_eb2b[16];
                                c.style.top = d.top - b.top + _$_eb2b[44];
                                c.style.left = a.left - d.left + _$_eb2b[44];
                                c.style.width = _$_eb2b[156];
                                c.style.height = d.height + _$_eb2b[44];
                                c.classList.add(_$_eb2b[157])
                            }
                    } else return !1
                },
                end: function () {
                    if (null != k.event.y) this.rows[k.event.y].element.classList.remove(_$_eb2b[155]), null != k.event.d && k.event.y != k.event.d && (this.moveRow(k.event.y, k.event.d), this.updateSelectionFromCoords(0, k.event.d, this.options.columns.length - 1, k.event.d)), this.parent.helper.classList.remove(_$_eb2b[161]);
                    else if (null != k.event.x) {
                        this.headers[k.event.x].classList.remove(_$_eb2b[155]);
                        for (var b = 0; b < this.tbody.children.length; b++) {
                            var c = this.tbody.children[b].getAttribute(_$_eb2b[43]);
                            this.records[c][k.event.x].element.classList.remove(_$_eb2b[155])
                        }
                        null != k.event.d && k.event.x != k.event.d && (this.moveColumn(k.event.x, k.event.d), this.refreshBorders());
                        this.parent.helper.classList.remove(_$_eb2b[157])
                    }
                    this.parent.helper.style.display = _$_eb2b[3];
                    k.event = null
                },
                cancel: function () {
                    k.event.d = null;
                    k.end.call(this)
                },
                update: function (b) {
                    if (null != k.event.y) {
                        var c = b.target.getAttribute(_$_eb2b[43]);
                        if (null != c) {
                            var a = c = parseInt(c),
                                d = b.target.clientHeight / 2 > b.offsetY;
                            d ? k.event.y < c && (a = c - 1) : k.event.y > c && (a = c + 1);
                            b = !1;
                            this.merged.rows[a] && (k.event.y > a ? this.merged.rows[a - 1] && (b = !0) : this.merged.rows[a + 1] && (b = !0));
                            b || (k.event.d = a, a = this.parent.element.getBoundingClientRect(), c = this.rows[c].element.getBoundingClientRect(), this.parent.helper.style.top = (d ? c.top : c.bottom) - a.top + _$_eb2b[44])
                        }
                    } else d = b.target.getAttribute(_$_eb2b[42]), null != d && (a = d = parseInt(d), (c = b.target.clientWidth / 2 > b.offsetX) ? k.event.x <
                        d && (a = d - 1) : k.event.x > d && (a = d + 1), b = !1, this.merged.cols[a] && (k.event.x > a ? this.merged.cols[a - 1] && (b = !0) : this.merged.cols[a + 1] && (b = !0)), b || (k.event.d = a, a = this.headers[d].getBoundingClientRect(), d = this.content.getBoundingClientRect(), this.parent.helper.style.left = (c ? a.left : a.right) - d.left + _$_eb2b[44]))
                }
            };
            return k
        }(),
        ca = function () {
            var k = function (b) {
                b.getMerge = k[_$_eb2b[51]];
                b.setMerge = k[_$_eb2b[11]];
                b.updateMerge = k.update;
                b.removeMerge = k.remove;
                b.destroyMerged = b.destroyMerge = k.destroy
            };
            k.getHeight = function (b,
                c) {
                var a, d = 1;
                if (this.merged.rows[b])
                    for (var e = 0; e < this.options.columns.length; e++) {
                        var f = E.getColumnNameFromCoords(e, b);
                        if (f = this.merged.cells[f])
                            if (a = this.options.mergeCells[f]) c && void 0 !== c[f] || (a[1] > d && (d = a[1]), c && (c[f] = this.records[b][e].element && !this.records[b][e].merged ? !0 : !1))
                    }
                return d - 1
            };
            k.getWidth = function (b, c) {
                var a, d = 1;
                if (this.merged.cols[b])
                    for (var e = 0; e < this.options.data.length; e++) {
                        var f = E.getColumnNameFromCoords(b, e);
                        if (f = this.merged.cells[f])
                            if (a = this.options.mergeCells[f]) c && void 0 !==
                                c[f] || (a[0] > d && (d = a[0]), c && (c[f] = this.records[e][b].element && !this.records[e][b].merged ? !0 : !1))
                    }
                return d - 1
            };
            k[_$_eb2b[51]] = function (b) {
                return b ? this.options.mergeCells[b] : this.options.mergeCells
            };
            k[_$_eb2b[11]] = function (b, c, a) {
                if (!Q.call(this.parent, this)) return !1;
                this.options.mergeCells || (this.options.mergeCells = {});
                var d, e = null,
                    f = {},
                    g = {},
                    h = {},
                    l = !1;
                typeof b == _$_eb2b[17] ? h[b] = [c, a] : typeof b == _$_eb2b[84] ? h = b : ((e = this.getHighlighted()) ? e = [E.getColumnNameFromCoords(e[0], e[1]), e[2] - e[0] + 1, e[3] - e[1] + 1] :
                    (jSuites.notification({
                        message: B(_$_eb2b[162])
                    }), e = !1), e && (h[e[0]] = [e[1], e[2]]));
                c = Object.keys(h);
                if (c.length) {
                    for (a = 0; a < c.length; a++) {
                        var m = c[a],
                            p = h[c[a]][0],
                            n = h[c[a]][1];
                        p = parseInt(p);
                        n = parseInt(n);
                        if ((!p || 2 > p) && (!n || 2 > n)) console.log(B(_$_eb2b[163]) + _$_eb2b[137] + m);
                        else if (this.options.mergeCells[m]) g[m] = [p, n], f[m] = this.options.mergeCells[m], k.update.call(this, m, p, n);
                        else {
                            e = E.getCoordsFromColumnName(m);
                            for (var t = e[1]; t < e[1] + n; t++)
                                for (var v = e[0]; v < e[0] + p; v++) b = E.getColumnNameFromCoords(v, t), (d = this.merged.cells[b]) &&
                                    this.options.mergeCells[d] && (f[d] = this.options.mergeCells[d], k.applyDestroy.call(this, d));
                            g[m] = this.options.mergeCells[m] = [p, n];
                            k.applyCreate.call(this, m);
                            N.attached.call(this, e[0], e[1]) || (l = e)
                        }
                    }
                    Object.keys(g).length && (l && G[_$_eb2b[46]].call(this, e[1], e[0]), T.call(this.parent, {
                        worksheet: this,
                        action: _$_eb2b[68],
                        newValue: g,
                        oldValue: f
                    }), J.call(this, _$_eb2b[68], [g]), k.build.call(this), za.refresh.call(this), ka.refresh.call(this), F.call(this.parent, _$_eb2b[164], this, g, f))
                }
            };
            k.update = function (b, c, a) {
                var d =
                    this.options.mergeCells;
                if (d[b]) {
                    var e = d[b][1];
                    if (c !== d[b][0] || e !== a) k.applyDestroy.call(this, b), c && a && (d[b] = [c, a], k.applyCreate.call(this, b))
                }
            };
            k.remove = function (b, c) {
                if (!Q.call(this.parent, this)) return !1;
                c = {};
                var a = {},
                    d = {};
                typeof b == _$_eb2b[17] ? d[b] = !0 : typeof b == _$_eb2b[84] && (d = b);
                b = Object.keys(d);
                for (d = 0; d < b.length; d++) this.options.mergeCells[b[d]] && (c[b[d]] = this.options.mergeCells[b[d]], a[b[d]] = !0, k.applyDestroy.call(this, b[d], !0));
                T.call(this.parent, {
                    worksheet: this,
                    action: _$_eb2b[69],
                    newValue: a,
                    oldValue: c
                });
                J.call(this, _$_eb2b[69], {
                    data: a
                })
            };
            k.destroy = function () {
                k.remove.call(this, this.options.mergeCells, !0)
            };
            k.applyCreate = function (b, c) {
                var a = this.options.mergeCells;
                if (a[b]) {
                    var d = a[b][0] || 1,
                        e = a[b][1] || 1;
                    b = E.getCoordsFromColumnName(b);
                    if (!this.records[b[1]][b[0]].merged || 1 == c)
                        if (a = N[_$_eb2b[51]].call(this, b[0], b[1])) {
                            a.style.display = _$_eb2b[3];
                            1 < d ? a.setAttribute(_$_eb2b[52], d) : a.removeAttribute(_$_eb2b[52]);
                            1 < e ? a.setAttribute(_$_eb2b[165], e) : a.removeAttribute(_$_eb2b[165]);
                            1 == this.options.textOverflow &&
                                (a.style.overflow = _$_eb2b[49]);
                            for (var f = 0; f < e; f++)
                                for (var g = 0; g < d; g++) {
                                    c = b[0] + g;
                                    var h = b[1] + f;
                                    if (a = this.records[h][c])
                                        if (a.merged = [g, f, a.v, a.v], 0 != g || 0 != f) a.element || N[_$_eb2b[51]].call(this, c, h), a.element.style.display = _$_eb2b[40], a.merged[3] = a.element.innerText, a.element.innerText = _$_eb2b[3], M.call(this, c, h, _$_eb2b[3])
                                }
                        }
                }
            };
            k.applyDestroy = function (b, c) {
                var a = this.options.mergeCells;
                if (a[b]) {
                    var d = E.getCoordsFromColumnName(b),
                        e = d[0];
                    d = d[1];
                    var f = a[b][0],
                        g = a[b][1];
                    delete this.options.mergeCells[b];
                    for (b = 0; b < g; b++)
                        for (var h = 0; h < f; h++)
                            if (a = this.records[d + b][e + h]) a.element && (0 == h && 0 == b ? (a.element.removeAttribute(_$_eb2b[52]), a.element.removeAttribute(_$_eb2b[165])) : (a.element.style.display = _$_eb2b[3], 1 == c && (M.call(this, e + h, d + b, a.merged[2]), a.element.innerHTML = a.merged[3]))), a.merged && delete a.merged
                }
            };
            k.updateConfig = function (b, c, a, d) {
                if (1 == c) var e = [a];
                else {
                    e = [];
                    for (var f = a; f < a + d; f++) e.push(f)
                }
                var g = this.options.mergeCells,
                    h = Object.keys(g),
                    l = {};
                for (f = 0; f < h.length; f++) {
                    a = E.getCoordsFromColumnName(h[f]);
                    for (var m = a[b]; m < a[b] + g[h[f]][b]; m++) - 1 !== e.indexOf(m) && (l[h[f]] || (l[h[f]] = [g[h[f]][0], g[h[f]][1]], 1 == c && (l[h[f]][b] += d)), 0 == c && l[h[f]][b]--)
                }
                h = Object.keys(l);
                for (f = 0; f < h.length; f++) k.update.call(this, h[f], l[h[f]][0], l[h[f]][1]);
                k.build.call(this)
            };
            k.build = function () {
                var b = this.options.mergeCells;
                this.merged = {
                    cols: [],
                    rows: [],
                    cells: []
                };
                if (b)
                    for (var c = Object.keys(b), a = 0; a < c.length; a++) {
                        for (var d = E.getCoordsFromColumnName(c[a]), e = parseInt(b[c[a]][0]), f = parseInt(b[c[a]][1]), g = 0; g < e; g++) this.merged.cols[d[0] +
                            g] = !0;
                        for (var h = 0; h < f; h++) this.merged.rows[d[1] + h] = !0;
                        for (h = 0; h < f; h++)
                            for (g = 0; g < e; g++) {
                                var l = E.getColumnNameFromCoords(d[0] + g, d[1] + h);
                                this.merged.cells[l] = c[a]
                            }
                    }
            };
            k.batch = function (b) {
                var c = Object.keys(b);
                if (c.length)
                    for (var a = 0; a < c.length; a++) !1 === b[c[a]] && k.applyCreate.call(this, c[a])
            };
            return k
        }(),
        Ca = function () {
            var k = function (b) {
                b.getHeader = k[_$_eb2b[51]];
                b.setHeader = k[_$_eb2b[11]];
                b.getHeaders = k.all
            };
            k[_$_eb2b[51]] = function (b) {
                var c;
                if (c = this.options.columns) return c[b].title || E.getColumnName(b)
            };
            k.all = function (b) {
                for (var c = [], a = 0; a < this.options.columns.length; a++) c.push(k[_$_eb2b[51]].call(this, a));
                return b ? c : c.join(this.options.csvDelimiter)
            };
            k[_$_eb2b[11]] = function (b, c) {
                if (!Q.call(this.parent, this)) return !1;
                var a;
                if (a = this.options.columns[b]) {
                    var d = k[_$_eb2b[51]].call(this, b);
                    if (void 0 === c && (c = prompt(B(_$_eb2b[166]), d), !c)) return !1;
                    a.title = c;
                    if (a = this.headers[b]) c ? (a.innerHTML = c, a.setAttribute(_$_eb2b[167], c)) : (a.innerHTML = E.getColumnName(b), a.removeAttribute(_$_eb2b[167]));
                    T.call(this.parent, {
                        worksheet: this,
                        action: _$_eb2b[74],
                        column: b,
                        oldValue: d,
                        newValue: c
                    });
                    J.call(this, _$_eb2b[74], {
                        column: b,
                        title: c
                    });
                    F.call(this.parent, _$_eb2b[168], this, b, c, d)
                }
            };
            k.create = function (b) {
                var c = this.options.columns[b],
                    a = c.width || this.options.defaultColWidth,
                    d = c.title || E.getColumnName(b),
                    e = document.createElement(_$_eb2b[148]);
                this.parent.config.stripHTML ? e.innerText = d : e.innerHTML = d;
                e.setAttribute(_$_eb2b[42], b);
                c.title && e.setAttribute(_$_eb2b[167], e.innerText);
                c.tooltip && (e.title = c.tooltip);
                d = this.options.filters;
                !0 === c.filter && (d = !0);
                !1 === c.filter && (d = !1);
                !0 === d && e.classList.add(_$_eb2b[169]);
                !1 === c.visible && (a = _$_eb2b[99]);
                d = document.createElement(_$_eb2b[170]);
                d.setAttribute(_$_eb2b[153], a);
                c.type == _$_eb2b[49] && (e.style.display = _$_eb2b[40], d.style.display = _$_eb2b[40]);
                this.headers[b] = e;
                this.colgroup[b] = d;
                F.call(this.parent, _$_eb2b[171], this, b, e, c)
            };
            return k
        }(),
        aa = function () {
            var k = function (c, a, d) {
                var e = this.footers.content,
                    f;
                e && e[a] && e[a][c] && (f = e[a][c].element) && (void 0 === d && (e = this.options.footers, d = e[a] &&
                    e[a][c] ? e[a][c] : _$_eb2b[3]), P(d) && 1 == this.parent.config.parseFormulas && (d = W.execute.call(this, d, c, null, !1)), this.options.applyMaskOnFooters && (e = ba[_$_eb2b[51]].call(this, c), d = r.editors.general.parseValue(c, a, d, this, e[1], f)), d instanceof Element || d instanceof HTMLDocument ? (f.innerHTML = _$_eb2b[3], f.appendChild(d)) : 1 == this.parent.config.stripHTML ? f.innerText = d : f.innerHTML = d)
            },
                b = function (c) {
                    c.getFooter = b[_$_eb2b[51]];
                    c.setFooter = b[_$_eb2b[11]];
                    c.resetFooter = b.reset;
                    c.refreshFooter = b.refresh;
                    c.getFooterValue =
                        b.value;
                    c.setFooterValue = b.value
                };
            b[_$_eb2b[51]] = function () {
                return this.options.footers
            };
            b[_$_eb2b[11]] = function (c) {
                if (c) {
                    var a = this.options.footers;
                    this.options.footers = c
                }
                this.options.footers != a && (aa.build.call(this), b.render.call(this), G.limited.call(this) && (this.scrollY.style.display = _$_eb2b[40], G.resetY.call(this)), J.call(this, _$_eb2b[172], {
                    data: this.options.footers
                }), F.call(this.parent, _$_eb2b[173], this, this.options.footers, a))
            };
            b.refresh = function () {
                var c = this.options.footers;
                if (c && c.length)
                    for (var a =
                        0; a < c.length; a++)
                        for (var d = 0; d < c[a].length; d++) k.call(this, d, a, c[a][d])
            };
            b.value = function (c, a, d) {
                if (void 0 === d) return this.options.footers[a][c];
                k.call(this, c, a, d);
                J.call(this, _$_eb2b[174], {
                    x: c,
                    y: a,
                    value: d
                });
                F.call(this.parent, _$_eb2b[175], this, c, a, d)
            };
            b.reset = function () {
                var c = this.options.footers;
                this.options.footers = null;
                this.footers.content = null;
                this.tfoot.innerHTML = _$_eb2b[3];
                J.call(this, _$_eb2b[176], {});
                F.call(this.parent, _$_eb2b[173], this, null, c)
            };
            b.create = function (c, a) {
                var d = this.footers.content[a][c];
                if (d && d.element) return d.element;
                d = this.footers.content[a][c] = {};
                var e = this.options.columns[c].align;
                d.element = document.createElement(_$_eb2b[148]);
                d.element.setAttribute(_$_eb2b[42], c);
                d.element.setAttribute(_$_eb2b[43], a);
                e && (d.element.style.textAlign = e);
                k.call(this, c, a);
                this.options.columns[c].type == _$_eb2b[49] && (d.element.style.display = _$_eb2b[40]);
                return d.element
            };
            b.build = function () {
                this.footers = {
                    element: this.tfoot,
                    content: []
                };
                if (this.options.footers) {
                    this.tfoot.innerHTML = _$_eb2b[3];
                    for (var c =
                        0; c < this.options.footers.length; c++) {
                        if (!this.footers.content[c]) {
                            this.footers.content[c] = [];
                            var a = document.createElement(_$_eb2b[146]),
                                d = document.createElement(_$_eb2b[148]);
                            d.innerHTML = _$_eb2b[149];
                            a.appendChild(d);
                            this.footers.element.appendChild(a)
                        }
                        for (a = 0; a < this.options.columns.length; a++) this.options.footers[c][a] || (this.options.footers[c][a] = _$_eb2b[3]), this.footers.content[c][a] || (this.footers.content[c][a] = {})
                    }
                }
            };
            b.render = function () {
                for (var c = this.options.footers, a = 0; a < c.length; a++)
                    for (var d =
                        0; d < c[a].length; d++) {
                        var e = aa.create.call(this, d, a);
                        this.tfoot.children[a].appendChild(e)
                    }
            };
            b.adjust = function (c, a, d) {
                var e = [],
                    f = this.options.footers,
                    g = this.footers.content;
                if (f)
                    if (1 == d) {
                        if (this.headers[c] && this.headers[c].parentNode) var h = this.headers[c] == this.thead.lastChild.lastChild ? !0 : !1,
                            l = !0;
                        for (d = 0; d < f.length; d++) {
                            for (var m = f[d].splice(c), p = c; p < c + a; p++) f[d][p] = _$_eb2b[3];
                            f[d] = f[d].concat(m);
                            m = g[d].splice(c);
                            for (p = c; p < c + a; p++) g[d][p] = {
                                element: b.create.call(this, p, d)
                            }, l && (1 == h ? this.tfoot.children[d].appendChild(g[d][p].element) :
                                this.tfoot.children[d].insertBefore(g[d][p].element, this.tfoot.children[d].children[c].nextSibling));
                            g[d] = g[d].concat(m)
                        }
                    } else
                        for (d = 0; d < f.length; d++) {
                            for (p = c; p < c + a; p++)(h = this.footers.content[d][p].element) && h.parentNode && h.parentNode.removeChild(h);
                            g[d].splice(c, a);
                            e[d] = f[d].splice(c, a)
                        }
                return e
            };
            return b
        }(),
        oa = function () {
            var k = function (b) {
                b.setNestedCell = k.cell;
                b.getNestedCell = k.cell;
                b.setNestedHeaders = k[_$_eb2b[11]];
                b.getNestedHeaders = k[_$_eb2b[51]];
                b.resetNestedHeaders = k.reset
            };
            k[_$_eb2b[51]] =
                function () {
                    return this.options.nestedHeaders
                };
            k[_$_eb2b[11]] = function (b) {
                k.reset.call(this);
                this.options.nestedHeaders = b;
                k.build.call(this);
                k.render.call(this);
                ka.refresh.call(this);
                U.call(this);
                J.call(this, _$_eb2b[177], {
                    data: b
                });
                F.call(this.parent, _$_eb2b[178], this, b)
            };
            k.reset = function () {
                var b = [],
                    c = this.thead.children;
                this.options.nestedHeaders = null;
                for (var a = 0; a < c.length; a++) c[a].classList.contains(_$_eb2b[179]) && b.push(c[a]);
                if (b.length)
                    for (; c = b.shift();) c.remove();
                this.nested = null;
                ka.refresh.call(this);
                U.call(this);
                J.call(this, _$_eb2b[180]);
                F.call(this.parent, _$_eb2b[178], this, [])
            };
            k.cell = function (b, c, a) {
                if (a) {
                    if (!Q.call(this.parent, this)) return !1;
                    var d = this.options.nestedHeaders[c][b],
                        e = this.nested.content[c][b].element;
                    void 0 !== a.title && (e.innerText = a.title);
                    void 0 !== a.tooltip && (e.title = a.tooltip);
                    void 0 !== a.colspan && (d.colspan = a.colspan, e.setAttribute(_$_eb2b[52], a.colspan), e.style.display = 0 == d.colspan ? _$_eb2b[40] : _$_eb2b[3]);
                    J.call(this, _$_eb2b[181], {
                        x: b,
                        y: c,
                        properties: a
                    });
                    F.call(this.parent,
                        _$_eb2b[182], this, b, c, a)
                } else return this.nested.content[c][b].element
            };
            k.range = function (b) {
                b = this.options.nestedHeaders[b];
                for (var c = [], a = 0, d = 0; d < b.length; d++) {
                    var e = parseInt(b[d].colspan);
                    c[d] = [a, a + e - 1];
                    a += e
                }
                return c
            };
            k.getColumns = function (b) {
                b = this.options.nestedHeaders[b];
                for (var c = [], a = 0; a < b.length; a++)
                    for (var d = b[a].colspan, e = 0; e < d; e++) c.push(a);
                return c
            };
            k.create = function (b, c) {
                if (this.nested.content[c][b]) return this.nested.content[c][b].element;
                var a = this.options.nestedHeaders[c][b] || {},
                    d =
                        this.nested.content[c][b] = {};
                a.colspan || (a.colspan = 1);
                a.align || (a.align = _$_eb2b[183]);
                a.title || (a.title = _$_eb2b[3]);
                var e = document.createElement(_$_eb2b[148]);
                e.setAttribute(_$_eb2b[184], b);
                e.setAttribute(_$_eb2b[185], c);
                e.setAttribute(_$_eb2b[52], a.colspan);
                e.setAttribute(_$_eb2b[186], a.align);
                e.innerHTML = a.title;
                a.tooltip && (e.title = a.tooltip);
                return d.element = e
            };
            k.build = function () {
                this.nested || (this.nested = {
                    content: []
                });
                if (this.options.nestedHeaders)
                    for (var b = 0; b < this.options.nestedHeaders.length; b++) {
                        this.nested.content[b] = [];
                        var c = document.createElement(_$_eb2b[148]);
                        c.innerHTML = _$_eb2b[149];
                        var a = document.createElement(_$_eb2b[146]);
                        a.classList.add(_$_eb2b[179]);
                        a.appendChild(c);
                        this.thead.insertBefore(a, this.thead.lastChild)
                    }
            };
            k.render = function () {
                for (var b, c = this.options.nestedHeaders, a = 0; a < c.length; a++)
                    for (var d = 0; d < this.thead.lastChild.children.length; d++) {
                        b = this.thead.lastChild.children[d].getAttribute(_$_eb2b[42]);
                        var e = k.getColumns.call(this, a);
                        void 0 !== e[b] && k.renderCell.call(this, e[b], a)
                    }
            };
            k.renderCell = function (b,
                c, a) {
                var d = oa.create.call(this, b, c);
                d.parentNode ? (b = d.getAttribute(_$_eb2b[52]), b++, d.setAttribute(_$_eb2b[52], b)) : (d.setAttribute(_$_eb2b[52], 1), a ? this.thead.children[c].insertBefore(d, this.thead.children[c].children[a]) : this.thead.children[c].appendChild(d), 0 == b && 0 < this.options.freezeColumns && (d.classList.add(_$_eb2b[50]), d.style.left = ya.call(this) ? _$_eb2b[160] : _$_eb2b[156]))
            };
            k.adjust = function (b, c, a, d) {
                var e = this.options.nestedHeaders;
                if (e)
                    if (1 == a)
                        if (d && d.nested)
                            for (a = 0; a < e.length; a++)
                                for (var f =
                                    0; f < e[a].length; f++) k.cell.call(this, f, a, {
                                        colspan: d.nested[a][f]
                                    });
                        else
                            for (a = 0; a < e.length; a++) {
                                if (f = k.getColumns.call(this, a)) {
                                    f = f[b];
                                    var g = parseInt(e[a][f].colspan) + c;
                                    k.cell.call(this, f, a, {
                                        colspan: g
                                    })
                                }
                            } else {
                        d = [];
                        for (a = 0; a < e.length; a++)
                            if (g = k.getColumns.call(this, a)) {
                                d[a] = [];
                                for (f = 0; f < c; f++) {
                                    var h = g[b + f];
                                    typeof d[a][h] == _$_eb2b[13] && (d[a][h] = e[a][h].colspan);
                                    e[a][h].colspan--
                                }
                                g = Object.keys(d[a]);
                                for (f = 0; f < g.length; f++) k.cell.call(this, g[f], a, {
                                    colspan: e[a][g[f]].colspan
                                })
                            } return d
                    }
            };
            return k
        }(),
        ma =
            function () {
                var k = function (b, c) {
                    var a = function (p) {
                        var n;
                        (n = this.results) ? p = n.indexOf(p) : n = this.rows;
                        p--;
                        for (var t = p; 0 <= t; t--)
                            if (p = this.results ? n[t] : t, this.rows[p] && this.rows[p].element) return p;
                        return 0
                    },
                        d = function (p) {
                            var n;
                            (n = this.results) ? p = n.indexOf(p) : n = this.rows;
                            p++;
                            for (var t = p; t < n.length; t++)
                                if (p = this.results ? n[t] : t, this.rows[p] && this.rows[p].element) return p;
                            return n[n.length - 1]
                        },
                        e = function (p, n) {
                            for (; 0 <= p; p--)
                                if (this.records[n][p].element && this.records[n][p].element.style.display != _$_eb2b[40]) return p;
                            return 0
                        },
                        f = function (p, n) {
                            for (; p < this.options.columns.length; p++)
                                if (this.records[n][p].element && this.records[n][p].element.style.display != _$_eb2b[40]) return p;
                            return this.options.columns - 1
                        },
                        g = this.getHighlighted(),
                        h = g[0],
                        l = g[1],
                        m = g[2];
                    g = g[3];
                    b = parseInt(b);
                    c = parseInt(c);
                    null != b && null != c && (0 < b - m ? (f = f.call(this, parseInt(m) + 1, c), b = parseInt(b)) : (f = parseInt(b), b = e.call(this, parseInt(h) - 1, c)), 0 < c - g ? (d = d.call(this, parseInt(g)), c = parseInt(c)) : (d = parseInt(c), c = a.call(this, parseInt(l))), b - f <= c - d ? (f = parseInt(h),
                        b = parseInt(m)) : (d = parseInt(l), c = parseInt(g)), this.setBorder(f, d, b, c, _$_eb2b[139]))
                };
                k.execute = function (b) {
                    if (!Q.call(this.parent, this)) return !1;
                    var c = this.borders.cloning;
                    if (b)
                        if (c && c.active) {
                            b = c.x1;
                            var a = c.y1,
                                d = c.x2,
                                e = c.y2
                        } else return !1;
                    else if (d = this.getSelected(), d.length) b = d[0].x, a = parseInt(d[d.length - 1].y) + 1, d = d[d.length - 1].x, e = this.options.data.length - 1;
                    else return !1;
                    var f, g = 0,
                        h = [],
                        l = [],
                        m = !1,
                        p = this.getHighlighted(),
                        n = Fa[_$_eb2b[51]].call(this, p, !1, !1),
                        t = this.getSelected();
                    if (f = this.getStyle(t,
                        !0)) {
                        var v = Object.keys(f);
                        if (v.length)
                            for (var u = 0; u < v.length; u++) h.push(f[v[u]])
                    }
                    if (p[0] == b) var y = a < p[1] ? a - p[1] : 1,
                        z = 0;
                    else z = b < p[0] ? b - p[0] : 1, y = 0;
                    v = 0;
                    b = parseInt(b);
                    d = parseInt(d);
                    a = parseInt(a);
                    e = parseInt(e);
                    for (var D = a; D <= e; D++)
                        if (!this.results || -1 != this.results.indexOf(D)) {
                            void 0 == n[g] && (g = 0);
                            f = 0;
                            p[0] != b && (z = b < p[0] ? b - p[0] : 1);
                            for (u = b; u <= d; u++) {
                                if (!(0 != m || this.records[D][u].element && this.records[D][u].element.classList.contains(_$_eb2b[18]))) {
                                    if ((!c || 1 != c.active) && M.call(this, u, D)) {
                                        m = !0;
                                        continue
                                    }
                                    void 0 ==
                                        n[g] ? f = 0 : void 0 == n[g][f] && (f = 0);
                                    var A = n[g][f];
                                    A && !n[1] && 1 == this.parent.config.autoIncrement && (this.options.columns[u].type == _$_eb2b[55] || this.options.columns[u].type == _$_eb2b[14] || 1 == this.options.columns[u].shiftFormulas ? P(A) ? A = W.shiftFormula(A, z, D - p[1]) : A == Number(A) && (A = Number(A) + y) : this.options.columns[u].type == _$_eb2b[187] && (A = new Date(A), A.setDate(A.getDate() + y), A = A.getFullYear() + _$_eb2b[188] + jSuites.two(parseInt(A.getMonth() + 1)) + _$_eb2b[188] + jSuites.two(A.getDate()) + _$_eb2b[133] + _$_eb2b[189]));
                                    A = {
                                        x: u,
                                        y: D,
                                        value: A
                                    };
                                    h && null != h[v] && (A.style = h[v], v++, v >= h.length && (v = 0));
                                    l.push(A)
                                }
                                f++;
                                p[0] != b && z++
                            }
                            g++;
                            y++
                        } l.length && (t[0].x < b && (b = t[0].x), t[0].y < a && (a = t[0].y), t[t.length - 1].x > d && (d = t[t.length - 1].x), t[t.length - 1].y > e && (e = t[t.length - 1].y), this.setValue(l), this.resetBorders(_$_eb2b[139], !0), this.updateSelectionFromCoords(b, a, d, e))
                };
                k.end = function (b) {
                    ma.event = !1;
                    ma.execute.call(this, !0)
                };
                k.cancel = function (b) {
                    ma.event = !1;
                    this.resetBorders(_$_eb2b[139], !0)
                };
                return k
            }(),
        Fa = function () {
            var k = function (b) {
                b.getData =
                    k[_$_eb2b[51]];
                b.getJson = k[_$_eb2b[51]];
                b.setData = k[_$_eb2b[11]];
                b.loadData = k.load;
                b.getRowData = k.getRowData;
                b.setRowData = k.setRowData;
                b.getColumnData = k.getColumnData;
                b.setColumnData = k.setColumnData;
                b.updateData = k.update;
                b.refresh = k.refresh;
                b.data = k[_$_eb2b[51]];
                b.download = k.download;
                b.copy = k.copy;
                b.paste = k.paste;
                b.cut = function () {
                    return b.copy(!0)
                }
            };
            k[_$_eb2b[51]] = function (b, c, a, d) {
                !0 === b && (b = this.getHighlighted());
                var e = b ? !0 : !1,
                    f = [],
                    g = this.options.columns.length,
                    h = this.options.data.length;
                void 0 ===
                    d && (d = this.dataType && typeof a == _$_eb2b[13] ? 1 : 0);
                for (var l = 0; l < h; l++) {
                    var m = null;
                    var p = d ? {} : [];
                    for (var n = 0; n < g; n++) {
                        var t;
                        if (!(t = !e)) {
                            t = b[1];
                            var v = b[2],
                                u = b[3];
                            t = n >= b[0] && n <= v && l >= t && l <= u ? !0 : !1
                        }
                        if (t && (!e || !this.results || -1 != this.results.indexOf(l))) {
                            t = M.call(this, n, l);
                            if (c) {
                                if (e && !1 === this.rows[l].visible) continue;
                                m = ea.processed.call(this, n, l);
                                void 0 !== m && (t = m)
                            }
                            a && t.match && (t.match(a) || t.match(/\n/) || t.match(/"/)) && (t = t.replace(new RegExp(_$_eb2b[190], _$_eb2b[191]), _$_eb2b[192]), t = _$_eb2b[190] + t + _$_eb2b[190]);
                            d ? jSuites.path.call(p, _$_eb2b[3] + Z.call(this, n), t) : p.push(t);
                            m = !0
                        }
                    }
                    m && (a ? f.push(p.join(a)) : f.push(p))
                }
                return a ? f.join(_$_eb2b[10]) + _$_eb2b[10] : f
            };
            k.parse = function () {
                for (var b, c = {}, a = 0, d, e = [], f = this.options.data, g = 0; g < f.length; g++) b = null, d = g, f[g].data && (Array.isArray(f[g].data) || typeof f[g].data == _$_eb2b[84]) && (typeof f[g].id !== _$_eb2b[13] && (b = parseInt(f[g].id)), typeof f[g].row !== _$_eb2b[13] && (d = f[g].row), e[d] = f[g].data, f[g] = []), d > a && (a = d), b && (c[d] = b);
                if (Object.keys(e).length)
                    for (g = 0; g <= a; g++) e[g] &&
                        (f[g] = e[g]);
                a = this.getPrimaryKey();
                if (!1 !== a)
                    for (a = Z.call(this, a), g = 0; g < f.length; g++) c[g] || !1 === a || (b = parseInt(f[g][a]), 0 < b && (c[g] = b));
                return c
            };
            k.type = function () {
                for (var b = this.dataType = 0; b < this.options.columns.length; b++) typeof this.options.columns[b].name !== _$_eb2b[13] && (this.dataType = 1)
            };
            k[_$_eb2b[11]] = function (b) {
                b = k.standardize(b);
                var c = {},
                    a = [],
                    d = {};
                var e = Object.keys(b[0]).length - this.options.columns.length;
                0 < e && this.insertRow(e);
                var f = b.length - this.options.data.length;
                0 < f && this.insertRow(f);
                for (var g = 0; g < this.options.data.length; g++) {
                    void 0 === b[g] && (b[g] = {
                        row: g,
                        data: []
                    });
                    for (var h = 0; h < this.options.columns.length; h++) b[g].data[h] || (b[g].data[h] = _$_eb2b[3])
                }
                for (h = 0; h < this.options.columns.length; h++) {
                    var l = Z.call(this, h);
                    d[l] = h
                }
                for (g = 0; g < b.length; g++) {
                    f = b[g].row;
                    l = Object.keys(b[g].data);
                    for (h = 0; h < l.length; h++) e = null, void 0 !== d[l[h]] ? e = d[l[h]] : -1 < l[h] && (e = parseInt(l[h])), null !== e && a.push({
                        x: e,
                        y: f,
                        value: b[g].data[l[h]]
                    });
                    b[g].id && (c[f] = b[g].id)
                }
                ta.setId.call(this, c);
                ea[_$_eb2b[11]].call(this,
                    a, null, !0)
            };
            k.build = function () {
                this.results = null;
                this.rows = [];
                this.records = [];
                this.sequence = 0;
                k.type.call(this);
                if (this.options.data) {
                    var b = k.parse.call(this),
                        c = this.options.data.length;
                    this.options.minDimensions[1] > c && (c = this.options.minDimensions[1]);
                    for (var a = 0; a < c; a++) {
                        var d = b[a] ? b[a] : null;
                        na.row.call(this, a, d)
                    }
                }
            };
            k.load = function (b) {
                this.resetBorders();
                this.resetSelection();
                this.tbody.innerText = _$_eb2b[3];
                this.options.data = b;
                k.build.call(this);
                G.call(this);
                X.update.call(this)
            };
            k.refresh = function (b) {
                if (typeof b !==
                    _$_eb2b[13])
                    for (var c = 0; c < this.options.columns.length; c++) {
                        var a = this.records[b] && this.records[b][c].element ? this.records[b][c].element : null,
                            d = ba[_$_eb2b[51]].call(this, c, b);
                        typeof d[0].updateCell == _$_eb2b[85] && d[0].updateCell(a, M.call(this, c, b), c, b, this, d[1]);
                        a && N.applyOverflow.call(this, a, c, b)
                    } else if (this.options.url) {
                        var e = this;
                        1 == this.parent.config.loadingSpin && jSuites.loading.show();
                        jSuites.ajax({
                            url: this.options.url,
                            method: _$_eb2b[58],
                            dataType: _$_eb2b[59],
                            success: function (f) {
                                e.setData(f);
                                jSuites.loading.hide()
                            }
                        })
                    }
            };
            k.getRowData = function (b, c) {
                if (typeof this.options.data[b] === _$_eb2b[13]) return !1;
                if (c) {
                    if (this.dataType) {
                        c = {};
                        for (var a = 0; a < this.options.columns.length; a++) {
                            var d = _$_eb2b[3] + Z.call(this, a),
                                e = _$_eb2b[3] + ea.processed.call(this, a, b, !0);
                            jSuites.path.call(c, d, e)
                        }
                    } else
                        for (c = [], a = 0; a < this.options.columns.length; a++) c.push(ea.processed.call(this, a, b, !0));
                    return c
                }
                return JSON.parse(JSON.stringify(this.options.data[b]))
            };
            k.setRowData = function (b, c, a) {
                for (var d = [], e = 0; e < this.options.columns.length; e++) typeof c[e] ===
                    _$_eb2b[13] && (c[e] = _$_eb2b[3]), d.push({
                        x: e,
                        y: b,
                        value: c[e]
                    });
                d.length && this.setValue(d, null, a)
            };
            k.getColumnData = function (b, c) {
                if (typeof this.options.columns[b] === _$_eb2b[13]) return !1;
                for (var a = [], d = 0; d < this.options.data.length; d++) {
                    var e = c ? ea.processed.call(this, b, d, !0) : M.call(this, b, d);
                    a.push(e)
                }
                return a
            };
            k.setColumnData = function (b, c, a) {
                for (var d = [], e = 0; e < this.rows.length; e++) typeof c[e] === _$_eb2b[13] && (c[e] = _$_eb2b[3]), d.push({
                    x: b,
                    y: e,
                    value: c[e]
                });
                d.length && this.setValue(d, null, a)
            };
            k.copy = function (b) {
                var c =
                    this,
                    a = this.getHighlighted(),
                    d = function () {
                        c.setBorder(a[0], a[1], a[2], a[3], _$_eb2b[129]);
                        c.borders.copying.clear = 1 == b ? !0 : !1
                    },
                    e = k[_$_eb2b[51]].call(this, a, !0, _$_eb2b[193]),
                    f = F.call(this.parent, _$_eb2b[194], this, a, e);
                if (f) e = f;
                else if (!1 === f) return !1;
                navigator.clipboard ? navigator.clipboard.writeText(e).then(d) : (this.parent.textarea.value = e, this.parent.textarea.select(), document.execCommand(_$_eb2b[195]), d());
                return e
            };
            k.paste = function (b, c, a) {
                if (!Q.call(this.parent, this)) return !1;
                var d = this,
                    e = function () {
                        d.borders.copying &&
                            (d.resetBorders(_$_eb2b[129], !0), d.borders.copying.clear = !1)
                    },
                    f = null,
                    g = this.options.style,
                    h = null,
                    l = null,
                    m = [];
                if (this.borders.copying) {
                    var p = [this.borders.copying.x1, this.borders.copying.y1, this.borders.copying.x2, this.borders.copying.y2];
                    h = p[0];
                    l = p[1];
                    var n = jSuites.hash(a),
                        t = jSuites.hash(k[_$_eb2b[51]].call(this, p, !0, _$_eb2b[193]));
                    if (n != t) e();
                    else {
                        var v = 0,
                            u = 0;
                        a = k[_$_eb2b[51]].call(this, p, !1, !1);
                        for (n = p[1]; n <= p[3]; n++)
                            if (!this.results || -1 != this.results.indexOf(n)) {
                                m[u] = [];
                                v = 0;
                                for (t = p[0]; t <= p[2]; t++) f =
                                    E.getColumnNameFromCoords(t, n), m[u][v] = g && g[f] ? g[f] : _$_eb2b[196], v++;
                                u++
                            }
                    }
                }
                b = parseInt(b);
                c = parseInt(c);
                if (a) {
                    Array.isArray(a) || (a = E.parseCSV(a, _$_eb2b[193]));
                    if (n = F.call(this.parent, _$_eb2b[197], this, a, b, c, m)) a = n;
                    else if (!1 === n) return !1;
                    100 < a.length && !this.options.pagination && !G.limited.call(this) && console.log(_$_eb2b[198]);
                    if (f = this.getHighlighted()) {
                        v = [];
                        u = [];
                        var y = [];
                        g = [];
                        var z = 0,
                            D = 0;
                        for (n = f[1]; n <= f[3]; n++)
                            if (!this.results || -1 != this.results.indexOf(n)) {
                                v = [];
                                u = [];
                                for (t = f[0]; t <= f[2]; t++) v.push(a[D][z]),
                                    m && m[D] && m[D][z] ? u.push(m[D][z]) : u.push(_$_eb2b[3]), z++, z >= a[0].length && (z = 0);
                                y.push(v);
                                g.push(u);
                                D++;
                                D >= a.length && (D = 0)
                            } 0 == y.length % a.length && 0 == y[0].length % a[0].length && (a = y, m = g)
                    }
                    g = null;
                    f = [];
                    if (this.borders.copying && !0 === this.borders.copying.clear) {
                        for (n = p[1]; n <= p[3]; n++)
                            for (t = p[0]; t <= p[2]; t++)(!this.results || this.results && 0 <= this.results.indexOf(n)) && f.push({
                                x: t,
                                y: n,
                                value: _$_eb2b[3],
                                style: _$_eb2b[3]
                            });
                        e()
                    }
                    n = b + a[0].length - this.options.columns.length;
                    0 < n && this.insertColumn(n);
                    n = c + a.length - this.rows.length;
                    0 < n && this.insertRow(n);
                    u = v = b;
                    z = y = c;
                    D = null;
                    this.results && (g = this.results.indexOf(c));
                    for (n = 0; n < a.length; n++)
                        for (t = 0; t < a[n].length; t++) {
                            null !== h && null !== l && a[n][t][0] == _$_eb2b[12] && (a[n][t] = W.shiftFormula(a[n][t], b - h, c - l));
                            u = b + t;
                            z = c + n;
                            this.results && (z = this.results[g + n]);
                            var A = {
                                x: u,
                                y: z,
                                value: a[n][t]
                            };
                            m && m[n] && m[n][t] && (A.style = m[n][t]);
                            f.push(A);
                            null === D && za.isSelected.call(this, u, z, p) && (D = !0)
                        }
                    D && e();
                    f.length && (ea[_$_eb2b[11]].call(this, f, null, this.parent.config.forceUpdateOnPaste ? !0 : !1), F.call(this.parent,
                        _$_eb2b[199], this, f), this.updateSelectionFromCoords(v, y, u, z))
                }
            };
            k.download = function (b, c) {
                if (this.parent.config.allowExport) {
                    void 0 === c && (c = !0);
                    var a = _$_eb2b[3];
                    if (1 == b || 1 == this.parent.config.includeHeadersOnDownload) a += this.getHeaders().replace(/\s+/gm, _$_eb2b[133]) + _$_eb2b[10];
                    a += k[_$_eb2b[51]].call(this, null, c, this.options.csvDelimiter);
                    c = new Blob([_$_eb2b[201] + a], {
                        type: _$_eb2b[202]
                    });
                    window.navigator && window.navigator.msSaveOrOpenBlob ? window.navigator.msSaveOrOpenBlob(c, this.options.csvFileName +
                        _$_eb2b[203]) : (b = document.createElement(_$_eb2b[204]), c = URL.createObjectURL(c), b.href = c, b.setAttribute(_$_eb2b[205], this.options.csvFileName + _$_eb2b[203]), document.body.appendChild(b), b.click(), b.parentNode.removeChild(b))
                } else console.error(_$_eb2b[200])
            };
            k.standardize = function (b) {
                b || (b = []);
                Array.isArray(b) || (b = [b]);
                void 0 == b[0] || Array.isArray(b[0]) || typeof b[0] == _$_eb2b[84] || (b = [b]);
                for (var c = 0; c < b.length; c++) {
                    var a = {};
                    Array.isArray(b[c]) || void 0 == b[c].data ? a.data = b[c] : a = b[c];
                    a.row = void 0 == b[c].row ?
                        c : b[c].row;
                    b[c] = a
                }
                return b
            };
            return k
        }(),
        Wa = function () {
            var k = function (b) {
                b.getLabel = k[_$_eb2b[51]];
                b.getLabelFromCoords = k[_$_eb2b[51]]
            };
            k[_$_eb2b[51]] = function (b, c, a) {
                if (typeof b == _$_eb2b[17]) {
                    b = E.getCoordsFromColumnName(b);
                    var d = b[0];
                    c = b[1]
                } else d = b;
                return this.getProcessed(d, c, a)
            };
            return k
        }(),
        Ia = function () {
            var k = function (b) {
                b.getDefinedNames = k[_$_eb2b[51]];
                b.setDefinedNames = k[_$_eb2b[11]]
            };
            k[_$_eb2b[11]] = function (b) {
                var c = Object.keys(b);
                if (c.length) {
                    for (var a = 0; a < c.length; a++) this.parent.config.definedNames[c[a]] =
                        b[c[a]], b[c[a]] || delete this.parent.config.definedNames[c[a]];
                    J.call(this, _$_eb2b[206], [b])
                }
            };
            k[_$_eb2b[51]] = function (b) {
                var c = this.parent.config.definedNames;
                return b ? c[b] : c
            };
            k.updateAll = function (b, c) {
                var a, d = {};
                if (a = this.parent.config.definedNames) {
                    var e = Object.keys(a);
                    if (e.length) {
                        for (var f = this.getWorksheetName(), g = 0; g < e.length; g++) {
                            var h = W.update.call(this, a[e[g]], b, c, f);
                            h !== a[e[g]] && (d[e[g]] = h)
                        }
                        k[_$_eb2b[11]].call(this, d)
                    }
                }
            };
            k.updateWorksheetName = function (b, c) {
                var a, d = {};
                if (a = this.parent.config.definedNames) {
                    var e =
                        Object.keys(a);
                    if (e.length) {
                        for (var f = 0; f < e.length; f++) {
                            var g = W.updateWorksheetName(a[e[f]], b, c);
                            g !== a[e[f]] && (d[e[f]] = g)
                        }
                        k[_$_eb2b[11]].call(this, d)
                    }
                }
            };
            return k
        }(),
        la = function () {
            var k = [_$_eb2b[207], _$_eb2b[208], _$_eb2b[209], _$_eb2b[210], _$_eb2b[211]],
                b = [_$_eb2b[188], _$_eb2b[152], _$_eb2b[212], _$_eb2b[213], _$_eb2b[214], _$_eb2b[215], _$_eb2b[1], _$_eb2b[216], _$_eb2b[25], _$_eb2b[217], _$_eb2b[12], _$_eb2b[218], _$_eb2b[219], _$_eb2b[220]],
                c = [],
                a = function () {
                    if (0 < c.length) {
                        for (var f, g = 0; g < c.length; g++) f = -1 ===
                            c[g].indexOf(_$_eb2b[136]) ? this : I(c[g].split(_$_eb2b[136])[0]), ka.destroy.call(f, c[g]);
                        c = []
                    }
                },
                d = function (f) {
                    a.call(this);
                    for (var g = 0, h = [], l, m, p, n = 0; n < f.children.length; n++)
                        if (l = f.children[n].getAttribute(_$_eb2b[221])) {
                            h[l] || (h[l] = k[g++], 4 < g && (g = 0));
                            f.children[n].style.color = h[l]; - 1 === l.indexOf(_$_eb2b[136]) ? (m = this, p = l) : (m = l.split(_$_eb2b[136]), p = m[1], m = I(m[0]));
                            if (0 <= p.indexOf(_$_eb2b[137])) {
                                var t = p.split(_$_eb2b[137]);
                                p = E.getCoordsFromColumnName(t[0]);
                                t = E.getCoordsFromColumnName(t[1])
                            } else t = p =
                                E.getCoordsFromColumnName(p);
                            za[_$_eb2b[11]].call(m, p[0], p[1], t[0], t[1], null, l, h[l]);
                            c.push(l)
                        }
                },
                e = function (f, g) {
                    g && typeof g != _$_eb2b[85] || (g = {
                        type: _$_eb2b[222],
                        onchange: g
                    });
                    g.type === _$_eb2b[222] ? (f.classList.add(_$_eb2b[223]), f.addEventListener(_$_eb2b[224], function (h) {
                        h = h.changedTouches && h.changedTouches[0] ? h.changedTouches[0].clientX : h.clientX;
                        var l = this.getBoundingClientRect();
                        24 > l.width - (h - l.left) && (e.current = f, f.classList.add(_$_eb2b[225]), f.classList.add(_$_eb2b[94]))
                    })) : (f.addEventListener(_$_eb2b[226],
                        function (h) {
                            P(f.innerText) && (f.classList.add(_$_eb2b[94]), r.current && setTimeout(function () {
                                f.worksheet = r.current;
                                e.parse.call(r.current, f);
                                var l = document.createTextNode(_$_eb2b[3]);
                                f.appendChild(l);
                                jSuites.focus(f)
                            }))
                        }), f.addEventListener(_$_eb2b[227], function (h) {
                            P(f.innerText) ? f.classList.add(_$_eb2b[94]) : f.classList.contains(_$_eb2b[94]) && (f.classList.remove(_$_eb2b[94]), f.innerText = f.innerText, f.worksheet && a.call(f.worksheet));
                            h.preventDefault()
                        }));
                    f.classList.add(_$_eb2b[228]);
                    f.classList.add(_$_eb2b[227]);
                    f.setAttribute(_$_eb2b[229], !0);
                    f.addEventListener(_$_eb2b[230], function (h) {
                        f.worksheet && (a.call(f.worksheet), typeof f.onchange == _$_eb2b[85] && f.onchange.call(f), f.worksheet = null)
                    });
                    f.val = function (h) {
                        if (void 0 === h) return f.innerText;
                        f.innerText = h
                    };
                    g.onchange && (f.onchange = g.onchange)
                };
            e.current = null;
            e.parse = function (f) {
                for (var g = _$_eb2b[3], h = _$_eb2b[3], l = window.getSelection(), m = document.createRange(), p = E.getCaretIndex.call(this.parent, f), n = W.tokenize(f.innerText.replace(/(\r\n|\n|\r)/gm, _$_eb2b[3])),
                    t = 0; t < n.length; t++) n[t] && W.tokenIdentifier.test(n[t].trim()) ? (t == n.length - 1 && (h = _$_eb2b[231]), g += _$_eb2b[232] + h + _$_eb2b[233] + n[t].trim().replace(/\$/g, _$_eb2b[3]) + _$_eb2b[234] + n[t] + _$_eb2b[235]) : g += n[t];
                f.innerHTML = g;
                g = null;
                for (t = 0; t < f.childNodes.length && !(g = f.childNodes[t], h = g.tagName ? g.innerText.length : g.length, 0 >= p - h); t++) p -= h;
                g && (g.tagName ? m.setStart(g.firstChild, p) : (p > g.length && (p = g.length), m.setStart(g, p)), l.removeAllRanges(), l.addRange(m));
                d.call(this, f)
            };
            e.close = function (f) {
                a.call(this);
                f ?
                    f.classList.remove(_$_eb2b[94]) : e.current && (typeof e.current.onchange == _$_eb2b[85] && e.current.onchange.call(e.current), e.current.classList.remove(_$_eb2b[225]), e.current.classList.remove(_$_eb2b[94]), e.current.worksheet = null, e.current = null)
            };
            e.getName = function (f, g, h, l, m) {
                f = E.getColumnNameFromCoords(f, g);
                h = E.getColumnNameFromCoords(h, l);
                m.ctrlKey && (f = h);
                return f !== h ? (m = f + _$_eb2b[137] + h, m = W.getTokensFromRange.call(this, m), W.getRangeFromTokens.call(this, m)) : f
            };
            e.range = function (f, g, h, l, m) {
                e.current.getAttribute(_$_eb2b[236]) ==
                    _$_eb2b[237] && (f = h, g = l);
                f = e.getName.call(this, f, g, h, l, m);
                g = document.createElement(_$_eb2b[238]);
                g.innerText = f;
                g.setAttribute(_$_eb2b[221], f);
                e.current.innerText = _$_eb2b[3];
                e.current.appendChild(g);
                d.call(this, e.current);
                jSuites.focus(g);
                m.preventDefault()
            };
            e.update = function (f, g, h, l, m, p, n) {
                p.getAttribute(_$_eb2b[236]) == _$_eb2b[237] && (f = h, g = l);
                var t = jSuites.getNode();
                if (t) {
                    var v = e.getName.call(this, f, g, h, l, m);
                    this !== n && (v = n.getWorksheetName() + _$_eb2b[136] + v);
                    f = function (u) {
                        var y = document.createElement(_$_eb2b[238]);
                        y.innerText = v;
                        y.setAttribute(_$_eb2b[221], v);
                        t.parentNode === p && !1 === u ? t.parentNode.insertBefore(y, t.nextSibling) : p.appendChild(y);
                        t = y
                    };
                    if (t.getAttribute(_$_eb2b[221])) m.ctrlKey ? (p.appendChild(document.createTextNode(_$_eb2b[25])), f(!0)) : (t.innerText = v, t.setAttribute(_$_eb2b[221], v));
                    else {
                        if (!t.innerText || t.innerText !== _$_eb2b[3] && -1 == b.indexOf(t.innerText.slice(-1)) || t.nextElementSibling && t.nextElementSibling.getAttribute(_$_eb2b[221])) return !1;
                        f(!1)
                    }
                    jSuites.focus(t);
                    d.call(this, p);
                    if (typeof p.onchange ==
                        _$_eb2b[85]) p.onchange();
                    m.preventDefault();
                    return !0
                }
                return !1
            };
            e.onkeydown = function (f) {
                f.target.worksheet = this;
                if (f.ctrlKey) {
                    if (67 == f.which || 88 == f.which) f = window.getSelection().toString().replace(/(\r\n|\n|\r)/gm, _$_eb2b[3]), navigator.clipboard ? navigator.clipboard.writeText(f) : (this.parent.textarea.value = f, this.parent.textarea.select(), document.execCommand(_$_eb2b[195]))
                } else if (36 == f.which) {
                    var g = window.getSelection(),
                        h = document.createRange();
                    h.setStart(f.target, 0);
                    g.removeAllRanges();
                    g.addRange(h)
                } else 35 ==
                    f.which ? jSuites.focus(f.target) : 187 == f.which ? !f.target.innerText.replace(/(\r\n|\n|\r)/gm, _$_eb2b[3]) && f.target.getAttribute(_$_eb2b[96]) && f.target.removeAttribute(_$_eb2b[96]) : 13 == f.which && f.target.blur()
            };
            e.palette = function (f) {
                k = f
            };
            return e
        }(),
        W = function () {
            var k = new RegExp(/^(('.*?'!)|(\w*!))?(\$?[A-Z]+\$?[0-9]+)(:\$?[A-Z]+\$?[0-9]+)?$/i),
                b = function (g) {
                    g = c(g);
                    for (var h = [_$_eb2b[188], _$_eb2b[152], _$_eb2b[212], _$_eb2b[213], _$_eb2b[214], _$_eb2b[239]], l = [_$_eb2b[215], _$_eb2b[240], _$_eb2b[1], _$_eb2b[2],
                    _$_eb2b[216], _$_eb2b[241], _$_eb2b[25], _$_eb2b[217], _$_eb2b[12], _$_eb2b[218], _$_eb2b[219], _$_eb2b[220]
                    ], m = _$_eb2b[3], p = [], n = 0, t = 0; t < g.length; t++) {
                        if (g[t] == _$_eb2b[190] || g[t] == _$_eb2b[134]) n = 0 == n ? 1 : 0;
                        0 == n && (-1 < h.indexOf(g[t]) || -1 < l.indexOf(g[t])) ? (m && (p.push(m), m = _$_eb2b[3]), p.push(g[t])) : m += g[t]
                    }
                    m && p.push(m);
                    for (t = 0; t < p.length; t++) 0 == n && (p[t] = p[t].trim());
                    return p
                },
                c = function (g, h) {
                    var l = _$_eb2b[3],
                        m = 0;
                    g = g.replace(new RegExp(_$_eb2b[242], _$_eb2b[191]), _$_eb2b[218]);
                    g = g.replace(new RegExp(_$_eb2b[243],
                        _$_eb2b[191]), _$_eb2b[219]);
                    for (var p = [_$_eb2b[12], _$_eb2b[136], _$_eb2b[219], _$_eb2b[218]], n = 0; n < g.length; n++) g[n] == _$_eb2b[190] && (m = 0 == m ? 1 : 0), 1 == m ? l += g[n] : (l += g[n].toUpperCase(), 1 == h && 0 < n && g[n] == _$_eb2b[12] && -1 == p.indexOf(g[n - 1]) && -1 == p.indexOf(g[n + 1]) && (l += _$_eb2b[12]));
                    return l
                },
                a = function (g) {
                    g.setFormula = a[_$_eb2b[11]];
                    g.executeFormula = a.execute;
                    g.resetArray = a.resetArray;
                    g.formula = []
                };
            a.run = function (g, h) {
                var l = _$_eb2b[3],
                    m = Object.keys(h);
                if (m.length) {
                    for (var p = {}, n = 0; n < m.length; n++) {
                        var t = m[n].replace(/!/g,
                            _$_eb2b[27]);
                        0 < t.indexOf(_$_eb2b[27]) && (t = t.split(_$_eb2b[27]), p[t[0]] = !0)
                    }
                    t = Object.keys(p);
                    for (n = 0; n < t.length; n++) l += _$_eb2b[244] + t[n] + _$_eb2b[245];
                    for (n = 0; n < m.length; n++) t = m[n].replace(/!/g, _$_eb2b[27]), null === h[m[n]] || jSuites.isNumeric(h[m[n]]) || (p = h[m[n]].match(/(('.*?'!)|(\w*!))?(\$?[A-Z]+\$?[0-9]*):(\$?[A-Z]+\$?[0-9]*)?/g)) && p.length && (h[m[n]] = e(p, h[m[n]])), l = 0 < t.indexOf(_$_eb2b[27]) ? l + (t + _$_eb2b[246] + h[m[n]] + _$_eb2b[247]) : l + (_$_eb2b[244] + t + _$_eb2b[246] + h[m[n]] + _$_eb2b[247])
                }
                g = g.replace(/\$/g,
                    _$_eb2b[3]);
                (p = g.match(/(('.*?'!)|(\w*!))?(\$?[A-Z]+\$?[0-9]*):(\$?[A-Z]+\$?[0-9]*)?/g)) && p.length && (g = e(p, g));
                g = g.replace(/!/g, _$_eb2b[27]);
                g = (new Function(l + _$_eb2b[248] + g))();
                null === g && (g = 0);
                return g
            };
            a[_$_eb2b[11]] = function () { };
            var d = function (g) {
                g = c(g, !0);
                return g.replace(/\$/g, _$_eb2b[3])
            },
                e = function (g, h) {
                    for (var l = 0; l < g.length; l++) {
                        var m = a.getTokensFromRange.call(this, g[l]);
                        h = h.replace(g[l], _$_eb2b[216] + m.join(_$_eb2b[25]) + _$_eb2b[241])
                    }
                    return h
                },
                f = function (g, h, l) {
                    for (var m = this.config.definedNames,
                        p = 0; p < g.length; p++) g[p] && m && m[g[p]] && (h = h.replace(g[p], m[g[p]]), l && (l[g[p]] = m[g[p]]));
                    return d(h)
                };
            a.resetArray = function (g, h) {
                var l;
                if (l = this.records[h][g].a) {
                    for (var m = 0; m < l.length; m++)
                        for (var p = 0; p < l[m].length; p++) this.options.data[h + m][g + p] || (this.records[h + m][g + p].v = _$_eb2b[3], this.records[h + m][g + p].element && (this.records[h + m][g + p].element.innerText = _$_eb2b[3]));
                    this.records[h][g].a = null
                }
            };
            a.execute = function (g, h, l, m, p) {
                if (1 == this.parent.processing) return null !== h && null !== l && this.parent.queue.push([this,
                    h, l
                ]), _$_eb2b[3];
                if (typeof this.parent.config.onbeforeformula == _$_eb2b[85]) {
                    var n = F.call(this.parent, _$_eb2b[249], this, g, h, l);
                    if (!1 === n) return g;
                    n && (g = n)
                }
                var t = [],
                    v = function (Y, q, x, w) {
                        if (void 0 != q && void 0 != x) {
                            var C = E.getColumnNameFromCoords(q, x);
                            x = w.getWorksheetName() + _$_eb2b[27] + C;
                            t[x] || (t[x] = 0);
                            t[x]++;
                            if (3 < t[x]) throw {
                                error: _$_eb2b[250],
                                message: _$_eb2b[251],
                                reference: x
                            };
                        } else C = null;
                        p || (Y = d(Y));
                        var H = Y,
                            L = H.match(/[A-Z_]+[A-Z0-9_\.]*/g);
                        L && L.length && (H = f.call(w.parent, L, H, u));
                        (L = H.match(/(('.*?'!)|(\w*!))?(\$?[A-Z]+\$?[0-9]*):(\$?[A-Z]+\$?[0-9]*)?/g)) &&
                            L.length && (H = e.call(w, L, H));
                        L = [];
                        H = b(H);
                        for (var O = 0; O < H.length; O++) H[O] && k.test(H[O]) && L.push(H[O]);
                        if (L) {
                            if (-1 < L.indexOf(C)) throw {
                                error: _$_eb2b[252],
                                message: _$_eb2b[253]
                            };
                            for (H = 0; H < L.length; H++) {
                                O = L[H].split(_$_eb2b[136]);
                                O[1] ? (q = O[1], O = I(O[0]), x = w.getWorksheetName() + _$_eb2b[27] + C) : (q = O[0], O = w, x = C);
                                if (typeof O == _$_eb2b[13] || typeof O.formula == _$_eb2b[13]) throw {
                                    error: _$_eb2b[254],
                                    message: _$_eb2b[255]
                                };
                                x && (O.formula[q] || (O.formula[q] = []), 0 > O.formula[q].indexOf(x) && O.formula[q].push(x));
                                if (!u[L[H]]) {
                                    x =
                                        E.getCoordsFromColumnName(q);
                                    q = x[0];
                                    x = x[1];
                                    var R = void 0 !== O.records[x][q].v ? O.records[x][q].v : M.call(O, q, x);
                                    u[L[H]] || ((_$_eb2b[3] + R).substr(0, 1) == _$_eb2b[12] ? u[L[H]] = v(R, q, x, O) : (R === _$_eb2b[3] || null === R || void 0 === R ? R = null : jSuites.isNumeric(R) || (R = _$_eb2b[190] + R + _$_eb2b[190]), u[L[H]] = R))
                                }
                            }
                        }
                        return Y.substr(1)
                    },
                    u = {};
                try {
                    g = v(g, h, l, this);
                    if (g == _$_eb2b[3]) return _$_eb2b[3];
                    0 == g.indexOf(_$_eb2b[256]) && (p = !0);
                    var y = p ? a.run(g, u) : r.formula(g, u, h, l, this);
                    if (!1 !== m && void 0 != h && void 0 != l)
                        if (y instanceof Date) this.records[l][h].v =
                            jSuites.calendar.dateToNum(y);
                        else if (Array.isArray(y)) {
                            m = [];
                            n = null;
                            for (var z = 0; z < y.length; z++)
                                for (var D = 0; D < y[z].length; D++) {
                                    (0 < D || 0 < z) && this.options.data[l + z][h + D] !== _$_eb2b[3] && (n = _$_eb2b[257]);
                                    var A = E.getColumnNameFromCoords(h + D, l + z);
                                    m.push(A)
                                }
                            if (!n) {
                                A = E.getColumnNameFromCoords(h, l);
                                var V = Object.keys(this.formula);
                                for (z = 0; z < V.length; z++) {
                                    var sa = this.formula[V[z]].indexOf(A);
                                    if (0 <= sa)
                                        for (D = 0; D < m.length; D++) sa = this.formula[V[z]].indexOf(m[D]), -1 == sa && this.formula[V[z]].push(m[D])
                                }
                                for (z = 0; z < y.length; z++)
                                    for (D =
                                        0; D < y[z].length; D++) this.records[l + z][h + D].v = y[z][D], this.records[l + z][h + D].element && (this.records[l + z][h + D].element.innerText = y[z][D]);
                                this.records[l][h].a = y;
                                n = y[0][0];
                                this.records[l][h].v = n
                            }
                            y = n
                        } else this.records[l][h].v = y;
                    return y
                } catch (Y) {
                    return 1 == this.parent.config.debugFormulas && console.log(g, u, Y), Y && Y.error ? Y.error : _$_eb2b[254]
                }
            };
            a.update = function (g, h, l, m) {
                var p = null,
                    n = null,
                    t = function (y) {
                        l && l[y] && (y = _$_eb2b[258]);
                        h[y] && (y = h[y]);
                        return y
                    };
                g = c(g, !0);
                g = b(g);
                for (var v = 0; v < g.length; v++)
                    if (g[v] &&
                        k.test(g[v])) {
                        if (-1 == g[v].indexOf(_$_eb2b[136])) {
                            if (n = g[v], p = _$_eb2b[3], m) continue
                        } else if (n = g[v].split(_$_eb2b[136]), p = n[0], n = n[1], m != p) continue;
                        if (0 <= n.indexOf(_$_eb2b[137])) {
                            n = a.getTokensFromRange.call(this, n);
                            for (var u = 0; u < n.length; u++) n[u] = t(n[u]);
                            n = a.getRangeFromTokens.call(this, n)
                        } else - 1 == n.indexOf(_$_eb2b[259]) ? n = t(n) : (n = t(n.replace(/\$/g, _$_eb2b[3])), n = _$_eb2b[259] + n.match(/[A-Z]+/g) + _$_eb2b[259] + n.match(/[0-9]+/g));
                        p && (n = p + _$_eb2b[136] + n);
                        g[v] = n
                    } return g.join(_$_eb2b[3])
            };
            a.getChain = function (g) {
                if (void 0 ===
                    g) return [];
                var h = [],
                    l = [],
                    m = [],
                    p = function (v) {
                        if (0 <= v.indexOf(_$_eb2b[27])) {
                            var u = v.split(_$_eb2b[27]),
                                y = u[0],
                                z = u[1];
                            u = I(u[0])
                        } else u = this, y = u.getWorksheetName(), z = v;
                        if (u.formula[z] && !m[v]) {
                            var D = !1;
                            m[v] = !0;
                            for (v = 0; v < u.formula[z].length; v++) {
                                var A = u.formula[z][v]; - 1 == A.indexOf(_$_eb2b[27]) && (A = y + _$_eb2b[27] + A);
                                if (!l[A]) {
                                    var V = u.getValue(A);
                                    P(V) ? h.push([A, V, a.getTokens.call(u, V, y)]) : (D = !0, u.formula[z][v] = null);
                                    p.call(u, A);
                                    l[A] = !0
                                }
                            }
                            1 == D && (u.formula[z] = u.formula[z].filter(function (sa) {
                                return null != sa
                            }))
                        }
                    },
                    n = Object.keys(g);
                for (g = 0; g < n.length; g++) p.call(this, n[g]);
                n = 0;
                for (g = h.length - 1; 0 <= g; g--) {
                    for (var t = 0; t < g; t++)
                        if (0 <= h[t][2].indexOf(h[g][0])) {
                            h.splice(t, 0, h.splice(g, 1)[0]);
                            g = h.length;
                            break
                        } n++;
                    if (1E6 < n) {
                        console.error(_$_eb2b[260]);
                        break
                    }
                }
                l = [];
                for (g = 0; g < h.length; g++) l[h[g][0]] = h[g][1];
                return l
            };
            a.updateAll = function (g, h) {
                var l, m;
                Ia.updateAll.call(this, g, h);
                var p = Object.keys(this.formula);
                var n = [];
                var t = [];
                for (l = 0; l < p.length; l++) {
                    var v = p[l];
                    var u = this.formula[v];
                    if (h && h[v])
                        for (var y = 0; y < u.length; y++) g[u[y]] &&
                            (u[y] = g[u[y]]), t[u[y]] = !0;
                    else {
                        var z = [];
                        for (y = 0; y < u.length; y++) h && h[u[y]] || (g[u[y]] && (u[y] = g[u[y]]), t[u[y]] = !0, z.push(u[y]));
                        z.length && (g[v] && (v = g[v]), n[v] = z)
                    }
                }
                this.formula = n;
                z = this.getWorksheetName();
                for (var D = 0; D < r.spreadsheet.length; D++)
                    for (var A = r.spreadsheet[D].worksheets, V = 0; V < A.length; V++)
                        if (A[V].getWorksheetName() !== z)
                            for (p = Object.keys(A[V].formula), l = 0; l < p.length; l++) {
                                n = [];
                                u = A[V].formula[p[l]];
                                for (y = 0; y < u.length; y++) v = u[y].split(_$_eb2b[27]), v[0] == z && v[1] ? h && h[v[1]] || (g[v[1]] && (v[1] = g[v[1]]),
                                    n.push(v[0] + _$_eb2b[27] + v[1])) : n.push(u[y]);
                                A[V].formula[p[l]] = n
                            }
                p = [];
                v = Object.keys(t);
                for (y = 0; y < v.length; y++) 0 <= v[y].indexOf(_$_eb2b[27]) ? (u = v[y].split(_$_eb2b[27]), n = I(u[0]), l = u[1], z = this.getWorksheetName(), u = u[0]) : (n = this, l = v[y], z = null, u = this.getWorksheetName()), (m = n.getValue(l)) && P(m) && (n = a.update.call(n, m, g, h, z), n != m && (t[v[y]] = n, l = E.getCoordsFromColumnName(l), p[u] || (p[u] = []), p[u].push({
                    x: l[0],
                    y: l[1],
                    value: n,
                    force: !0
                })));
                v = Object.keys(p);
                for (g = 0; g < v.length; g++) n = I(v[g]), p[v[g]] = ea.applyValues.call(n,
                    p[v[g]]), ea.setValueChained.call(n, p[v[g]]), J.call(n, _$_eb2b[108], {
                        data: p[v[g]]
                    })
            };
            a.updateWorksheetName = function (g, h, l) {
                for (var m = b(g), p = 0; p < m.length; p++) g = m[p].split(_$_eb2b[136]), g[1] && (g[0] = g[0].replace(new RegExp(_$_eb2b[134], _$_eb2b[191]), _$_eb2b[3]), g[0].toUpperCase() == h.toUpperCase() && (g[0] = l, 0 <= g[0].indexOf(_$_eb2b[133]) && (g[0] = _$_eb2b[134] + g[0] + _$_eb2b[134]), m[p] = g.join(_$_eb2b[136])));
                return m.join(_$_eb2b[3])
            };
            a.updateWorksheetNames = function (g, h) {
                for (var l, m, p = 0; p < r.spreadsheet.length; p++)
                    for (var n = [], t = r.spreadsheet[p].worksheets, v = 0; v < t.length; v++) {
                        for (var u = 0; u < t[v].options.data.length; u++)
                            for (var y = 0; y < t[v].options.data[u].length; y++)(l = t[v].options.data[u][y]) && P(l) && (m = a.updateWorksheetName(l, g, h), m != l && n.push({
                                x: y,
                                y: u,
                                value: m,
                                force: !0
                            }));
                        n.length && (n = ea.applyValues.call(t[v], n), J.call(t[v], _$_eb2b[108], {
                            data: n
                        }))
                    }
            };
            a.shiftFormula = function (g, h, l) {
                var m = b(g, !0);
                g = function (v) {
                    if (-1 == v.indexOf(_$_eb2b[136])) {
                        v = E.getCoordsFromColumnName(v);
                        var u = _$_eb2b[3]
                    } else u = m[p].split(_$_eb2b[136]),
                        v = E.getCoordsFromColumnName(u[1]), u = u[0] + _$_eb2b[136];
                    var y = v[0] + h;
                    v = v[1] + l;
                    return v = 0 > y || 0 > v ? _$_eb2b[258] : u + E.getColumnNameFromCoords(y, v)
                };
                for (var p = 0; p < m.length; p++)
                    if (k.test(m[p]) && -1 == m[p].indexOf(_$_eb2b[259])) {
                        var n = -1 == m[p].indexOf(_$_eb2b[136]) ? [_$_eb2b[3], m[p]] : m[p].split(_$_eb2b[136]);
                        if (-1 == n[1].indexOf(_$_eb2b[137])) n[1] = g(n[1]);
                        else {
                            var t = n[1].split(_$_eb2b[137]);
                            n[1] = g(t[0]) + _$_eb2b[137] + g(t[1])
                        }
                        m[p] = n[0] ? n.join(_$_eb2b[136]) : n[1]
                    } return m.join(_$_eb2b[3])
            };
            a.getTokensFromRange = function (g) {
                var h =
                    this,
                    l = _$_eb2b[3],
                    m = _$_eb2b[3];
                0 <= g.indexOf(_$_eb2b[27]) ? m = _$_eb2b[27] : 0 <= g.indexOf(_$_eb2b[136]) && (m = _$_eb2b[136]);
                g = g.replace(/\$/g, _$_eb2b[3]);
                m && (l = g.split(m), h = I(l[0]), g = l[1], l = l[0] + m);
                if (-1 == g.indexOf(_$_eb2b[137])) return [g];
                m = [];
                g = g.split(_$_eb2b[137]);
                var p = E.getCoordsFromColumnName(g[0]),
                    n = E.getCoordsFromColumnName(g[1]);
                if (p[0] <= n[0]) {
                    g = p[0];
                    var t = n[0]
                } else g = n[0], t = p[0];
                if (null === p[1] && null == n[1]) {
                    var v = 0;
                    p = 0;
                    h.options && h.options.data && (p = h.options.data.length - 1)
                } else p[1] <= n[1] ? (v = p[1],
                    p = n[1]) : (v = n[1], p = p[1]);
                for (h = v; h <= p; h++)
                    for (v = g; v <= t; v++) m.push(l + E.getColumnNameFromCoords(v, h));
                return m
            };
            a.getRangeFromTokens = function (g) {
                g = g.filter(function (m) {
                    return m != _$_eb2b[258]
                });
                for (var h = _$_eb2b[3], l = 0; l < g.length; l++) 0 <= g[l].indexOf(_$_eb2b[27]) && (h = g[l].split(_$_eb2b[27]), g[l] = h[1], h = h[0]);
                h && (h += _$_eb2b[27]);
                g.sort(function (m, p) {
                    m = E.getCoordsFromColumnName(m);
                    p = E.getCoordsFromColumnName(p);
                    return m[1] > p[1] ? 1 : m[1] < p[1] ? -1 : m[0] > p[0] ? 1 : m[0] < p[0] ? -1 : 0
                });
                return g.length ? h + (g[0] + _$_eb2b[137] +
                    g[g.length - 1]) : _$_eb2b[258]
            };
            a.getTokens = function (g, h) {
                var l = d(g);
                g.replace(/!/g, _$_eb2b[27]);
                (g = l.match(/[A-Z_]+[A-Z0-9_\.]*/g)) && g.length && (l = f.call(this.parent, g, l));
                (g = l.match(/('.*?'?|\w*\.)?(\$?[A-Z]+\$?[0-9]*):(\$?[A-Z]+\$?[0-9]*)?/g)) && g.length && (l = e.call(this, g, l));
                g = l.match(/('.*?'?|\w*\.)?(\$?[A-Z]+\$?[0-9]+)(:\$?[A-Z]+\$?[0-9]+)?/g);
                for (l = 0; l < g.length; l++) - 1 == g[l].indexOf(_$_eb2b[27]) && h && (g[l] = h + _$_eb2b[27] + g[l]);
                return g
            };
            a.tokenIdentifier = k;
            a.tokenize = b;
            return a
        }(),
        ea = function () {
            var k =
                function (b) {
                    b.name = Z;
                    b.value = M;
                    b.getValue = k[_$_eb2b[51]];
                    b.getValueFromCoords = k.getFromCoords;
                    b.setValue = k[_$_eb2b[11]];
                    b.setValueFromCoords = k.setFromCoords;
                    b.setCheckRadioValue = k.setCheckRadio;
                    b.getProcessed = k.processed
                };
            k[_$_eb2b[51]] = function (b, c) {
                var a = this;
                if (typeof b == _$_eb2b[84]) {
                    var d = b.x;
                    b = b.y
                } else 0 <= b.indexOf(_$_eb2b[27]) && (a = b.split(_$_eb2b[27]), b = a[1], a = I(a[0])), b = E.getCoordsFromColumnName(b), d = b[0], b = b[1];
                return k.getFromCoords.call(a, d, b, c)
            };
            k.getFromCoords = function (b, c, a) {
                var d = null;
                null != b && null != c && (d = a ? k.processed.call(this, b, c) : M.call(this, b, c));
                return d
            };
            k[_$_eb2b[11]] = function (b, c, a) {
                if (!Q.call(this.parent, this)) return !1;
                var d = null,
                    e = null,
                    f = null,
                    g = [];
                a = a ? !0 : !1;
                var h = function (p, n) {
                    var t = null;
                    typeof p == _$_eb2b[17] ? (f = E.getCoordsFromColumnName(p), d = f[0], e = f[1]) : p.tagName ? (d = p.getAttribute(_$_eb2b[42]), e = p.getAttribute(_$_eb2b[43])) : p.element && p.element.tagName ? (d = p.element.getAttribute(_$_eb2b[42]), e = p.element.getAttribute(_$_eb2b[43])) : (d = p.x, e = p.y, void 0 !== p.value && (n = p.value),
                        void 0 !== p.style && (t = p.style));
                    p = {
                        x: d,
                        y: e,
                        value: n,
                        force: a
                    };
                    null !== t && (p.style = t);
                    g.push(p)
                };
                if (b && Array.isArray(b))
                    for (var l = 0; l < b.length; l++) h(b[l], c);
                else if (typeof b == _$_eb2b[84] && void 0 == b.x && void 0 == b.tagName)
                    for (c = Object.keys(b), l = 0; l < c.length; l++) h(c[l], b[c[l]]);
                else h(b, c);
                if (g.length) {
                    g = k.applyValues.call(this, g);
                    this.parent.config.parseFormulas && k.setValueChained.call(this, g);
                    X.update.call(this, !0);
                    this.refreshBorders();
                    var m = this;
                    setTimeout(function () {
                        ka.refresh.call(m)
                    }, 250);
                    if (g.length) {
                        T.call(this.parent, {
                            worksheet: m,
                            action: _$_eb2b[77],
                            records: g,
                            selection: this.selectedCell
                        });
                        b = this.getPrimaryKey();
                        if (!1 !== b)
                            for (l = 0; l < g.length; l++) g[l].x === b && g[l].value && S.call(this, g[l]);
                        J.call(this, _$_eb2b[77], {
                            data: g
                        });
                        F.call(this.parent, _$_eb2b[261], this, g)
                    }
                }
            };
            k.setFromCoords = function (b, c, a, d) {
                var e = [];
                e.push({
                    x: b,
                    y: c,
                    value: a
                });
                k[_$_eb2b[11]].call(this, e, null, d)
            };
            k.setCheckRadio = function () {
                for (var b = [], c = this.getSelected(), a = 0; a < c.length; a++) {
                    var d = c[a].x,
                        e = c[a].y;
                    if (!this.options.columns[d].readOnly && this.options.columns[d].type ==
                        _$_eb2b[262]) {
                        var f = M.call(this, d, e) ? !1 : !0;
                        b.push({
                            x: d,
                            y: e,
                            value: f
                        })
                    }
                }
                b.length && k[_$_eb2b[11]].call(this, b)
            };
            k.applyValues = function (b) {
                for (var c, a, d = this.options.style || {}, e = [], f = {}, g = 0; g < b.length; g++)
                    if (c = b[g], a = this.updateCell(c.x, c.y, c.value, c.force), void 0 !== a.value) {
                        if (void 0 !== c.style) {
                            var h = E.getColumnNameFromCoords(c.x, c.y);
                            a.style = f[h] = c.style;
                            a.oldStyle = d && d[h] ? d[h] : _$_eb2b[196]
                        }
                        e.push(a)
                    } 0 < Object.keys(f).length && Qa.update.call(this, f);
                return e
            };
            k.processed = function (b, c, a) {
                var d = M.call(this,
                    b, c);
                if (null === d) return null;
                var e = ba[_$_eb2b[51]].call(this, b, c),
                    f = e[1];
                f.type != _$_eb2b[49] && f.type != _$_eb2b[263] && f.type != _$_eb2b[264] && f.type != _$_eb2b[265] && f.type != _$_eb2b[266] && 0 != f.process && (!0 === a || f.type != _$_eb2b[267] && f.type != _$_eb2b[268] && f.type != _$_eb2b[269]) && (f.type == _$_eb2b[262] ? d = e[0][_$_eb2b[51]](f, d, a) : this.records[c][b].element ? d = 1 == this.parent.config.stripHTML ? this.records[c][b].element.innerText : this.records[c][b].element.innerHTML : ((_$_eb2b[3] + d).substr(0, 1) == _$_eb2b[12] && (d =
                    this.executeFormula(d, b, c, !1)), e[0] && typeof e[0][_$_eb2b[51]] == _$_eb2b[85] && (d = e[0][_$_eb2b[51]](f, d, a))));
                return d
            };
            k.setValueChained = function (b) {
                for (var c, a = [], d, e = 0; e < b.length; e++) c = b[e], d = E.getColumnNameFromCoords(c.x, c.y), a[d] = c.value;
                a = W.getChain.call(this, a);
                var f = [],
                    g = Object.keys(a);
                for (e = 0; e < g.length; e++) 0 <= g[e].indexOf(_$_eb2b[27]) ? (c = g[e].split(_$_eb2b[27]), d = c[1], c = I(c[0])) : (d = g[e], c = this), d = E.getCoordsFromColumnName(d), b = N.update.call(c, d[0], d[1], a[g[e]], !0), this.parent.config.onformulachain &&
                    (b.w = c, b.v = c.records[d[1]][d[0]].v, b.cell = g[e], b.f = a[g[e]], f.push(b));
                F.call(this.parent, _$_eb2b[270], this, f)
            };
            return k
        }(),
        N = function () {
            var k = function (b) {
                b.getCell = k[_$_eb2b[51]];
                b.getCellFromCoords = k[_$_eb2b[51]];
                b.updateCell = k.update;
                b.updateCells = k.updateAll;
                b.getSelected = k.selected;
                b.isAttached = k.attached
            };
            k.attached = function (b, c) {
                return this.records[c] && this.records[c][b] && this.records[c][b].element && this.records[c][b].element.parentNode && this.records[c][b].element.parentNode.parentNode ? !0 : !1
            };
            k[_$_eb2b[51]] = function (b, c, a) {
                typeof b == _$_eb2b[17] && (c = E.getCoordsFromColumnName(b), b = c[0], c = c[1]);
                b = parseInt(b);
                c = parseInt(c);
                if (this.records[c] && this.records[c][b] && this.records[c][b].element) return this.records[c][b].element;
                var d = M.call(this, b, c);
                null === d && (d = _$_eb2b[3]);
                return N.create.call(this, b, c, d, a)
            };
            k.getFromCoords = function (b, c, a) {
                if (!b) return null;
                var d = [],
                    e = b[0],
                    f = b[1],
                    g = b[2];
                b = b[3];
                for (a && (a = this.results ? E.invert(this.results) : null); f <= b; f++)
                    for (var h = e; h <= g; h++)
                        if (!a || a[f]) c ? d.push(E.getColumnNameFromCoords(h,
                            f)) : d.push(this.records[f][h]);
                return d
            };
            k.selected = function (b, c) {
                return k.getFromCoords.call(this, this.getHighlighted(), b, c)
            };
            k.getSelectedColumns = function () {
                var b = [],
                    c = this.getHighlighted();
                if (c)
                    for (var a = c[0]; a <= c[2]; a++) b.push(a);
                return b
            };
            k.updateAll = function (b) {
                for (var c = this.getPrimaryKey(), a = [], d = 0; d < b.length; d++) void 0 !== b[d].id ? (S.call(this, b[d]), a.push({
                    id: b[d].id,
                    y: b[d].y
                }), !1 !== c && k.update.call(this, c, b[d].y, b[d].id, !0)) : a.push(k.update.call(this, b[d].x, b[d].y, b[d].value, b[d].force));
                J.call(this, _$_eb2b[107], [a])
            };
            k.update = function (b, c, a, d) {
                var e = this.records[c];
                if (e = e && e[b] ? e[b] : null) {
                    e = e.element;
                    var f = {
                        x: b,
                        y: c
                    };
                    if (d || !e || 1 != e.classList.contains(_$_eb2b[18])) {
                        if (d = F.call(this.parent, _$_eb2b[271], this, e, b, c, a)) a = d;
                        else if (!1 === d) return f;
                        d = ba[_$_eb2b[51]].call(this, b, c);
                        if (typeof d[0].updateCell == _$_eb2b[85]) {
                            var g = d[0].updateCell(e, a, b, c, this, d[1]);
                            void 0 !== g && (a = g)
                        }
                        g = M.call(this, b, c);
                        if (g !== a) {
                            var h = M.call(this, b, c, a);
                            f.oldValue = g;
                            f.value = h;
                            e && k.applyOverflow.call(this, e, b,
                                c);
                            d[1] && typeof d[1].render == _$_eb2b[85] && d[1].render(e, a, b, c, this, d[1]);
                            F.call(this.parent, _$_eb2b[272], this, e, b, c, f.value, f.oldValue)
                        }
                    }
                } else return !1;
                return f
            };
            k.create = function (b, c, a, d) {
                var e = document.createElement(_$_eb2b[148]);
                e.setAttribute(_$_eb2b[42], b);
                e.setAttribute(_$_eb2b[43], c);
                var f = ba[_$_eb2b[51]].call(this, b, c);
                f[0] && typeof f[0].createCell == _$_eb2b[85] && (f = f[0].createCell(e, a, b, c, this, f[1]), void 0 !== f && (a = f));
                M.call(this, b, c, a);
                this.records[c][b] || na.cell.call(this, b, c, a);
                this.records[c][b].element =
                    e;
                !a && this.records[c][b].v && (e.innerText = this.records[c][b].v);
                k.applyProperties.call(this, e, b, c, d);
                k.applyOverflow.call(this, e, b, c);
                F.call(this.parent, _$_eb2b[273], this, e, b, c, a);
                return e
            };
            k.getAttributes = function (b, c, a) {
                a[b] || (a[b] = {});
                b == _$_eb2b[274] ? a[b][c] = this.getValue(c) : b == _$_eb2b[275] ? this.merged.cells && (c = this.merged.cells[c]) && (a[b][c] = this.options.mergeCells[c]) : this.options[b] && this.options[b][c] && (a[b][c] = this.options[b][c])
            };
            k.setAttributes = function (b) {
                b && b.style && this.setStyle(b.style);
                b && b.comments && this.setComments(b.comments);
                b && b.meta && this.setMeta(b.meta);
                b && b.cells && this.setCells(b.cells);
                b && b.mergeCells && this.setMerge(b.mergeCells);
                b && b.values && k.batch.call(this, b.values);
                b && b.formulas && k.batch.call(this, b.formulas, !0)
            };
            k.applyProperties = function (b, c, a, d) {
                if (!d) {
                    var e = E.getColumnNameFromCoords(c, a),
                        f = this.options.style;
                    d = this.options.comments;
                    var g = this.options.cells,
                        h = this.options.rows;
                    if (f && f[e]) {
                        var l = (l = b.getAttribute(_$_eb2b[100])) ? l + (_$_eb2b[220] + f[e]) : f[e];
                        b.setAttribute(_$_eb2b[100],
                            l)
                    }
                    this.options.columns[c] && this.options.columns[c].wrap && b.classList.add(_$_eb2b[276]);
                    d && d[e] && b.setAttribute(_$_eb2b[277], d[e]);
                    g && g[e] && (1 == g[e].readOnly && b.classList.add(_$_eb2b[18]), !1 === g[e].locked && !0 === this.options.selectUnLockedCells && b.classList.add(_$_eb2b[278]), 1 == g[e].wrap && b.classList.add(_$_eb2b[276]));
                    h && h[a] && 1 == h[a].readOnly && b.classList.add(_$_eb2b[18])
                }
                d = this.options.columns[c];
                if (!b.style.textAlign || d.align)
                    if (l = d.align ? d.align : this.options.defaultColAlign) b.style.textAlign =
                        l;
                !1 === this.records[a][c].readonly || 1 != this.records[a][c].readonly && 1 != d.readOnly && 1 != d.readonly || b.classList.add(_$_eb2b[18])
            };
            k.applyOverflow = function (b, c, a) {
                1 == this.options.textOverflow && 0 < c && (c == this.options.columns.length - 1 && (b.style.overflow = _$_eb2b[49]), this.records[a][c - 1] && this.records[a][c - 1].element && (this.records[a][c - 1].element.style.overflow = !b.innerText && this.headers[c - 1] && this.headers[c - 1].offsetWidth ? _$_eb2b[3] : _$_eb2b[49]))
            };
            k.batch = function (b, c) {
                var a = [],
                    d = Object.keys(b);
                if (d.length)
                    for (var e =
                        0; e < d.length; e++) {
                        if (0 <= d[e].indexOf(_$_eb2b[27])) {
                            var f = d[e].split(_$_eb2b[27]);
                            var g = I(f[0]);
                            var h = f[1];
                            f = f[0]
                        } else g = this, h = d[e], f = this.getWorksheetName();
                        h = E.getCoordsFromColumnName(h);
                        h = g.updateCell(h[0], h[1], b[d[e]], !0);
                        a[f] || (a[f] = []);
                        a[f].push(h)
                    }
                if (1 == c && (d = Object.keys(a), d.length))
                    for (e = 0; e < d.length; e++) g = I(d[e]), J.call(g, _$_eb2b[108], a[d[e]])
            };
            return k
        }(),
        Ha = function () {
            var k = function (b) {
                b.getColumnOptions = k[_$_eb2b[51]];
                b.setColumnOptions = k[_$_eb2b[11]];
                b.getCells = k.getCells;
                b.setCells =
                    k.setCells;
                b.getProperties = k[_$_eb2b[51]];
                b.setProperties = k[_$_eb2b[11]];
                b.getProperty = k[_$_eb2b[51]];
                b.setProperty = k[_$_eb2b[11]];
                b.resetProperty = k[_$_eb2b[11]];
                b.updateProperty = k.update;
                b.getOptions = k[_$_eb2b[51]]
            };
            k.getCells = function (b) {
                return b ? this.options.cells[b] : this.options.cells
            };
            k.setCells = function (b, c) {
                if (!Q.call(this.parent, this)) return !1;
                if (typeof b == _$_eb2b[17]) this.options.cells[b] = c;
                else {
                    c = Object.keys(b);
                    for (var a = 0; a < c.length; a++) this.options.cells[c[a]] = b[c[a]]
                }
            };
            k[_$_eb2b[51]] =
                function (b, c) {
                    var a = null;
                    void 0 !== c && (c = E.getColumnNameFromCoords(b, c), this.options.cells && this.options.cells[c] && (a = this.options.cells[c]));
                    null === a && (a = this.options.columns[b]);
                    if (!a) a = {
                        type: _$_eb2b[55]
                    };
                    else if (!a.type || typeof a.type == _$_eb2b[17] && !r.editors[a.type]) a.type = _$_eb2b[55];
                    return a
                };
            k[_$_eb2b[11]] = function (b, c, a) {
                if (!Q.call(this.parent, this)) return !1;
                this.edition && this.closeEditor(this.edition, !1);
                var d = [];
                Array.isArray(b) ? d = b : jSuites.isNumeric(b) && jSuites.isNumeric(c) ? (b = {
                    x: b,
                    y: c
                },
                    typeof a == _$_eb2b[84] && (b.value = a), d.push(b)) : typeof c === _$_eb2b[84] && (b = {
                        x: b,
                        value: c
                    }, d.push(b));
                if (Array.isArray(d))
                    for (a = 0; a < d.length; a++) {
                        var e = d[a];
                        b = e.x;
                        c = e.y;
                        var f = k[_$_eb2b[51]].call(this, b, c),
                            g = e.value,
                            h = r.editors[f.type],
                            l = null;
                        g && g.type && (l = r.editors[g.type]);
                        if (typeof c === _$_eb2b[13])
                            for (e.oldValue = f, parseInt(g.width) || (g.width = this.options.defaultColWidth || 100), g.type && r.editors[g.type] || (g.type = _$_eb2b[55]), this.options.columns[b] = g, g.width && this.colgroup[b] && this.colgroup[b].setAttribute(_$_eb2b[153],
                                parseInt(g.width)), typeof g.title !== _$_eb2b[13] && this.headers[b] && (g.title ? (this.headers[b].innerText = g.title, this.headers[b].setAttribute(_$_eb2b[167], g.title)) : (this.headers[b].innerText = E.getColumnName(b), this.headers[b].removeAttribute(_$_eb2b[167]))), g.tooltip && this.headers[b] && (this.headers[b].title = g.tooltip), c = 0; c < this.records.length; c++) this.records[c][b].element && (h && typeof h.destroyCell == _$_eb2b[85] && h.destroyCell(this.records[c][b].element, b, c), l && typeof l.createCell == _$_eb2b[85] && (N.applyProperties.call(this,
                                    this.records[c][b].element, b, c), l.createCell(this.records[c][b].element, M.call(this, b, c), b, c, this, g)));
                        else if (f = E.getColumnNameFromCoords(b, c), e.oldValue = this.options.cells && this.options.cells[f] ? this.options.cells[f] : _$_eb2b[3], g ? (this.options.cells || (this.options.cells = {}), this.options.cells[f] = g) : (delete this.options.cells[f], g = k[_$_eb2b[51]].call(this, b, c), l = r.editors[g.type]), e = this.records[c][b].element) h && typeof h.destroyCell == _$_eb2b[85] && h.destroyCell(e, b, c, this), l && typeof l.createCell ==
                            _$_eb2b[85] && (h = M.call(this, b, c), N.applyProperties.call(this, e, b, c, this), g = l.createCell(e, h, b, c, this, g), void 0 !== g && M.call(this, b, c, g))
                    } else return console.error(_$_eb2b[260]), !1;
                this.refreshBorders();
                T.call(this.parent, {
                    worksheet: this,
                    action: _$_eb2b[78],
                    records: d
                });
                J.call(this, _$_eb2b[78], {
                    data: d
                });
                F.call(this.parent, _$_eb2b[279], this, d);
                return !0
            };
            k.update = function (b) {
                var c = JSON.parse(JSON.stringify(b));
                if (Array.isArray(b))
                    for (var a = 0; a < b.length; a++) {
                        var d = b[a].x,
                            e = b[a].y,
                            f = b[a];
                        var g = E.getColumnNameFromCoords(d,
                            e);
                        this.options.cells[g] || (this.options.cells[g] = {});
                        g = this.options.cells[g];
                        f.oldValue = Object.assign({}, g);
                        for (var h = Object.keys(f.value), l = 0; l < h.length; l++) g[h[l]] = f.value[h[l]];
                        f = ba[_$_eb2b[51]].call(this, d, e);
                        typeof f[0].updateProperty == _$_eb2b[85] && f[0].updateProperty(d, e, this, f[1])
                    }
                T.call(this.parent, {
                    worksheet: this,
                    action: _$_eb2b[79],
                    records: b
                });
                J.call(this, _$_eb2b[79], {
                    data: c
                });
                F.call(this.parent, _$_eb2b[279], this, b)
            };
            return k
        }(),
        Aa = function () {
            var k = function (c, a) {
                Array.isArray(a) || (a = [a]);
                for (var d, e = c ? _$_eb2b[3] : _$_eb2b[40], f = Object.keys(a), g = 0; g < f.length; g++)
                    if (d = this.rows[a[f[g]]]) d.element && (d.element.style.display = e), d.visible = c;
                this.resetBorders();
                G.refresh.call(this)
            },
                b = function (c) {
                    c.getRow = b[_$_eb2b[51]];
                    c.moveRow = b.move;
                    c.insertRow = b.add;
                    c.deleteRow = b.del;
                    c.getSelectedRows = b.selected;
                    c.showRow = k.bind(c, !0);
                    c.hideRow = k.bind(c, !1)
                };
            b[_$_eb2b[51]] = function (c) {
                return this.rows[c]
            };
            b.attached = function (c) {
                return this.rows[c] && this.rows[c].element && this.rows[c].element.parentNode ?
                    !0 : !1
            };
            b.selected = function (c) {
                var a = [],
                    d = this.getHighlighted();
                if (d)
                    for (var e = d[1]; e <= d[3]; e++) this.rows[e] && (c ? a.push(e) : a.push(this.rows[e].element));
                return a
            };
            b.create = function (c, a) {
                if (this.rows[c] && this.rows[c].element) return this.rows[c].element;
                this.rows[c] || na.row.call(this, c);
                var d = document.createElement(_$_eb2b[146]);
                d.setAttribute(_$_eb2b[43], c);
                this.rows[c].element = d;
                var e = null;
                this.options.rows[c] && !a && (this.options.rows[c].style && d.setAttribute(_$_eb2b[100], this.options.rows[c].style),
                    this.options.rows[c].height && (d.style.height = parseInt(this.options.rows[c].height) + _$_eb2b[44]), this.options.rows[c].title && (e = this.options.rows[c].title), 0 == this.options.rows[c].visible && (this.rows[c].visible = !1), this.options.rows[c].id && (this.rows[c].id = this.options.rows[c].id));
                F.call(this.parent, _$_eb2b[280], this, c, d);
                0 == this.rows[c].visible && (d.style.display = _$_eb2b[40]);
                e || (e = parseInt(c + 1));
                a = document.createElement(_$_eb2b[148]);
                a.innerHTML = e;
                a.setAttribute(_$_eb2b[43], c);
                a.className = _$_eb2b[281];
                d.appendChild(a);
                return d
            };
            b.move = function (c, a) {
                if (!Q.call(this.parent, this)) return !1;
                c = parseInt(c);
                a = parseInt(a);
                if (!this.rows[a] || !this.rows[c]) return console.error(_$_eb2b[282]), !1;
                if (0 < Object.keys(this.getMerge()).length) {
                    var d = null;
                    this.merged.rows[c] ? d = !0 : this.merged.rows[a] && (c > a ? this.merged.rows[a - 1] && (d = !0) : this.merged.rows[a + 1] && (d = !0));
                    d && this.destroyMerged()
                }
                this.resetBorders();
                0 <= Array.prototype.indexOf.call(this.tbody.children, this.rows[a].element) ? (d = G.renderRow.call(this, c), c > a ? this.tbody.insertBefore(d,
                    this.rows[a].element) : this.tbody.insertBefore(d, this.rows[a].element.nextSibling)) : this.rows[c].element && (this.tbody.removeChild(this.rows[c].element), G.refresh.call(this));
                this.rows.splice(a, 0, this.rows.splice(c, 1)[0]);
                this.records.splice(a, 0, this.records.splice(c, 1)[0]);
                this.options.data.splice(a, 0, this.options.data.splice(c, 1)[0]);
                0 < this.options.pagination && this.tbody.children.length != this.options.pagination && this.page(this.pageNumber);
                J.call(this, _$_eb2b[66], {
                    f: c,
                    t: a
                });
                X.references.call(this);
                X.update.call(this);
                T.call(this.parent, {
                    worksheet: this,
                    action: _$_eb2b[66],
                    oldValue: c,
                    newValue: a
                });
                F.call(this.parent, _$_eb2b[283], this, c, a)
            };
            b.add = function (c, a, d, e) {
                if (!Q.call(this.parent, this)) return !1;
                if (!this.options.allowInsertRow) return console.error(_$_eb2b[284]), !1;
                var f = [];
                if (0 < c) var g = parseInt(c);
                else g = 1, Array.isArray(c) && !e && (Array.isArray(c[0]) ? e = c : (e = [], e.push(c)));
                e = Fa.standardize(e);
                d = d ? !0 : !1;
                var h = this.options.data ? this.options.data.length - 1 : 0;
                if (void 0 == a || a > h || 0 > a) a = h, d = !1;
                g || (g =
                    1);
                if (!1 === F.call(this.parent, _$_eb2b[285], this, a, g, d)) return console.log(_$_eb2b[286]), !1;
                this.resetBorders();
                var l = d ? a : a + 1,
                    m = 0;
                c = [];
                for (var p = [], n = this.rows.splice(l), t = this.options.data ? this.options.data.splice(l) : [], v = this.records.splice(l), u = l; u < g + l; u++) {
                    var y = e[m] && e[m].id ? e[m].id : ta.next.call(this);
                    na.row.call(this, u, y);
                    var z = this.dataType ? {} : [];
                    for (var D = 0; D < this.options.columns.length; D++) {
                        var A = Z.call(this, D);
                        z[A] = _$_eb2b[3];
                        e && e[m] && e[m].data && (typeof e[m].data[A] !== _$_eb2b[13] ? z[A] = e[m].data[A] :
                            typeof e[m].data[D] !== _$_eb2b[13] && (z[A] = e[m].data[D]));
                        f.push({
                            x: D,
                            y: u,
                            value: z[A],
                            force: !0
                        })
                    }
                    p.push({
                        id: y,
                        data: z
                    });
                    m++
                }
                this.rows = this.rows.concat(n);
                this.records = this.records.concat(v);
                Array.prototype.push.apply(this.options.data, t);
                this.results && 0 < this.results.length && (h = this.results[this.results.length - 1]);
                e = !1;
                n[0] ? 0 <= Array.prototype.indexOf.call(this.tbody.children, n[0].element) && (e = !0) : this.rows[h] ? this.rows[h].element && 0 <= Array.prototype.indexOf.call(this.tbody.children, this.rows[h].element) &&
                    (e = !0) : e = !0;
                for (u = l; u < g + l; u++) this.results && (this.rows[u].results = !0), 1 == e && (h = n[0] ? n[0].element : null, A = G.renderRow.call(this, u, null, null, !0), h ? this.tbody.insertBefore(A, h) : this.tbody.appendChild(A), 0 < this.options.pagination ? Array.prototype.indexOf.call(this.tbody.children, A) >= this.options.pagination && (e = !1) : G.limited.call(this) && (G.isVisible.call(this, A.children[1]) || (e = !1))), c.push(this.records[u]);
                this.merged.rows[l] && ca.updateConfig.call(this, 1, 1, l - 1, g);
                J.call(this, _$_eb2b[62], {
                    numOfRows: g,
                    rowNumber: a,
                    insertBefore: d ? 1 : 0,
                    data: p
                });
                l = this.tbody.firstChild.getAttribute(_$_eb2b[43]);
                this.results && (l = this.results.indexOf(parseInt(l)), this.results = va.refresh.call(this));
                X.references.call(this);
                f.length && ea.applyValues.call(this, f);
                0 < this.options.pagination ? ua[_$_eb2b[11]].call(this, this.pageNumber) : (this.results && (l = this.results[l]), this.tbody.textContent = _$_eb2b[3], G[_$_eb2b[46]].call(this, l));
                X.update.call(this);
                U.call(this);
                this.refreshBorders();
                T.call(this.parent, {
                    worksheet: this,
                    action: _$_eb2b[62],
                    rowNumber: a,
                    numOfRows: g,
                    insertBefore: d,
                    data: p
                });
                F.call(this.parent, _$_eb2b[287], this, a, g, c, d)
            };
            b.del = function (c, a) {
                if (!Q.call(this.parent, this)) return !1;
                if (!this.options.allowDeleteRow) return console.error(_$_eb2b[288]), !1;
                var d = null,
                    e = {},
                    f = [],
                    g = 0;
                void 0 == c && (a = this.getSelectedRows(), a[0] ? (c = parseInt(a[0].getAttribute(_$_eb2b[43])), a = a.length) : (c = this.options.data.length - 1, a = 1));
                var h = this.options.data.length - 1;
                if (void 0 == c || c > h || 0 > c) c = h;
                a || (a = 1);
                c + a >= this.options.data.length && (a = this.options.data.length -
                    c, a >= this.options.data.length && (a = this.options.data.length, this.resetSelection()));
                if (!1 === F.call(this.parent, _$_eb2b[289], this, c, a)) return console.log(_$_eb2b[290]), !1;
                if (-1 < parseInt(c) && 0 < parseInt(a)) {
                    h = parseInt(this.thead.lastChild.children[1].getAttribute(_$_eb2b[42]));
                    var l = parseInt(this.tbody.firstChild.getAttribute(_$_eb2b[43]));
                    this.resetBorders();
                    for (var m = c; m < c + a; m++)
                        for (var p = 0; p < this.options.columns.length; p++) d = E.getColumnNameFromCoords(p, m), N.getAttributes.call(this, _$_eb2b[291], d, e),
                            N.getAttributes.call(this, _$_eb2b[100], d, e), N.getAttributes.call(this, _$_eb2b[292], d, e), N.getAttributes.call(this, _$_eb2b[293], d, e), N.getAttributes.call(this, _$_eb2b[275], d, e), N.getAttributes.call(this, _$_eb2b[274], d, e), f[d] = !0;
                    if (d = W.getChain.call(this, e.values)) e.formulas = d;
                    for (m = c; m < c + a; m++) 0 <= Array.prototype.indexOf.call(this.tbody.children, this.rows[m].element) && (this.rows[m].element.parentNode.removeChild(this.rows[m].element), g++);
                    d = [];
                    for (m = c; m < c + a; m++) d.push({
                        id: this.getRowId(m),
                        row: m,
                        data: this.getRowData(m)
                    });
                    this.rows.splice(c, a);
                    m = this.records.splice(c, a);
                    this.options.data.splice(c, a);
                    ca.updateConfig.call(this, 1, 0, c, a);
                    J.call(this, _$_eb2b[63], {
                        rowNumber: c,
                        numOfRows: a,
                        data: d
                    });
                    X.references.call(this, f);
                    this.results && (this.results = va.refresh.call(this));
                    0 < this.options.pagination ? this.page(this.pageNumber, function () {
                        if (0 == this.tbody.children.length) {
                            var n = this.whichPage(this.options.data.length - 1);
                            this.page(n)
                        }
                    }) : G.limited.call(this) && 0 < g && (this.tbody.firstChild ? l = parseInt(this.tbody.firstChild.getAttribute(_$_eb2b[43])) :
                        this.results ? l = this.results.length ? this.results[0] : null : this.options.data.length ? l > this.options.data.length - 1 && (l = this.options.data.length - 1) : l = null, null !== l && (this.tbody.textContent = _$_eb2b[3], G.call(this), G[_$_eb2b[46]].call(this, l, h)));
                    X.update.call(this, !0);
                    this.refreshBorders();
                    T.call(this.parent, {
                        worksheet: this,
                        action: _$_eb2b[63],
                        rowNumber: c,
                        numOfRows: a,
                        insertBefore: 1,
                        data: d,
                        attributes: e
                    });
                    F.call(this.parent, _$_eb2b[294], this, c, a, m, d, e)
                }
            };
            return b
        }(),
        Xa = function () {
            var k = function (c, a) {
                Array.isArray(a) ||
                    (a = [a]);
                for (var d, e, f = Object.keys(a), g = 0; g < f.length; g++)
                    if (d = a[f[g]], e = this.options.columns[d]) this.colgroup[d] && (this.colgroup[d].style.width = c ? parseInt(e.width || this.options.defaultColWidth) + _$_eb2b[44] : _$_eb2b[145]), e.visible = c;
                G.refresh.call(this);
                0 < this.options.freezeColumns && ja[_$_eb2b[11]].call(this, this.options.freezeColumns);
                this.resetBorders()
            },
                b = function (c) {
                    c.getColumn = b[_$_eb2b[51]];
                    c.getColumnIdByName = b.getByName;
                    c.getPrimaryKey = b.getPrimaryKey;
                    c.getSelectedColumns = b.selected;
                    c.moveColumn =
                        b.move;
                    c.insertColumn = b.add;
                    c.deleteColumn = b.del;
                    c.showColumn = k.bind(c, !0);
                    c.hideColumn = k.bind(c, !1)
                };
            b[_$_eb2b[51]] = function (c) {
                return this.options.columns[c] || !1
            };
            b.getByName = function (c) {
                for (var a = this.options.columns, d = 0; d < a.length; d++)
                    if (a[d].name == c) return d;
                return !1
            };
            b.getPrimaryKey = function () {
                for (var c = 0; c < this.options.columns.length; c++)
                    if (1 == this.options.columns[c].primaryKey) return c;
                return !1
            };
            b.selected = function () {
                var c = [],
                    a = this.getHighlighted();
                if (a)
                    for (var d = a[0]; d <= a[2]; d++) c.push(d);
                return c
            };
            b.move = function (c, a) {
                if (!Q.call(this.parent, this)) return !1;
                c = parseInt(c);
                a = parseInt(a);
                if (!this.options.columns[c]) return console.error(_$_eb2b[295]), !1;
                if (!this.options.columns[a]) return console.error(_$_eb2b[296]), !1;
                if (0 < Object.keys(this.getMerge()).length) {
                    var d = null;
                    this.merged.cols[c] ? d = !0 : this.merged.cols[a] && (c > a ? this.merged.cols[a - 1] && (d = !0) : this.merged.cols[a + 1] && (d = !0));
                    d && this.destroyMerged()
                }
                this.resetBorders();
                d = this.options.freezeColumns;
                0 < d && (c < d || a < d) ? ja.reset.call(this) :
                    d = null;
                if (this.headers[a] && this.headers[a].parentNode) {
                    this.headers[c] || Ca.create.call(this, c);
                    var e = (c > a ? this.headers[a] : this.headers[a].nextSibling) || null;
                    this.headerContainer.insertBefore(this.headers[c], e);
                    e = (c > a ? this.colgroup[a] : this.colgroup[a].nextSibling) || null;
                    this.colgroupContainer.insertBefore(this.colgroup[c], e);
                    for (var f = 0; f < this.tbody.children.length; f++) {
                        var g = this.tbody.children[f].getAttribute(_$_eb2b[43]);
                        e = (c > a ? this.records[g][a].element : this.records[g][a].element.nextSibling) ||
                            null;
                        this.tbody.children[f].insertBefore(N[_$_eb2b[51]].call(this, c, g), e)
                    }
                    if (this.options.footers)
                        for (f = 0; f < this.tfoot.children.length; f++) e = (c > a ? this.footers.content[f][a].element : this.footers.content[f][a].element.nextSibling) || null, this.tfoot.children[f].insertBefore(aa.create.call(this, c, f), e)
                } else if (this.headers[c] && this.headers[c].parentNode) {
                    this.headerContainer.removeChild(this.headers[c]);
                    this.colgroupContainer.removeChild(this.colgroup[c]);
                    for (f = 0; f < this.tbody.children.length; f++) g = parseInt(this.tbody.children[f].getAttribute(_$_eb2b[43])),
                        this.tbody.children[f].removeChild(this.records[g][c].element);
                    if (this.options.footers)
                        for (f = 0; f < this.tfoot.children.length; f++) this.tfoot.children[f].removeChild(this.footers.content[f][c].element)
                }
                this.options.columns.splice(a, 0, this.options.columns.splice(c, 1)[0]);
                this.headers.splice(a, 0, this.headers.splice(c, 1)[0]);
                this.colgroup.splice(a, 0, this.colgroup.splice(c, 1)[0]);
                for (f = 0; f < this.rows.length; f++) this.dataType || this.options.data[f].splice(a, 0, this.options.data[f].splice(c, 1)[0]), this.records[f].splice(a,
                    0, this.records[f].splice(c, 1)[0]);
                if (this.options.footers)
                    for (f = 0; f < this.options.footers.length; f++) this.options.footers[f].splice(a, 0, this.options.footers[f].splice(c, 1)[0]), this.footers.content[f].splice(a, 0, this.footers.content[f].splice(c, 1)[0]);
                J.call(this, _$_eb2b[67], {
                    f: c,
                    t: a
                });
                X.references.call(this);
                X.update.call(this);
                0 < d && (c <= d || d <= d) && ja[_$_eb2b[11]].call(this, d);
                T.call(this.parent, {
                    worksheet: this,
                    action: _$_eb2b[67],
                    oldValue: c,
                    newValue: a
                });
                F.call(this.parent, _$_eb2b[297], this, c, a)
            };
            b.add =
                function (c, a, d, e, f, g) {
                    if (!Q.call(this.parent, this)) return !1;
                    if (!this.options.allowInsertColumn) return console.error(_$_eb2b[298]), !1;
                    var h = [];
                    if (0 < c) var l = parseInt(c);
                    else if (l = 1, Array.isArray(c) && !f)
                        if (Array.isArray(c[0])) f = c;
                        else {
                            f = [];
                            for (var m = 0; m < c.length; m++) f[m] = [c[m]]
                        } f || (f = []);
                    d = d ? !0 : !1;
                    c = this.options.columns.length - 1;
                    if (void 0 == a || a > c || 0 > a) a = c, d = !1;
                    l || (l = 1);
                    if (!1 === F.call(this.parent, _$_eb2b[299], this, a, l, d)) return console.log(_$_eb2b[300]), !1;
                    ka.reset.call(this);
                    if (1 == this.dataType) var p =
                        parseInt((_$_eb2b[3] + Date.now()).substr(-8));
                    e || (e = []);
                    for (m = 0; m < l; m++) e[m] || (e[m] = {
                        type: _$_eb2b[55],
                        source: [],
                        options: [],
                        width: this.options.defaultColWidth
                    }), 1 == this.dataType && typeof e[m].name == _$_eb2b[13] && (e[m].name = _$_eb2b[301] + (p + m));
                    var n = d ? a : a + 1;
                    this.options.columns = E.injectArray(this.options.columns, n, e);
                    p = function (z, D) {
                        for (var A = n; A < l + n; A++) Ca.create.call(this, A), z ? (this.headerContainer.insertBefore(this.headers[A], z), this.colgroupContainer.insertBefore(this.colgroup[A], D)) : (this.headerContainer.appendChild(this.headers[A]),
                            this.colgroupContainer.appendChild(this.colgroup[A]))
                    };
                    m = function (z, D) {
                        if (D)
                            for (var A = n; A < l + n; A++) this.rows[z].element.insertBefore(N[_$_eb2b[51]].call(this, A, z, !0), D);
                        else
                            for (A = n; A < l + n; A++) this.rows[z].element.appendChild(N[_$_eb2b[51]].call(this, A, z, !0))
                    };
                    var t = this.headers.splice(n),
                        v = this.colgroup.splice(n);
                    t[0] ? 0 <= Array.prototype.indexOf.call(this.headerContainer.children, t[0]) && p.call(this, t[0], v[0]) : this.headers[c] && 0 <= Array.prototype.indexOf.call(this.headerContainer.children, this.headers[c]) &&
                        p.call(this);
                    this.headers = this.headers.concat(t);
                    this.colgroup = this.colgroup.concat(v);
                    aa.adjust.call(this, n, l, 1);
                    oa.adjust.call(this, a, l, 1, g);
                    g = 0;
                    v = t = p = null;
                    for (var u = 0; u < this.options.data.length; u++) {
                        this.dataType || (t = this.options.data[u].splice(n));
                        v = this.records[u].splice(n);
                        g = 0;
                        for (var y = n; y < l + n; y++) p = f[u] && f[u][g] ? f[u][g] : _$_eb2b[3], M.call(this, y, u, p), h.push({
                            x: y,
                            y: u,
                            value: p
                        }), na.cell.call(this, y, u), g++;
                        this.rows[u].element && this.rows[u].element.parentNode && (v[0] ? 0 <= Array.prototype.indexOf.call(this.rows[u].element.children,
                            v[0].element) && m.call(this, u, v[0].element) : this.records[u][c].element && 0 <= Array.prototype.indexOf.call(this.rows[u].element.children, this.records[u][c].element) && m.call(this, u));
                        this.dataType || (this.options.data[u] = this.options.data[u].concat(t));
                        this.records[u] = this.records[u].concat(v)
                    }
                    this.merged.cols[n] && ca.updateConfig.call(this, 0, 1, n - 1, l);
                    J.call(this, _$_eb2b[64], {
                        numOfColumns: l,
                        columnNumber: a,
                        insertBefore: d ? 1 : 0,
                        properties: e,
                        data: f
                    });
                    X.references.call(this);
                    h.length && ea.applyValues.call(this,
                        h);
                    X.update.call(this);
                    this.refreshBorders();
                    T.call(this.parent, {
                        worksheet: this,
                        action: _$_eb2b[64],
                        columnNumber: a,
                        numOfColumns: l,
                        insertBefore: d,
                        properties: e,
                        data: f
                    });
                    F.call(this.parent, _$_eb2b[302], this, a, l, [], d)
                };
            b.del = function (c, a) {
                if (!Q.call(this.parent, this)) return !1;
                if (!this.options.allowDeleteColumn) return console.error(_$_eb2b[303]), !1;
                var d = {},
                    e = [],
                    f = 0;
                if (1 < this.options.columns.length) {
                    void 0 == c && (a = this.getSelectedColumns(!0), a.length ? (c = parseInt(a[0]), a = parseInt(a.length)) : (c = this.options.columns.length -
                        1, a = 1));
                    var g = this.options.columns.length - 1;
                    if (void 0 == c || c > g || 0 > c) c = g;
                    a || (a = 1);
                    c + a >= this.options.columns.length && (a = this.options.columns.length - c, a >= this.options.columns.length && (a--, console.error(_$_eb2b[304])));
                    if (!1 === F.call(this.parent, _$_eb2b[305], this, c, a)) return console.log(_$_eb2b[306]), !1;
                    if (-1 < parseInt(c) && 0 < parseInt(a)) {
                        g = parseInt(this.thead.lastChild.children[1].getAttribute(_$_eb2b[42]));
                        var h = 0;
                        this.tbody.firstChild && (h = parseInt(this.tbody.firstChild.getAttribute(_$_eb2b[43])));
                        this.resetBorders();
                        var l = null,
                            m = [],
                            p = [],
                            n = [];
                        var t = 0;
                        for (var v = c; v < c + a; v++) 0 <= Array.prototype.indexOf.call(this.headerContainer.children, this.headers[v]) && (this.colgroup[v].parentNode.removeChild(this.colgroup[v]), this.headers[v].parentNode.removeChild(this.headers[v]), f++), n[t] = this.options.columns[v], t++;
                        for (var u = 0; u < this.options.data.length; u++)
                            for (v = c; v < c + a; v++) t = E.getColumnNameFromCoords(v, u), N.getAttributes.call(this, _$_eb2b[291], t, d), N.getAttributes.call(this, _$_eb2b[100], t, d), N.getAttributes.call(this, _$_eb2b[292],
                                t, d), N.getAttributes.call(this, _$_eb2b[293], t, d), N.getAttributes.call(this, _$_eb2b[275], t, d), N.getAttributes.call(this, _$_eb2b[274], t, d), e[t] = !0, this.records[u][v].element && 0 <= Array.prototype.indexOf.call(this.rows[u].element.children, this.records[u][v].element) && this.records[u][v].element.parentNode.removeChild(this.records[u][v].element);
                        if (v = W.getChain.call(this, d.values)) d.formulas = v;
                        this.options.columns.splice(c, a);
                        this.headers.splice(c, a);
                        this.colgroup.splice(c, a);
                        for (u = 0; u < this.options.data.length; u++) this.dataType ||
                            (p[u] = this.options.data[u].splice(c, a)), m[u] = this.records[u].splice(c, a);
                        aa.adjust.call(this, c, a, 0);
                        (v = oa.adjust.call(this, c, a, 0)) && (l = {
                            nested: v
                        });
                        ca.updateConfig.call(this, 0, 0, c, a);
                        J.call(this, _$_eb2b[65], {
                            columnNumber: c,
                            numOfColumns: a
                        });
                        X.references.call(this, e);
                        G.limited.call(this) && 0 < f && (this.tbody.textContent = _$_eb2b[3], G.call(this), G[_$_eb2b[46]].call(this, h, g));
                        X.update.call(this, !0);
                        this.refreshBorders();
                        T.call(this.parent, {
                            worksheet: this,
                            action: _$_eb2b[65],
                            columnNumber: c,
                            numOfColumns: a,
                            insertBefore: 1,
                            data: p,
                            properties: n,
                            attributes: d,
                            extra: l
                        });
                        F.call(this.parent, _$_eb2b[307], this, c, a, m, n, d)
                    }
                }
            };
            return b
        }(),
        Qa = function () {
            var k = {},
                b = {},
                c = function (d) {
                    if (typeof d == _$_eb2b[17]) {
                        var e = d;
                        d = E.getCoordsFromColumnName(d);
                        var f = d[0];
                        d = d[1]
                    } else f = d.x, d = d.y, e = E.getColumnNameFromCoords(f, d);
                    if (this.records[d] && this.records[d][f] && this.records[d][f].element) {
                        b[e] = this.options.style[e];
                        this.records[d][f].element.setAttribute(_$_eb2b[100], _$_eb2b[3]);
                        if (this.options.cells && this.options.cells[e] && this.options.cells[e].align) var g =
                            this.options.cells[e].align;
                        else this.options.columns[f].align ? g = this.options.columns[f].align : this.options.defaultColAlign && (g = this.options.defaultColAlign);
                        g && (this.records[d][f].element.style.textAlign = g)
                    }
                    this.options.style && this.options.style[e] && delete this.options.style[e]
                },
                a = function (d) {
                    d.getStyle = a[_$_eb2b[51]];
                    d.setStyle = a[_$_eb2b[11]];
                    d.resetStyle = a.reset
                };
            a[_$_eb2b[51]] = function (d) {
                var e = this.options.style;
                if (!e) return !1;
                if (d) {
                    if (typeof d == _$_eb2b[17]) return e[d];
                    var f = [],
                        g = Object.keys(d);
                    if (0 < g.length)
                        for (var h = 0; h < g.length; h++) {
                            if (typeof d[h] == _$_eb2b[17]) var l = e && e[d[h]] ? e[d[h]] : _$_eb2b[3];
                            else l = E.getColumnNameFromCoords(d[h].x, d[h].y), l = e && e[l] ? e[l] : _$_eb2b[3];
                            f.push(l)
                        }
                    return f
                }
                return this.options.style
            };
            a[_$_eb2b[11]] = function (d, e, f, g) {
                if (!Q.call(this.parent, this)) return !1;
                this.options.style || (this.options.style = {});
                var h = this.options.style,
                    l = null,
                    m = [],
                    p = [],
                    n = {};
                p = function (u, y, z) {
                    if (!n[u] && (h[u] ? m = h[u].split(_$_eb2b[220]) : (h[u] = _$_eb2b[3], m = []), n[u] = {}, m.length))
                        for (var D = 0; D <
                            m.length; D++) m[D] && m[D].trim() && (l = m[D].split(_$_eb2b[137]), n[u][l[0].trim()] = l[1].trim());
                    b[u] || (b[u] = []);
                    k[u] || (k[u] = []);
                    n[u][y] || (n[u][y] = _$_eb2b[3]);
                    b[u].push([y + _$_eb2b[137] + n[u][y]]);
                    n[u][y] = n[u][y] && n[u][y] == z && !g ? _$_eb2b[3] : z;
                    k[u].push([y + _$_eb2b[137] + n[u][y]]);
                    z = E.getCoordsFromColumnName(u);
                    this.records[z[1]] && this.records[z[1]][z[0]] && this.records[z[1]][z[0]].element && (this.records[z[1]][z[0]].element.style[y] = n[u][y]);
                    n[u][y] || delete n[u][y]
                };
                k = {};
                b = {};
                if (e)
                    if (typeof d == _$_eb2b[17]) p.call(this,
                        d, e, f);
                    else {
                        if (d && d.length)
                            for (var t = 0; t < d.length; t++) {
                                var v = E.getColumnNameFromCoords(d[t].x, d[t].y);
                                p.call(this, v, e, f)
                            }
                    }
                else
                    for (f = Object.keys(d), t = 0; t < f.length; t++)
                        if (v = d[f[t]])
                            for (typeof v == _$_eb2b[17] && (v = v.split(_$_eb2b[220])), e = 0; e < v.length; e++) v[e] && (typeof v[e] == _$_eb2b[17] && (v[e] = v[e].split(_$_eb2b[137])), v[e][0].trim() && p.call(this, f[t], v[e][0].trim(), v[e][1].trim()));
                m = {};
                p = Object.keys(n);
                if (p.length) {
                    for (t = 0; t < p.length; t++) {
                        m[p[t]] || (m[p[t]] = []);
                        d = Object.keys(n[p[t]]);
                        for (e = 0; e < d.length; e++) m[p[t]].push(d[e] +
                            _$_eb2b[116] + n[p[t]][d[e]]);
                        this.options.style[p[t]] = m[p[t]].join(_$_eb2b[308])
                    }
                    p = Object.keys(k);
                    for (t = 0; t < p.length; t++) k[p[t]] = k[p[t]].join(_$_eb2b[220]);
                    p = Object.keys(b);
                    for (t = 0; t < p.length; t++) b[p[t]] = b[p[t]].join(_$_eb2b[220]);
                    this.refreshBorders();
                    T.call(this.parent, {
                        worksheet: this,
                        action: _$_eb2b[70],
                        oldValue: b,
                        newValue: k
                    });
                    J.call(this, _$_eb2b[70], [k]);
                    F.call(this.parent, _$_eb2b[309], this, k, b)
                }
            };
            a.reset = function (d) {
                if (!Q.call(this.parent, this)) return !1;
                if (d) {
                    b = {};
                    if (Array.isArray(d))
                        for (var e =
                            0; e < d.length; e++) c.call(this, d[e]);
                    else c.call(this, d);
                    this.refreshBorders();
                    T.call(this.parent, {
                        worksheet: this,
                        action: _$_eb2b[71],
                        cells: d,
                        oldValue: b
                    });
                    J.call(this, _$_eb2b[71], [d]);
                    F.call(this.parent, _$_eb2b[311], this, d)
                } else console.error(_$_eb2b[310])
            };
            a.update = function (d) {
                k = {};
                b = {};
                for (var e, f = Object.keys(d), g = 0; g < f.length; g++) {
                    b[f[g]] = this.options.style[f[g]] || _$_eb2b[3];
                    k[f[g]] = this.options.style[f[g]] = d[f[g]];
                    e = E.getCoordsFromColumnName(f[g]);
                    var h = e[0],
                        l = e[1];
                    this.records[l] && this.records[l][h] &&
                        this.records[l][h].element && (this.records[l][h].element.setAttribute(_$_eb2b[100], d[f[g]]), !this.records[l][h].element.style.textAlign && (e = this.options.columns[h].align ? this.options.columns[h].align : this.options.defaultColAlign)) && (this.records[l][h].element.style.textAlign = e)
                }
                J.call(this, _$_eb2b[71], [f]);
                J.call(this, _$_eb2b[70], [k]);
                F.call(this.parent, _$_eb2b[309], this, k, b)
            };
            return a
        }(),
        Ya = function () {
            var k = function (b) {
                b.getComments = k[_$_eb2b[51]];
                b.setComments = k[_$_eb2b[11]]
            };
            k[_$_eb2b[51]] = function (b) {
                return b &&
                    typeof b == _$_eb2b[17] ? this.options.comments && this.options.comments[b] ? this.options.comments[b] : !1 : this.options.comments
            };
            k[_$_eb2b[11]] = function (b, c) {
                if (!Q.call(this.parent, this)) return !1;
                if (typeof b == _$_eb2b[17]) {
                    var a = {};
                    a[b] = c
                } else a = b;
                if (c = F.call(this.parent, _$_eb2b[312], this, a)) a = c;
                else if (!1 === c) return !1;
                this.options.comments || (this.options.comments = {});
                b = {};
                var d = Object.keys(a);
                if (d.length) {
                    for (var e = 0; e < d.length; e++) {
                        var f = E.getCoordsFromColumnName(d[e]);
                        this.records[f[1]] && this.records[f[1]][f[0]] &&
                            (b[d[e]] = this.options.comments[d[e]] || _$_eb2b[3], (c = a[d[e]]) ? this.options.comments[d[e]] = c : delete this.options.comments[d[e]], this.records[f[1]][f[0]].element && (c ? this.records[f[1]][f[0]].element.setAttribute(_$_eb2b[277], c) : this.records[f[1]][f[0]].element.removeAttribute(_$_eb2b[277])))
                    }
                    T.call(this.parent, {
                        worksheet: this,
                        action: _$_eb2b[75],
                        newValue: a,
                        oldValue: b
                    });
                    J.call(this, _$_eb2b[75], [a]);
                    F.call(this.parent, _$_eb2b[313], this, a, b)
                }
            };
            return k
        }(),
        Za = function () {
            var k = function (b) {
                b.getMeta = k[_$_eb2b[51]];
                b.setMeta = k[_$_eb2b[11]];
                b.resetMeta = k.reset
            };
            k[_$_eb2b[51]] = function (b, c) {
                if (b) {
                    if (this.options.meta[b]) return c ? this.options.meta[b][c] : this.options.meta[b]
                } else return this.options.meta
            };
            k[_$_eb2b[11]] = function (b, c, a) {
                if (!Q.call(this.parent, this)) return !1;
                this.options.meta || (this.options.meta = {});
                if (typeof b == _$_eb2b[17] && c) {
                    a || (a = _$_eb2b[3]);
                    var d = {};
                    d[b] = {};
                    d[b][c] = a;
                    b = d
                }
                c = Object.keys(b);
                if (c.length) {
                    a = {};
                    for (d = 0; d < c.length; d++) {
                        this.options.meta[c[d]] || (this.options.meta[c[d]] = {});
                        a[c[d]] ||
                            (a[c[d]] = {});
                        for (var e = Object.keys(b[c[d]]), f = 0; f < e.length; f++) this.options.meta[c[d]][e[f]] = b[c[d]][e[f]], a[c[d]][e[f]] = b[c[d]][e[f]]
                    }
                    Object.keys(a).length && (J.call(this, _$_eb2b[314], [a]), F.call(this.parent, _$_eb2b[315], this, a))
                }
            };
            k.reset = function () {
                this.options.meta = {};
                J.call(this, _$_eb2b[316], {});
                F.call(this.parent, _$_eb2b[317], this, {})
            };
            return k
        }(),
        $a = function () {
            var k = function (b) {
                b.orderBy = k.execute
            };
            k.handler = function (b, c) {
                return function (a, d) {
                    a = a[1];
                    d = d[1];
                    return b ? a === _$_eb2b[3] && d !== _$_eb2b[3] ?
                        1 : a !== _$_eb2b[3] && d === _$_eb2b[3] ? -1 : a > d ? -1 : a < d ? 1 : 0 : a === _$_eb2b[3] && d !== _$_eb2b[3] ? 1 : a !== _$_eb2b[3] && d === _$_eb2b[3] ? -1 : a > d ? 1 : a < d ? -1 : 0
                }
            };
            k.execute = function (b, c, a) {
                if (0 <= b) {
                    c = null == c ? this.headers[b].classList.contains(_$_eb2b[318]) ? 1 : 0 : c ? 1 : 0;
                    for (var d, e, f = [], g = 0; g < this.options.data.length; g++) d = Ha[_$_eb2b[51]].call(this, b, g), e = this.records[g][b].v, e = d.type == _$_eb2b[263] || d.type == _$_eb2b[268] || d.type == _$_eb2b[269] || d.type == _$_eb2b[319] || d.type == _$_eb2b[14] || d.type == _$_eb2b[320] ? Number(e) : jSuites.isNumeric(e) ?
                        Number(e) : e.toLowerCase(), f[g] = [g, e];
                    g = typeof this.parent.config.sorting === _$_eb2b[85] ? this.parent.config.sorting(c, b) : k.handler(c, b);
                    f.sort(g);
                    d = a ? !0 : !1;
                    if (!a)
                        for (a = [], g = 0; g < f.length; g++) a[g] = f[g][0];
                    if (g = F.call(this.parent, _$_eb2b[321], this, b, c, a)) a = g;
                    else if (!1 === g) return !1;
                    if (d) this.destroyMerged();
                    else if (0 < Object.keys(this.getMerge()).length)
                        if (confirm(B(_$_eb2b[322]))) this.destroyMerged();
                        else return !1;
                    if (a.length < f.length)
                        for (g = 0; g < f.length; g++) - 1 == a.indexOf(f[g][0]) && a.push(f[g][0]);
                    f = [];
                    for (g = 0; g < a.length; g++) f[a[g]] = g;
                    Ea.call(this, !1);
                    ka.reset.call(this);
                    J.call(this, _$_eb2b[76], {
                        column: b,
                        direction: c,
                        order: a
                    });
                    k.update.call(this, a);
                    k.arrow.call(this, b, c);
                    ka.refresh.call(this);
                    T.call(this.parent, {
                        worksheet: this,
                        action: _$_eb2b[76],
                        column: b,
                        newValue: a,
                        oldValue: f,
                        direction: c
                    });
                    F.call(this.parent, _$_eb2b[323], this, b, c, a);
                    return !0
                }
            };
            k.update = function (b) {
                for (var c = [], a = 0; a < b.length; a++) c[a] = this.options.data[b[a]];
                for (a = 0; a < b.length; a++) this.options.data[a] = c[a];
                c = [];
                for (a = 0; a < b.length; a++) c[a] =
                    this.records[b[a]];
                this.records = c;
                c = [];
                for (a = 0; a < b.length; a++) c[a] = this.rows[b[a]];
                this.rows = c;
                X.references.call(this);
                this.searchInput && this.searchInput.value ? qa.update.call(this, null) : da.update.call(this)
            };
            k.arrow = function (b, c) {
                for (var a = 0; a < this.headers.length; a++) this.headers[a].classList.remove(_$_eb2b[324]), this.headers[a].classList.remove(_$_eb2b[318]);
                c ? this.headers[b].classList.add(_$_eb2b[324]) : this.headers[b].classList.add(_$_eb2b[318])
            };
            return k
        }(),
        ua = function () {
            var k = function (b) {
                b.whichPage =
                    k.whichPage;
                b.quantityOfPages = k.quantityOfPages;
                b.page = k[_$_eb2b[11]];
                b.updatePagination = k.update
            };
            k.build = function () {
                this.pageNumber = 0;
                var b = document.createElement(_$_eb2b[20]),
                    c = document.createElement(_$_eb2b[20]);
                this.pagination = document.createElement(_$_eb2b[20]);
                this.pagination.classList.add(_$_eb2b[325]);
                this.pagination.appendChild(b);
                this.pagination.appendChild(c);
                this.options.pagination || (this.pagination.style.display = _$_eb2b[40]);
                this.element.appendChild(this.pagination)
            };
            k.pageUp = function () {
                0 <
                    this.pageNumber && (this.pageNumber--, k[_$_eb2b[11]].call(this, this.pageNumber))
            };
            k.pageDown = function () {
                this.pageNumber < k.quantityOfPages.call(this) - 1 && (this.pageNumber++, k[_$_eb2b[11]].call(this, this.pageNumber))
            };
            k.whichPage = function (b) {
                if (0 < this.options.pagination) return this.rows[b] ? (this.results && (b = this.results.indexOf(parseInt(b))), Math.ceil((parseInt(b) + 1) / parseInt(this.options.pagination)) - 1) : null;
                console.log(_$_eb2b[326]);
                return !1
            };
            k.quantityOfPages = function () {
                if (0 < this.options.pagination) return Math.ceil((this.results ?
                    this.results.length : this.rows.length) / parseInt(this.options.pagination));
                console.log(_$_eb2b[326]);
                return !1
            };
            k[_$_eb2b[11]] = function (b, c) {
                var a = this.pageNumber,
                    d = parseInt(this.options.pagination);
                if (d) {
                    if (null == b || -1 == b) b = Math.ceil((this.results ? this.results : this.rows).length / d) - 1;
                    if (!1 === F.call(this.parent, _$_eb2b[328], this, b, a, d)) return !1;
                    this.pageNumber = b;
                    G.resetY.call(this);
                    aa.refresh.call(this);
                    typeof c == _$_eb2b[85] && c.call(this);
                    F.call(this.parent, _$_eb2b[329], this, b, a, d)
                } else console.error(_$_eb2b[327])
            };
            k.update = function () {
                this.pagination.children[0].innerHTML = _$_eb2b[3];
                this.pagination.children[1].innerHTML = _$_eb2b[3];
                if (this.options.pagination) {
                    var b = this.results ? this.results.length : this.rows.length;
                    if (b) {
                        b = Math.ceil(b / this.options.pagination);
                        if (6 > this.pageNumber) var c = 1,
                            a = 10 > b ? b : 10;
                        else 5 > b - this.pageNumber ? (c = b - 9, a = b, 1 > c && (c = 1)) : (c = this.pageNumber - 4, a = this.pageNumber + 5);
                        if (1 < c) {
                            var d = document.createElement(_$_eb2b[20]);
                            d.className = _$_eb2b[331];
                            d.innerHTML = _$_eb2b[218];
                            d.title = 1;
                            this.pagination.children[1].appendChild(d)
                        }
                        for (; c <=
                            a; c++) d = document.createElement(_$_eb2b[20]), d.className = _$_eb2b[331], d.innerHTML = c, this.pagination.children[1].appendChild(d), this.pageNumber == c - 1 && d.classList.add(_$_eb2b[332]);
                        a < b && (d = document.createElement(_$_eb2b[20]), d.className = _$_eb2b[331], d.innerHTML = _$_eb2b[219], d.title = b, this.pagination.children[1].appendChild(d));
                        this.pagination.children[0].innerHTML = B(_$_eb2b[333], [this.pageNumber + 1, b])
                    } else this.pagination.children[0].innerHTML = B(_$_eb2b[330])
                }
            };
            return k
        }(),
        ja = function () {
            var k = function (b) {
                b.setFreezeColumns =
                    k[_$_eb2b[11]];
                b.resetFreezeColumns = k.reset
            };
            k.width = function () {
                if (this.options.freezeColumns) {
                    k.headers.call(this);
                    for (var b = 0; b < this.rows.length; b++) this.rows[b].element && k.update.call(this, b)
                }
            };
            k.update = function (b) {
                var c, a;
                if (a = this.options.freezeColumns) {
                    var d = ya.call(this) ? 50 : 0;
                    if (this.rows[b].element)
                        for (var e = 0; e < a; e++) {
                            if (c = this.records[b][e].element) c.classList.add(_$_eb2b[50]), c.style.left = d + 1 + _$_eb2b[44];
                            !1 !== this.options.columns[e].visible && (d += this.options.columns[e].width)
                        }
                }
            };
            k.headers =
                function () {
                    var b = ya.call(this) ? 50 : 0,
                        c;
                    if (c = this.options.freezeColumns)
                        for (var a = 0; a < c; a++) {
                            if (this.headers[a]) {
                                this.headers[a].classList.add(_$_eb2b[50]);
                                this.headers[a].style.left = b + 1 + _$_eb2b[44];
                                if (this.options.footers)
                                    for (var d = 0; d < this.options.footers.length; d++) this.footers.content[d][a].element.classList.add(_$_eb2b[50]), this.footers.content[d][a].element.style.left = b + 1 + _$_eb2b[44];
                                !1 !== this.options.columns[a].visible && (b += this.options.columns[a].width)
                            }
                        } else k.reset.call(this)
                };
            k[_$_eb2b[11]] =
                function (b) {
                    k.reset.call(this);
                    this.options.freezeColumns = b;
                    k.width.call(this);
                    G.limited.call(this) && U.call(this)
                };
            k.reset = function () {
                G[_$_eb2b[46]].call(this, null, this.options.freezeColumns);
                for (var b, c = 1; c <= this.options.freezeColumns; c++) {
                    if (b = this.thead.lastChild) b.children[c].classList.remove(_$_eb2b[50]), b.children[c].style.left = _$_eb2b[3];
                    for (var a = 0; a < this.rows.length; a++)
                        if (b = this.rows[a].element) b.children[c].classList.remove(_$_eb2b[50]), b.children[c].style.left = _$_eb2b[3];
                    if (this.options.footers)
                        for (a =
                            0; a < this.options.footers.length; a++) this.tfoot.children[a].children[c].classList.remove(_$_eb2b[50]), this.tfoot.children[a].children[c].style.left = _$_eb2b[3]
                }
                this.options.freezeColumns = 0
            };
            k.getWidth = function () {
                var b = ya.call(this) ? 50 : 0;
                if (0 < this.options.freezeColumns)
                    for (var c = 0; c <= this.options.freezeColumns; c++) b = this.thead.lastChild.children[c] ? b + this.thead.lastChild.children[c].offsetWidth : b + this.options.columns[c].width;
                return b
            };
            return k
        }(),
        X = function () {
            var k = function (a, d) {
                if (this.options[a]) {
                    for (var e = {}, f = Object.keys(this.options[a]), g = 0; g < f.length; g++) e[d && void 0 != d[f[g]] ? d[f[g]] : f[g]] = this.options[a][f[g]];
                    this.options[a] = e
                }
            },
                b = function () {
                    if (0 < this.options.minSpareRows) {
                        for (var a = 0, d = this.rows.length - 1; 0 <= d; d--) {
                            for (var e = !1, f = 0; f < this.options.columns.length; f++) M.call(this, f, d) && (e = !0);
                            if (e) break;
                            else a++
                        }
                        0 < this.options.minSpareRows - a && this.insertRow(this.options.minSpareRows - a)
                    }
                    if (0 < this.options.minSpareCols) {
                        a = 0;
                        for (f = this.options.columns.length - 1; 0 <= f; f--) {
                            e = !1;
                            for (d = 0; d < this.rows.length; d++) M.call(this,
                                f, d) && (e = !0);
                            if (e) break;
                            else a++
                        }
                        0 < this.options.minSpareCols - a && this.insertColumn(this.options.minSpareCols - a)
                    }
                },
                c = function () { };
            c.build = function () {
                var a = this;
                this.content = document.createElement(_$_eb2b[20]);
                this.content.classList.add(_$_eb2b[334]);
                this.content.addEventListener(_$_eb2b[335], function (f) {
                    if (G.limited.call(a) && !a.options.pagination) {
                        var g = Math.abs(f.deltaX),
                            h = Math.abs(f.deltaY);
                        f.shiftKey || g > h ? 0 > f.deltaX || 0 > f.deltaY ? G.pageLeft.call(a, 1, null, f) : G.pageRight.call(a, 1, null, f) : 0 > f.deltaY ?
                            G.pageUp.call(a, 1, null, f) : G.pageDown.call(a, 1, null, f)
                    }
                });
                this.content.addEventListener(_$_eb2b[336], function (f) {
                    if (!a.edition) return f.preventDefault(), !1
                });
                this.table = document.createElement(_$_eb2b[337]);
                this.thead = document.createElement(_$_eb2b[338]);
                this.tbody = document.createElement(_$_eb2b[339]);
                this.tfoot = document.createElement(_$_eb2b[340]);
                this.headers = [];
                this.colgroup = [];
                this.colgroupContainer = document.createElement(_$_eb2b[341]);
                var d = document.createElement(_$_eb2b[170]);
                d.setAttribute(_$_eb2b[153],
                    _$_eb2b[342]);
                this.colgroupContainer.appendChild(d);
                this.headerContainer = document.createElement(_$_eb2b[146]);
                d = document.createElement(_$_eb2b[148]);
                d.classList.add(_$_eb2b[343]);
                this.headerContainer.appendChild(d);
                this.thead.appendChild(this.headerContainer);
                this.table.classList.add(_$_eb2b[344]);
                this.table.setAttribute(_$_eb2b[345], _$_eb2b[99]);
                this.table.setAttribute(_$_eb2b[346], _$_eb2b[99]);
                this.table.setAttribute(_$_eb2b[347], _$_eb2b[348]);
                this.table.appendChild(this.colgroupContainer);
                this.table.appendChild(this.thead);
                this.table.appendChild(this.tbody);
                this.table.appendChild(this.tfoot);
                this.options.textOverflow || this.table.classList.add(_$_eb2b[349]);
                !1 === this.options.selectLockedCells && this.table.classList.add(_$_eb2b[350]);
                this.corner = document.createElement(_$_eb2b[20]);
                this.corner.className = _$_eb2b[351];
                this.corner.setAttribute(_$_eb2b[347], _$_eb2b[352]);
                this.corner.setAttribute(_$_eb2b[353], _$_eb2b[354]);
                0 == this.options.selectionCopy && (this.corner.style.display = _$_eb2b[40]);
                this.scrollX = U.build.call(a, _$_eb2b[45]);
                this.scrollY = U.build.call(a, _$_eb2b[355]);
                d = document.createElement(_$_eb2b[20]);
                d.appendChild(this.content);
                d.appendChild(this.scrollY);
                d.classList.add(_$_eb2b[356]);
                var e = document.createElement(_$_eb2b[20]);
                e.appendChild(d);
                e.appendChild(this.scrollX);
                e.classList.add(_$_eb2b[357]);
                this.content.appendChild(this.table);
                this.content.appendChild(this.corner);
                this.element.appendChild(e);
                1 == this.options.tableOverflow && (this.options.tableHeight || (this.options.tableHeight = document.body.offsetHeight - 18),
                    this.options.tableWidth || (this.options.tableWidth = document.body.offsetWidth - 18), this.content.style.overflow = _$_eb2b[49], this.content.style.maxWidth = parseInt(this.options.tableWidth) + _$_eb2b[44], this.options.pagination || (this.content.style.maxHeight = parseInt(this.options.tableHeight) + _$_eb2b[44]));
                this.options.freezeColumns > this.options.columns.length && console.error(_$_eb2b[358]);
                qa.build.call(this);
                oa.build.call(this);
                ua.build.call(this);
                Fa.build.call(this);
                ca.build.call(this);
                aa.build.call(this);
                G.call(this);
                da.onload.call(this);
                b.call(this)
            };
            c.references = function (a) {
                var d = [],
                    e = [],
                    f = null,
                    g = null,
                    h = null,
                    l = null;
                for (f = 0; f < this.options.columns.length; f++)
                    if (g = this.headers[f]) h = g.getAttribute(_$_eb2b[42]), h != f && (g.setAttribute(_$_eb2b[42], f), g.getAttribute(_$_eb2b[167]) || (g.innerHTML = E.getColumnName(f)));
                for (var m = 0; m < this.rows.length; m++) g = this.rows[m], l = g.y, l != m && (g.y = m, d[l] = m, g.element && (g.element.setAttribute(_$_eb2b[43], m), g.element.children[0].setAttribute(_$_eb2b[43], m), f = this.options.rows &&
                    this.options.rows[l] && this.options.rows[l].title ? this.options.rows[l].title : m + 1, g.element.children[0].innerHTML = f));
                var p = function (t, v) {
                    g = this.records[v][t];
                    h = g.x;
                    l = g.y;
                    h != t && (g.x = t, g.element && g.element.setAttribute(_$_eb2b[42], t));
                    l != v && (g.y = v, g.element && g.element.setAttribute(_$_eb2b[43], v));
                    if (h != t || l != v) {
                        var u = E.getColumnNameFromCoords(h, l);
                        t = E.getColumnNameFromCoords(t, v);
                        e[u] = t
                    }
                };
                if (a) {
                    m = function (t, v) {
                        t && t[v] && delete t[v]
                    };
                    var n = Object.keys(a);
                    for (f = 0; f < n.length; f++) m(this, n[f]), m(this.options.meta,
                        n[f]), m(this.options.cells, n[f]), m(this.options.style, n[f]), m(this.options.comments, n[f])
                }
                for (m = 0; m < this.rows.length; m++)
                    for (f = 0; f < this.options.columns.length; f++) p.call(this, f, m);
                k.call(this, _$_eb2b[359], d);
                k.call(this, _$_eb2b[291], e);
                k.call(this, _$_eb2b[292], e);
                k.call(this, _$_eb2b[100], e);
                k.call(this, _$_eb2b[293], e);
                k.call(this, _$_eb2b[275], e);
                this.options.mergeCells && ca.build.call(this);
                W.updateAll.call(this, e, a);
                return e
            };
            c.update = function (a) {
                a && b.call(this);
                if (typeof this.parent.config.updateTable ==
                    _$_eb2b[85])
                    for (var d = 0; d < this.rows.length; d++)
                        for (var e = 0; e < this.options.columns.length; e++) a = this.records[d][e], this.parent.config.updateTable.call(this, this, a.element, e, d, M.call(this, e, d), a.r ? a.r : a.v);
                U.call(this);
                aa.refresh.call(this)
            };
            return c
        }(),
        na = function () {
            var k = {
                spreadsheet: function (b, c) {
                    var a = {
                        name: null,
                        config: {},
                        el: b,
                        element: b,
                        plugins: [],
                        worksheets: [],
                        history: [],
                        historyIndex: -1,
                        queue: [],
                        ignoreEvents: !1,
                        ignoreHistory: !1,
                        ignorePersistence: !1
                    };
                    Da.spreadsheet.call(a, c);
                    a.config.license && (r.license =
                        a.config.license);
                    b.spreadsheet = a;
                    b.jspreadsheet = b.jexcel = a.worksheets;
                    r.spreadsheet.push(a);
                    a.name || (a.name = jSuites.guid());
                    k.bind(a.config.root ? a.config.root : document);
                    var d = a.config.tabs;
                    typeof d !== _$_eb2b[84] && (d = {
                        allowCreate: d ? !0 : !1,
                        hideHeaders: d ? !1 : !0,
                        allowChangePosition: a.config.allowMoveWorksheet ? !0 : !1
                    });
                    d.maxWidth || (d.maxWidth = b.offsetWidth - 50 + _$_eb2b[44]);
                    d.animation = void 0 == d.animation ? !0 : d.animation;
                    d.onbeforecreate = function () {
                        a.createWorksheet();
                        return !1
                    };
                    d.onclick = function (f, g, h, l, m) {
                        0 <=
                            h && a.openWorksheet(h)
                    };
                    d.onchangeposition = function (f, g, h) {
                        a.updateWorksheet(g, h)
                    };
                    jSuites.tabs(b, d);
                    b.classList.add(_$_eb2b[360]);
                    1 == c.fullscreen && b.classList.add(_$_eb2b[15]);
                    var e = document.createElement(_$_eb2b[20]);
                    a.toolbar = document.createElement(_$_eb2b[20]);
                    a.toolbar.className = _$_eb2b[361];
                    e.appendChild(a.toolbar);
                    a.filter = da.build.call(a);
                    e.appendChild(a.filter);
                    a.helper = document.createElement(_$_eb2b[20]);
                    a.helper.className = _$_eb2b[362];
                    e.appendChild(a.helper);
                    a.loading = document.createElement(_$_eb2b[20]);
                    a.loading.classList.add(_$_eb2b[363]);
                    e.appendChild(a.loading);
                    e.appendChild(ba.build(a));
                    e.appendChild(Ra(a));
                    a.textarea = document.createElement(_$_eb2b[364]);
                    a.textarea.className = _$_eb2b[365];
                    a.textarea.tabIndex = _$_eb2b[366];
                    e.appendChild(a.textarea);
                    d.position === _$_eb2b[367] ? b.insertBefore(e, b.children[0]) : b.insertBefore(e, b.children[1]);
                    b.appendChild(Ja.call(a, r.license));
                    a.createWorksheet = ha.createWorksheet;
                    a.deleteWorksheet = ha.deleteWorksheet;
                    a.renameWorksheet = ha.renameWorksheet;
                    a.updateWorksheet =
                        ha.updateWorksheet;
                    a.openWorksheet = ha.openWorksheet;
                    a.moveWorksheet = ha.moveWorksheet;
                    a.getWorksheet = ha.getWorksheet;
                    a.getWorksheetActive = ha.getWorksheetActive;
                    a.getWorksheetInstance = ha.getWorksheetInstance;
                    a.getConfig = function () {
                        var f = a.config;
                        f.worksheets = [];
                        for (var g = 0; g < a.worksheets.length; g++) f.worksheets.push(a.worksheets[g].getConfig());
                        return f
                    };
                    a.fullscreen = Ka;
                    a.progress = La;
                    a.undo = T.undo;
                    a.redo = T.redo;
                    a.setToolbar = fa[_$_eb2b[11]];
                    a.getToolbar = fa[_$_eb2b[51]];
                    a.showToolbar = fa.show;
                    a.hideToolbar =
                        fa.hide;
                    a.tools = e;
                    1 !== a.edition && (b = Object.keys(a.config.definedNames)) && b.length && (a.config.definedNames = {}, console.log(_$_eb2b[368]));
                    c.plugins && pa.call(a, c.plugins);
                    a.setPlugins = function (f) {
                        pa.call(a, f)
                    };
                    b = Object.keys(r.extensions);
                    if (b.length)
                        for (d = 0; d < b.length; d++)
                            if (typeof r.extensions[b[d]].oninit == _$_eb2b[85]) r.extensions[b[d]].oninit(a, c);
                    return a
                },
                worksheet: function (b, c) {
                    var a = {};
                    b.worksheets.push(a);
                    a.parent = b;
                    Da(a);
                    Ta(a);
                    za(a);
                    ka(a);
                    ba(a);
                    Fa(a);
                    Va(a);
                    ca(a);
                    Qa(a);
                    Wa(a);
                    ea(a);
                    W(a);
                    N(a);
                    Aa(a);
                    Xa(a);
                    Za(a);
                    Ya(a);
                    Ca(a);
                    aa(a);
                    oa(a);
                    qa(a);
                    da(a);
                    fa(a);
                    ta(a);
                    ab(a);
                    bb(a);
                    ua(a);
                    ha(a);
                    ja(a);
                    Ua(a);
                    $a(a);
                    Ia(a);
                    Ha(a);
                    Da.worksheet.call(a, c);
                    !r.license && a.options.license && (r.license = a.options.license);
                    b.element.jexcel = b.element.jexcel ? b.worksheets : a;
                    pa.execute.call(a.parent, _$_eb2b[369], [a]);
                    a.onload = function () {
                        this.options.worksheetName || (this.options.worksheetName = _$_eb2b[370] + ha.nextName());
                        var d = this.options.worksheetName;
                        I(d) && console.log(_$_eb2b[371] + d + _$_eb2b[372]);
                        I(d, a);
                        this.element = b.element.tabs.appendElement(this.options.worksheetName,
                            function (e, f) {
                                a.options.worksheetState === _$_eb2b[49] && (f.style.display = _$_eb2b[40])
                            });
                        this.element.classList.add(_$_eb2b[373]);
                        this.element.jexcel = this;
                        X.build.call(this);
                        pa.execute.call(this.parent, _$_eb2b[374], [this]);
                        this.options.data && 500 < this.options.data.length && !G.limited.call(this) && !this.options.pagination && console.error(_$_eb2b[375])
                    };
                    return a
                },
                row: function (b, c) {
                    this.rows[b] || (this.rows[b] = {
                        element: null,
                        y: b
                    });
                    c && (this.rows[b].id = c, ta[_$_eb2b[11]].call(this, c));
                    this.options.rows && this.options.rows[b] &&
                        this.options.rows[b].height && (this.rows[b].height = this.options.rows[b].height);
                    this.records[b] || (this.records[b] = []);
                    this.options.data[b] || (this.options.data[b] = this.dataType ? {} : []);
                    for (c = 0; c < this.options.columns.length; c++) k.cell.call(this, c, b)
                },
                cell: function (b, c, a) {
                    typeof a == _$_eb2b[13] && (a = M.call(this, b, c), null === a || void 0 === a) && (a = _$_eb2b[3], M.call(this, b, c, a));
                    this.records[c][b] || (this.records[c][b] = {
                        element: null,
                        x: b,
                        y: c,
                        v: a
                    })
                },
                bind: function (b) {
                    for (var c = Object.keys(Ba), a = 0; a < c.length; a++)(c[a] ==
                        _$_eb2b[376] ? window : b).addEventListener(c[a], Ba[c[a]], c[a] == _$_eb2b[377] || c[a] == _$_eb2b[378] || c[a] == _$_eb2b[379] ? {
                            passive: !1
                        } : {})
                },
                unbind: function (b) {
                    for (var c = Object.keys(Ba), a = 0; a < c.length; a++)(c[a] == _$_eb2b[376] ? window : b).removeEventListener(c[a], Ba[c[a]])
                }
            };
            return k
        }(),
        ab = function () {
            var k = function (b) {
                b.getWidth = k[_$_eb2b[51]];
                b.setWidth = k[_$_eb2b[11]]
            };
            k[_$_eb2b[51]] = function (b) {
                var c = this.options.columns;
                if (typeof b === _$_eb2b[13]) {
                    b = [];
                    for (var a = 0; a < c.length; a++) b.push(c[a].width)
                } else b = c[b].width;
                return b
            };
            k[_$_eb2b[11]] = function (b, c, a) {
                if (!Q.call(this.parent, this)) return !1;
                if (c) {
                    if (Array.isArray(b)) {
                        a || (a = []);
                        for (var d = 0; d < b.length; d++) {
                            a[d] || (a[d] = this.options.columns[b[d]].width);
                            var e = Array.isArray(c) && c[d] ? c[d] : c;
                            this.options.columns[b[d]].width = e;
                            this.colgroup[b[d]] && this.colgroup[b[d]].setAttribute(_$_eb2b[153], e)
                        }
                    } else a || (a = this.options.columns[b].width), this.options.columns[b].width = c, this.colgroup[b] && this.colgroup[b].setAttribute(_$_eb2b[153], c);
                    this.refreshBorders();
                    T.call(this.parent, {
                        worksheet: this,
                        action: _$_eb2b[72],
                        column: b,
                        oldValue: a,
                        newValue: c
                    });
                    J.call(this, _$_eb2b[72], {
                        column: b,
                        width: c,
                        oldWidth: a
                    });
                    F.call(this.parent, _$_eb2b[380], this, b, c, a)
                }
                G.refresh.call(this)
            };
            return k
        }(),
        bb = function () {
            var k = function (c) {
                c.getHeight = k[_$_eb2b[51]];
                c.setHeight = k[_$_eb2b[11]]
            };
            k[_$_eb2b[51]] = function (c) {
                var a = null;
                if (c) this.rows[c].height ? a = this.rows[c].height : this.rows[c].element && (a = this.rows[c].element.style.height);
                else {
                    a = [];
                    for (var d = 0; d < this.rows.length; d++)(c = this.rows[d].height ?
                        this.rows[d].height : this.rows[d].element ? this.rows[d].element.style.height : null) && (a[d] = c)
                }
                return a
            };
            var b = function (c, a) {
                a = parseInt(a);
                var d = this.rows[c].height ? this.rows[c].height : this.rows[c].element ? this.rows[c].element.offsetHeight : this.options.defaultRowHeight;
                this.rows[c].element && (this.rows[c].element.style.height = a + _$_eb2b[44]);
                this.options.rows[c] || (this.options.rows[c] = {});
                this.options.rows[c].height = a + _$_eb2b[44];
                this.rows[c].height = a;
                return d
            };
            k[_$_eb2b[11]] = function (c, a, d) {
                if (!Q.call(this.parent,
                    this)) return !1;
                var e, f = typeof d == _$_eb2b[13] ? !0 : !1;
                if (a) {
                    if (Array.isArray(c))
                        for (!0 === f && (d = []), e = 0; e < c.length; e++) {
                            var g = Array.isArray(a) && a[e] ? a[e] : a;
                            b.call(this, c[e], g);
                            1 == f && d.push(f)
                        } else e = b.call(this, c, a), 1 == f && (d = e);
                    this.refreshBorders();
                    T.call(this.parent, {
                        worksheet: this,
                        action: _$_eb2b[73],
                        row: c,
                        oldValue: d,
                        newValue: a
                    });
                    J.call(this, _$_eb2b[73], {
                        row: c,
                        height: a,
                        oldHeight: d
                    });
                    F.call(this.parent, _$_eb2b[381], this, c, a, d)
                }
                G.refresh.call(this)
            };
            return k
        }(),
        da = function () {
            var k = function (g) {
                var h = e[g];
                g = a[e[g]];
                if (!g.element) {
                    g.element = document.createElement(_$_eb2b[382]);
                    g.element.innerHTML = g.value;
                    var l = document.createElement(_$_eb2b[227]);
                    l.type = _$_eb2b[262];
                    l.value = h;
                    l.o = g;
                    g.element.insertBefore(l, g.element.firstChild)
                }
                g.element.firstChild.checked = g.selected;
                return g.element
            },
                b = null,
                c = null,
                a = [],
                d = 0,
                e = null,
                f = function (g) {
                    g.setFilter = f[_$_eb2b[11]];
                    g.getFilter = f[_$_eb2b[51]];
                    g.openFilter = f.open;
                    g.closeFilter = f.close;
                    g.resetFilters = f.reset;
                    g.showFilter = f.show;
                    g.hideFilter = f.hide
                };
            f.isVisible = function () {
                return c
            };
            f[_$_eb2b[11]] = function (g, h) {
                var l = this.headers[g];
                g = this.options.columns[g];
                Array.isArray(h) && 0 < h.length ? (l && l.classList.add(_$_eb2b[383]), g.filters = h) : (l && l.classList.remove(_$_eb2b[383]), g.filters = null);
                f.update.call(this)
            };
            f[_$_eb2b[51]] = function (g) {
                if (g) return this.options.columns[g].filters || null;
                g = {};
                for (var h = 0; h < this.options.columns.length; h++) g[h] = this.options.columns[h].filters || null;
                return g
            };
            f.open = function (g) {
                b = parseInt(g);
                if (this.headers[b].classList.contains(_$_eb2b[169])) {
                    c = !0;
                    var h =
                        this.parent.element.getBoundingClientRect(),
                        l = this.headers[b].getBoundingClientRect();
                    g = l.left - h.left;
                    var m = l.top - h.top + l.height;
                    l = this.parent.filter;
                    h = h.left + h.width;
                    l.style.display = _$_eb2b[16];
                    g + l.offsetWidth > h && (g = h - (l.offsetWidth + 18));
                    l.style.top = m + _$_eb2b[44];
                    l.style.left = g + _$_eb2b[44];
                    l.children[0].focus();
                    g = l.children[1].selectAll;
                    l.children[1].textContent = _$_eb2b[3];
                    l.children[1].appendChild(g);
                    g.children[0].checked = this.options.columns[b].filters ? !1 : !0;
                    g = qa.execute.call(this, null, b);
                    g = E.invert(g);
                    l = [];
                    for (h = 0; h < this.rows.length; h++)
                        if (g[h]) {
                            m = _$_eb2b[3] + M.call(this, b, h);
                            var p = _$_eb2b[3] + ea.processed.call(this, b, h, !0);
                            m.substr(0, 1) == _$_eb2b[12] && (m = p);
                            l.push([m, p])
                        } l.sort(function (n, t) {
                            return n[0] > t[0] ? 1 : n[0] < t[0] ? -1 : 0
                        });
                    a = [];
                    for (h = 0; h < l.length; h++) a[l[h][0]] = {
                        value: l[h][1],
                        element: null
                    };
                    (g = F.call(this.parent, _$_eb2b[385], this, b, a)) && (a = g);
                    void 0 !== a[_$_eb2b[3]] && (delete a[_$_eb2b[3]], a[_$_eb2b[3]] = {
                        value: _$_eb2b[215] + B(_$_eb2b[386]) + _$_eb2b[240],
                        element: null
                    });
                    f.search.call(this, _$_eb2b[3])
                } else console.log(_$_eb2b[384])
            };
            f.close = function (g) {
                if (g) {
                    g = this.parent.filter.children[1];
                    if (1 == g.selectAll.firstChild.checked && e.length == Object.keys(a).length) g = null;
                    else {
                        g = 1 == g.currentSelection.firstChild.checked ? this.options.columns[b].filters || [] : [];
                        for (var h = 0; h < e.length; h++) a[e[h]].selected && g.push(e[h])
                    }
                    f[_$_eb2b[11]].call(this, b, g)
                }
                this.parent.filter.style.display = _$_eb2b[3];
                this.parent.filter.children[0].value = _$_eb2b[3];
                b = null;
                c = !1;
                a = null;
                d = 0;
                e = null
            };
            f.reset = function (g) {
                f.updateDOM.call(this, g, !1);
                va.reset.call(this);
                f.update.call(this)
            };
            f.execute = function (g) {
                for (var h = this.options.columns, l = [], m = 0; m < this.options.data.length; m++) {
                    for (var p = !0, n = 0; n < h.length; n++)
                        if (h[n].filters && g !== n) {
                            var t = _$_eb2b[3] + M.call(this, n, m),
                                v = _$_eb2b[3] + this.getLabelFromCoords(n, m, !0); - 1 == h[n].filters.indexOf(t) && -1 == h[n].filters.indexOf(v) && (p = !1)
                        } p && l.push(m)
                }
                return l
            };
            f.update = function () {
                va.reset.call(this);
                this.resetSelection();
                this.resetBorders();
                var g = qa.execute.call(this, null);
                if (typeof this.parent.config.onbeforefilter == _$_eb2b[85]) {
                    var h =
                        f[_$_eb2b[51]].call(this),
                        l = F.call(this.parent, _$_eb2b[387], this, h, g);
                    if (l) g = l;
                    else if (!1 === l) return !1
                }
                this.pageNumber = 0;
                g && g.length < this.rows.length && va[_$_eb2b[11]].call(this, g);
                G.resetY.call(this);
                aa.refresh.call(this);
                F.call(this.parent, _$_eb2b[388], this, h, g)
            };
            f.show = function (g) {
                f.updateDOM.call(this, g, !0)
            };
            f.hide = function (g) {
                f.updateDOM.call(this, g, !1, !0);
                f.update.call(this)
            };
            f.search = function (g) {
                for (var h = this.parent.filter.children[1]; h.children[1];) h.removeChild(h.lastChild);
                h.currentSelection.firstChild.checked = !1;
                if (g) {
                    e = [];
                    g = new RegExp(g, _$_eb2b[389]);
                    for (var l = Object.keys(a), m = 0; m < l.length; m++) a[l[m]] && (_$_eb2b[3] + a[l[m]].value).match(g) ? (e.push(l[m]), a[l[m]].selected = !0) : a[l[m]].selected = !1;
                    e.length && this.options.columns[b].filters && (h.firstChild.checked = !0, h.appendChild(h.currentSelection))
                } else {
                    e = Object.keys(a);
                    if (g = this.options.columns[b].filters)
                        for (m = 0; m < e.length; m++) a[e[m]].selected = !1, 0 <= g.indexOf(e[m]) && (a[e[m]].selected = !0);
                    else
                        for (m = 0; m < e.length; m++) a[e[m]].selected = !0;
                    h.firstChild.checked =
                        g ? !1 : !0
                }
                for (m = d = 0; m < e.length; m++) 200 > m && (g = d++, h.appendChild(k(g)))
            };
            f.build = function () {
                var g = document.createElement(_$_eb2b[20]);
                g.className = _$_eb2b[390];
                g.onclick = function (p) {
                    p.target.tagName == _$_eb2b[391] && p.target.o && (p.target.o.selected = p.target.checked);
                    p = !0;
                    for (var n = 1; n < this.children.length; n++) this.children[n].children[0].checked || (p = !1);
                    this.children[0].children[0].checked = p
                };
                var h = document.createElement(_$_eb2b[227]);
                h.type = _$_eb2b[55];
                h.placeholder = B(_$_eb2b[392]);
                h.className = _$_eb2b[393];
                var l = document.createElement(_$_eb2b[227]);
                l.type = _$_eb2b[394];
                l.value = _$_eb2b[395];
                l.className = _$_eb2b[396];
                var m = document.createElement(_$_eb2b[20]);
                m.className = _$_eb2b[397];
                m.appendChild(h);
                m.appendChild(g);
                m.appendChild(l);
                g.currentSelection = document.createElement(_$_eb2b[382]);
                g.currentSelection.innerHTML = _$_eb2b[398] + B(_$_eb2b[399]);
                g.selectAll = document.createElement(_$_eb2b[382]);
                g.selectAll.innerHTML = _$_eb2b[400] + B(_$_eb2b[401]) + _$_eb2b[240];
                g.selectAll.onclick = function () {
                    for (var p = this.children[0].checked,
                        n = 1; n < this.parentNode.children.length; n++) this.parentNode.children[n].children[0].checked = p;
                    if (1 == p)
                        for (n = 0; n < e.length; n++) a[e[n]].selected = !0;
                    else
                        for (p = Object.keys(a), n = 0; n < p.length; n++) a[p[n]].selected = !1
                };
                jSuites.lazyLoading(g, {
                    loadDown: function (p) {
                        for (p = 0; d < e.length - 1 && 10 > p;) {
                            var n = d++;
                            g.appendChild(k(n));
                            p++
                        }
                    }
                });
                return m
            };
            f.updateDOM = function (g, h, l) {
                var m = this.headers,
                    p = function (n) {
                        1 == h ? m[n].classList.add(_$_eb2b[169]) : (this.options.columns[n].filters = null, m[n].classList.remove(_$_eb2b[383]),
                            1 == l && m[n].classList.remove(_$_eb2b[169]))
                    };
                if (g) p.call(this, g);
                else
                    for (g = 0; g < m.length; g++) p.call(this, g)
            };
            f.onload = function () {
                var g, h = !1;
                if (g = this.options.columns)
                    for (var l = 0; l < g.length; l++) Array.isArray(g[l].filters) && 0 < g[l].filters.length && this.headers[l] && (this.headers[l].classList.add(_$_eb2b[383]), h = !0);
                1 == h && f.update.call(this)
            };
            return f
        }(),
        va = function () {
            var k = {};
            k[_$_eb2b[11]] = function (b) {
                for (var c = 0; c < b.length; c++) this.rows[b[c]].results = !0;
                this.results = b
            };
            k.reset = function () {
                for (var b = 0; b <
                    this.rows.length; b++) !0 === this.rows[b].results && delete this.rows[b].results;
                this.results = null
            };
            k.refresh = function () {
                for (var b = [], c = 0; c < this.rows.length; c++) !0 === this.rows[c].results && b.push(c);
                return b
            };
            return k
        }(),
        qa = function () {
            var k = function (a) {
                var d = document.createElement(_$_eb2b[20]);
                if (0 < a.options.pagination && a.options.paginationOptions && 0 < a.options.paginationOptions.length) {
                    a.paginationDropdown = document.createElement(_$_eb2b[402]);
                    a.paginationDropdown.classList.add(_$_eb2b[403]);
                    a.paginationDropdown.onchange =
                        function () {
                            a.options.pagination = parseInt(this.value);
                            a.page(0)
                        };
                    for (var e = 0; e < a.options.paginationOptions.length; e++) {
                        var f = document.createElement(_$_eb2b[404]);
                        f.value = a.options.paginationOptions[e];
                        f.innerHTML = a.options.paginationOptions[e];
                        a.paginationDropdown.appendChild(f)
                    }
                    a.paginationDropdown.value = a.options.pagination;
                    d.appendChild(document.createTextNode(B(_$_eb2b[405])));
                    d.appendChild(a.paginationDropdown);
                    d.appendChild(document.createTextNode(B(_$_eb2b[406])))
                }
                return d
            },
                b = function (a, d) {
                    var e =
                        this.merged.rows;
                    if (e[d]) {
                        for (; 0 < d && e[d - 1];) d--;
                        for (; e[d];) a.push(d++)
                    } else a.push(d)
                },
                c = function (a) {
                    a.search = c.update;
                    a.resetSearch = c.reset;
                    a.updateSearch = c.refresh;
                    a.showSearch = c.show;
                    a.hideSearch = c.hide
                };
            c.execute = function (a, d) {
                null === a ? a = this.searchInput.value || _$_eb2b[3] : a !== this.searchInput.value && (this.searchInput.value = a);
                this.pageNumber = 0;
                var e = [],
                    f = null;
                d = da.execute.call(this, d);
                if (a) try {
                    a = new RegExp(a, _$_eb2b[389]);
                    for (var g = 0; g < this.rows.length; g++) {
                        f = !1;
                        if (!d || 0 <= d.indexOf(g))
                            for (var h =
                                0; h < this.options.columns.length; h++) {
                                var l = _$_eb2b[3] + M.call(this, h, g),
                                    m = _$_eb2b[3] + this.getLabelFromCoords(h, g);
                                if (l.match(a) || m.match(a)) f = !0
                            }
                        1 == f && b.call(this, e, g)
                    }
                } catch (p) { } else e = d;
                if (e.length) {
                    for (g = 0; g < this.rows.length; g++) !0 === this.rows[g].results && e.push(g);
                    e.sort(function (p, n) {
                        return p - n
                    })
                }
                return e
            };
            c.update = function (a) {
                va.reset.call(this);
                this.resetSelection();
                this.resetBorders();
                var d = c.execute.call(this, a);
                if (typeof this.parent.config.onbeforesearch == _$_eb2b[85]) {
                    var e = F.call(this.parent,
                        _$_eb2b[408], this, a, d);
                    if (e) d = e;
                    else if (!1 === e) return !1
                }
                this.pageNumber = 0;
                d && d.length < this.rows.length && va[_$_eb2b[11]].call(this, d);
                G.resetY.call(this);
                aa.refresh.call(this);
                F.call(this.parent, _$_eb2b[409], this, a, d)
            };
            c.refresh = function () {
                G.resetY.call(this);
                aa.refresh.call(this)
            };
            c.reset = function () {
                this.searchInput.value = _$_eb2b[3];
                va.reset.call(this);
                G.resetY.call(this);
                aa.refresh.call(this)
            };
            c.show = function () {
                this.options.search = !0;
                this.searchContainer.style.display = _$_eb2b[3]
            };
            c.hide = function () {
                this.options.search = !1;
                this.searchContainer.style.display = _$_eb2b[40]
            };
            c.build = function () {
                this.searchContainer = document.createElement(_$_eb2b[20]);
                this.searchContainer.classList.add(_$_eb2b[410]);
                this.searchContainer.appendChild(k(this));
                var a = this.searchContainer,
                    d = a.appendChild,
                    e = document.createElement(_$_eb2b[20]);
                e.innerHTML = B(_$_eb2b[392]) + _$_eb2b[116];
                this.searchInput = document.createElement(_$_eb2b[227]);
                this.searchInput.classList.add(_$_eb2b[407]);
                e.appendChild(this.searchInput);
                d.call(a, e);
                0 == this.options.search &&
                    (this.searchContainer.style.display = _$_eb2b[40]);
                this.element.insertBefore(this.searchContainer, this.element.firstChild)
            };
            return c
        }(),
        Ra = function () {
            var k = function (b) {
                b.contextmenu = document.createElement(_$_eb2b[20]);
                b.contextmenu.className = _$_eb2b[411];
                jSuites.contextmenu(b.contextmenu, {
                    onclick: function (c, a, d) {
                        c.close()
                    }
                });
                return b.contextmenu
            };
            k.open = function (b, c) {
                var a = b.target.getAttribute(_$_eb2b[42]),
                    d = b.target.getAttribute(_$_eb2b[43]),
                    e = k[_$_eb2b[51]](this, a, d, b, c[0], c[1], c[2]);
                if (typeof this.parent.config.contextMenu ==
                    _$_eb2b[85]) {
                    var f = this.parent.config.contextMenu(this, a, d, b, e, c[0], c[1], c[2]);
                    if (f) e = f;
                    else if (!1 === f) return !1
                }
                b.preventDefault();
                f = pa.execute.call(this.parent, _$_eb2b[86], [this, a, d, b, e, c[0], c[1], c[2]]);
                e = f[4];
                this.parent.contextmenu.contextmenu.open(b, e)
            };
            k[_$_eb2b[51]] = function (b, c, a, d, e, f, g) {
                var h = [],
                    l = b.parent.config,
                    m = -1 != navigator.userAgent.indexOf(_$_eb2b[412]) ? _$_eb2b[413] : _$_eb2b[414];
                e == _$_eb2b[415] && (1 == l.allowRenameWorksheet && h.push({
                    title: B(_$_eb2b[416]),
                    onclick: function () {
                        var n = prompt(B(_$_eb2b[416]),
                            d.target.innerText);
                        n && b.parent.renameWorksheet(f, n)
                    }
                }), 1 == l.allowDeleteWorksheet && h.push({
                    title: B(_$_eb2b[417]),
                    onclick: function () {
                        confirm(B(_$_eb2b[418]), d.target.innerText) && b.parent.deleteWorksheet(f)
                    }
                }), h.push({
                    type: _$_eb2b[419]
                }));
                e == _$_eb2b[420] && (h.push({
                    title: B(_$_eb2b[421]),
                    onclick: function () {
                        var n = prompt(B(_$_eb2b[421]), d.target.innerText);
                        b.setNestedCell(f, g, {
                            title: n
                        })
                    }
                }), h.push({
                    type: _$_eb2b[419]
                }));
                if (e == _$_eb2b[422] || e == _$_eb2b[423] || e == _$_eb2b[424] || e == _$_eb2b[420]) h.push({
                    title: B(_$_eb2b[425]),
                    icon: _$_eb2b[426],
                    shortcut: m + _$_eb2b[427],
                    onclick: function () {
                        b.cut()
                    }
                }), h.push({
                    title: B(_$_eb2b[428]),
                    icon: _$_eb2b[429],
                    shortcut: m + _$_eb2b[430],
                    onclick: function () {
                        b.copy()
                    }
                }), navigator && navigator.clipboard && navigator.clipboard.readText && h.push({
                    title: B(_$_eb2b[431]),
                    icon: _$_eb2b[432],
                    shortcut: m + _$_eb2b[433],
                    onclick: function () {
                        b.selectedCell && navigator.clipboard.readText().then(function (n) {
                            n && b.paste(b.selectedCell[0], b.selectedCell[1], n)
                        })
                    }
                }), h.push({
                    type: _$_eb2b[419]
                });
                e == _$_eb2b[422] && (1 == b.options.allowInsertColumn &&
                    h.push({
                        title: B(_$_eb2b[434]),
                        onclick: function () {
                            b.insertColumn(1, parseInt(f), 1)
                        }
                    }), 1 == b.options.allowInsertColumn && h.push({
                        title: B(_$_eb2b[435]),
                        onclick: function () {
                            b.insertColumn(1, parseInt(f), 0)
                        }
                    }), 1 == b.options.allowDeleteColumn && h.push({
                        title: B(_$_eb2b[436]),
                        onclick: function () {
                            b.deleteColumn(b.getSelectedColumns().length ? void 0 : parseInt(c))
                        }
                    }), 1 == b.options.allowRenameColumn && h.push({
                        title: B(_$_eb2b[437]),
                        onclick: function () {
                            b.setHeader(f)
                        }
                    }), b.options.data.length || (h.push({
                        type: _$_eb2b[419]
                    }), h.push({
                        title: B(_$_eb2b[438]),
                        onclick: function () {
                            b.insertRow(0)
                        }
                    })), 1 == b.options.columnSorting && (h.push({
                        type: _$_eb2b[419]
                    }), h.push({
                        title: B(_$_eb2b[439]),
                        onclick: function () {
                            b.orderBy(f, 0)
                        }
                    }), h.push({
                        title: B(_$_eb2b[440]),
                        onclick: function () {
                            b.orderBy(f, 1)
                        }
                    }), h.push({
                        type: _$_eb2b[419]
                    })));
                if (e == _$_eb2b[424] || e == _$_eb2b[423]) 1 == b.options.allowInsertRow && (h.push({
                    title: B(_$_eb2b[441]),
                    onclick: function () {
                        b.insertRow(1, parseInt(a), 1)
                    }
                }), h.push({
                    title: B(_$_eb2b[442]),
                    onclick: function () {
                        b.insertRow(1, parseInt(a))
                    }
                })), 1 == b.options.allowDeleteRow &&
                    h.push({
                        title: B(_$_eb2b[443]),
                        onclick: function () {
                            b.deleteRow(b.getSelectedRows().length ? void 0 : parseInt(a))
                        }
                    }), h.push({
                        type: _$_eb2b[419]
                    });
                if (e == _$_eb2b[424] && 1 == b.options.allowComments) {
                    var p = b.records[g][f].element.getAttribute(_$_eb2b[277]) || _$_eb2b[3];
                    h.push({
                        title: p ? B(_$_eb2b[444]) : B(_$_eb2b[445]),
                        icon: _$_eb2b[446],
                        onclick: function () {
                            var n = E.getColumnNameFromCoords(f, g),
                                t = prompt(B(_$_eb2b[447]), p);
                            t && b.setComments(n, t)
                        }
                    });
                    p && h.push({
                        title: B(_$_eb2b[448]),
                        onclick: function () {
                            var n = E.getColumnNameFromCoords(f,
                                g);
                            b.setComments(n, _$_eb2b[3])
                        }
                    });
                    h.push({
                        type: _$_eb2b[419]
                    })
                }
                l.allowExport && h.push({
                    title: B(_$_eb2b[449]),
                    icon: _$_eb2b[450],
                    shortcut: m + _$_eb2b[451],
                    onclick: function () {
                        b.download()
                    }
                });
                l.about && h.push({
                    title: B(_$_eb2b[452]),
                    icon: _$_eb2b[453],
                    onclick: function () {
                        !0 === l.about ? alert(K().print()) : alert(l.about)
                    }
                });
                return h
            };
            return k
        }(),
        fa = function () {
            var k = function (b) {
                b.showToolbar = function () {
                    k.show.call(b.parent)
                };
                b.hideToolbar = function () {
                    k.hide.call(b.parent)
                };
                b.refreshToolbar = function () {
                    k.update.call(b.parent,
                        b)
                }
            };
            k[_$_eb2b[11]] = function (b) {
                this.toolbar.innerHTML = _$_eb2b[3];
                this.config.toolbar = k[_$_eb2b[51]].call(this, b);
                k.show.call(this)
            };
            k[_$_eb2b[51]] = function (b) {
                b || (b = this.config.toolbar);
                b ? Array.isArray(b) ? b = {
                    items: b
                } : typeof b === _$_eb2b[85] && (b = b({
                    items: k.getDefault()
                })) : b = {};
                typeof b !== _$_eb2b[84] && (b = {});
                b.items || (b.items = k.getDefault());
                typeof b.responsive == _$_eb2b[13] && (b.responsive = !0, b.bottom = !1, b.maxWidth = this.element.offsetWidth);
                return b
            };
            k.show = function () {
                this.toolbar.innerHTML || k.build.call(this);
                this.toolbar.style.display = _$_eb2b[3];
                this.element.classList.add(_$_eb2b[454])
            };
            k.hide = function () {
                this.toolbar.style.display = _$_eb2b[40];
                this.element.classList.remove(_$_eb2b[454])
            };
            k.update = function (b, c) {
                if (c = this.toolbarInstance)
                    if (c.update(b), c.options.responsive) {
                        var a = parseInt(b.content.style.maxWidth);
                        if (!a || this.config.fullscreen) a = b.element.offsetWidth;
                        c.options.maxWidth = a;
                        c.refresh()
                    }
            };
            k.build = function () {
                for (var b = k[_$_eb2b[51]].call(this), c = 0; c < b.items.length; c++)
                    if (b.items[c].type == _$_eb2b[402]) {
                        if (!b.items[c].options &&
                            b.items[c].v) {
                            b.items[c].options = [];
                            for (var a = 0; a < b.items[c].v.length; a++) b.items[c].options.push(b.items[c].v[a]);
                            b.items[c].k && (b.items[c].onchange = function (d, e, f, g) {
                                r.current.setStyle(r.current.getSelected(!1, !0), f.k, g)
                            })
                        }
                    } else b.items[c].type == _$_eb2b[267] ? (b.items[c].type = _$_eb2b[455], b.items[c].onclick = function (d, e, f) {
                        if (!f.color) {
                            var g = null;
                            jSuites.color(f, {
                                onchange: function (h, l) {
                                    (g = r.current.getSelected(!1, !0)) && r.current.setStyle(g, f.k, l)
                                },
                                onopen: function (h) {
                                    var l;
                                    r.current.cursor ? (l = r.current.cursor.style[f.k]) ?
                                        h.color.select(h.color.toHex(l)) : h.color.select(_$_eb2b[3]) : h.color.select(_$_eb2b[3])
                                }
                            });
                            f.color.open()
                        }
                    }) : !b.items[c].onclick && b.items[c].k && (b.items[c].onclick = function () {
                        r.current.setStyle(r.current.getSelected(!1, !0), this.k, this.v)
                    });
                (c = pa.execute.call(this, _$_eb2b[87], [b])) && (b = c[0]);
                this.toolbarInstance = jSuites.toolbar(this.toolbar, b);
                this.toolbarInstance.application = this;
                b = this.element.tabs.getActive();
                this.toolbarInstance.update(this.worksheets[b])
            };
            k.getDefault = function () {
                return [{
                    content: _$_eb2b[456],
                    onclick: function () {
                        r.current && r.current.undo()
                    }
                }, {
                    content: _$_eb2b[457],
                    onclick: function () {
                        r.current && r.current.redo()
                    }
                }, {
                    content: _$_eb2b[450],
                    onclick: function () {
                        r.current && r.current.download()
                    }
                }, {
                    type: _$_eb2b[458]
                }, {
                    type: _$_eb2b[402],
                    width: _$_eb2b[459],
                    options: [_$_eb2b[460], _$_eb2b[461], _$_eb2b[462]],
                    render: function (b) {
                        return _$_eb2b[463] + b + _$_eb2b[234] + b + _$_eb2b[464]
                    },
                    onchange: function (b, c, a, d) {
                        r.current && r.current.setStyle(r.current.getSelected(!1, !0), _$_eb2b[465], d)
                    }
                }, {
                    type: _$_eb2b[402],
                    width: _$_eb2b[466],
                    content: _$_eb2b[467],
                    options: [_$_eb2b[468], _$_eb2b[469], _$_eb2b[470], _$_eb2b[471], _$_eb2b[472]],
                    render: function (b) {
                        return _$_eb2b[473] + b + _$_eb2b[234] + b + _$_eb2b[464]
                    },
                    onchange: function (b, c, a, d) {
                        r.current && r.current.setStyle(r.current.getSelected(!1, !0), _$_eb2b[474], d)
                    }
                }, {
                    type: _$_eb2b[402],
                    options: [_$_eb2b[475], _$_eb2b[476], _$_eb2b[477], _$_eb2b[478]],
                    render: function (b) {
                        return _$_eb2b[479] + b + _$_eb2b[480]
                    },
                    onchange: function (b, c, a, d) {
                        r.current && r.current.setStyle(r.current.getSelected(!1, !0), _$_eb2b[481],
                            d.split(_$_eb2b[482])[2])
                    }
                }, {
                    type: _$_eb2b[455],
                    content: _$_eb2b[483],
                    k: _$_eb2b[484],
                    v: _$_eb2b[485]
                }, {
                    type: _$_eb2b[267],
                    content: _$_eb2b[486],
                    k: _$_eb2b[267]
                }, {
                    type: _$_eb2b[267],
                    content: _$_eb2b[487],
                    k: _$_eb2b[488]
                }, {
                    type: _$_eb2b[402],
                    data: [_$_eb2b[489], _$_eb2b[490], _$_eb2b[491], _$_eb2b[492], _$_eb2b[493], _$_eb2b[494], _$_eb2b[495], _$_eb2b[496], _$_eb2b[497], _$_eb2b[498]],
                    columns: 5,
                    render: function (b) {
                        return _$_eb2b[479] + b + _$_eb2b[480]
                    },
                    right: !0,
                    onchange: function (b, c, a, d, e) {
                        if (b = r.current.getHighlighted()) {
                            a =
                                c.thickness || 1;
                            c = c.color || _$_eb2b[499];
                            e = {};
                            for (var f = b[0], g = b[1], h = b[2], l = b[3], m = b[1]; m <= b[3]; m++)
                                for (var p = b[0]; p <= b[2]; p++) {
                                    var n = E.getColumnNameFromCoords(p, m);
                                    e[n] || (e[n] = _$_eb2b[3]);
                                    e[n] = d != _$_eb2b[494] && d != _$_eb2b[490] || p != f ? (d == _$_eb2b[491] || d == _$_eb2b[493]) && p > f ? e[n] + (_$_eb2b[500] + a + _$_eb2b[501] + c + _$_eb2b[308]) : d == _$_eb2b[489] ? e[n] + (_$_eb2b[500] + a + _$_eb2b[501] + c + _$_eb2b[308]) : e[n] + _$_eb2b[502] : e[n] + (_$_eb2b[500] + a + _$_eb2b[501] + c + _$_eb2b[308]);
                                    e[n] = d != _$_eb2b[489] && d != _$_eb2b[496] && d != _$_eb2b[490] ||
                                        p != h ? e[n] + _$_eb2b[504] : e[n] + (_$_eb2b[503] + a + _$_eb2b[501] + c + _$_eb2b[308]);
                                    e[n] = d != _$_eb2b[495] && d != _$_eb2b[490] || m != g ? (d == _$_eb2b[491] || d == _$_eb2b[492]) && m > g ? e[n] + (_$_eb2b[505] + a + _$_eb2b[501] + c + _$_eb2b[308]) : d == _$_eb2b[489] ? e[n] + (_$_eb2b[505] + a + _$_eb2b[501] + c + _$_eb2b[308]) : e[n] + _$_eb2b[506] : e[n] + (_$_eb2b[505] + a + _$_eb2b[501] + c + _$_eb2b[308]);
                                    e[n] = d != _$_eb2b[489] && d != _$_eb2b[497] && d != _$_eb2b[490] || m != l ? e[n] + _$_eb2b[508] : e[n] + (_$_eb2b[507] + a + _$_eb2b[501] + c + _$_eb2b[308])
                                }
                            Object.keys(e) && r.current.setStyle(e,
                                null, null, !0)
                        }
                    },
                    onload: function (b, c) {
                        var a = document.createElement(_$_eb2b[20]),
                            d = document.createElement(_$_eb2b[20]);
                        a.appendChild(d);
                        var e = jSuites.color(d, {
                            closeOnChange: !1,
                            onchange: function (f, g) {
                                f.parentNode.children[1].style.color = g;
                                c.color = g
                            }
                        });
                        d = document.createElement(_$_eb2b[455]);
                        d.classList.add(_$_eb2b[509]);
                        d.innerHTML = _$_eb2b[510];
                        d.onclick = function () {
                            e.open()
                        };
                        a.appendChild(d);
                        b.children[1].appendChild(a);
                        d = document.createElement(_$_eb2b[20]);
                        jSuites.picker(d, {
                            type: _$_eb2b[402],
                            data: [1,
                                2, 3, 4, 5
                            ],
                            render: function (f) {
                                return _$_eb2b[511] + f + _$_eb2b[512]
                            },
                            onchange: function (f, g, h, l) {
                                c.thickness = l
                            },
                            width: _$_eb2b[513]
                        });
                        b.children[1].appendChild(d);
                        d = document.createElement(_$_eb2b[20]);
                        d.style.flex = _$_eb2b[514];
                        b.children[1].appendChild(d)
                    }
                }, {
                    type: _$_eb2b[458]
                }, {
                    content: _$_eb2b[15],
                    onclick: function (b, c, a) {
                        a.children[0].innerText == _$_eb2b[15] ? (r.current.fullscreen(!0), a.children[0].innerText = _$_eb2b[515]) : (r.current.fullscreen(!1), a.children[0].innerText = _$_eb2b[15])
                    },
                    tooltip: _$_eb2b[516],
                    updateState: function (b, c, a, d) {
                        a.children[0].innerText = 1 == d.parent.config.fullscreen ? _$_eb2b[515] : _$_eb2b[15]
                    }
                }, {
                    content: _$_eb2b[517],
                    onclick: function (b, c, a) {
                        a.children[0].innerText == _$_eb2b[517] ? (qa.show.call(r.current), a.children[0].innerText = _$_eb2b[518]) : (qa.hide.call(r.current), a.children[0].innerText = _$_eb2b[517])
                    },
                    tooltip: _$_eb2b[519],
                    updateState: function (b, c, a, d) {
                        a.children[0].innerText = 1 == d.options.search ? _$_eb2b[518] : _$_eb2b[517]
                    }
                }]
            };
            return k
        }(),
        ha = function () {
            var k = function (b) {
                b.createWorksheet =
                    function (c) {
                        return b.parent.createWorksheet(c)
                    };
                b.deleteWorksheet = function (c) {
                    return b.parent.deleteWorksheet(c)
                };
                b.renameWorksheet = function (c, a) {
                    return b.parent.renameWorksheet(c, a)
                };
                b.moveWorksheet = function (c, a) {
                    return b.parent.moveWorksheet(c, a)
                };
                b.openWorksheet = function () {
                    return b.parent.openWorksheet(b.parent.getWorksheet(b))
                };
                b.getWorksheet = function (c) {
                    return b.parent.getWorksheet(c)
                };
                b.getWorksheetActive = function () {
                    return b.parent.getWorksheetActive()
                };
                b.getWorksheetName = function () {
                    return b.options.worksheetName.toUpperCase()
                }
            };
            k.nextName = function () {
                var b = 1,
                    c, a = r.spreadsheet;
                if (a.length)
                    for (var d = 0; d < a.length; d++) {
                        var e = a[d].worksheets;
                        if (e.length)
                            for (var f = 0; f < e.length; f++) e[f].options.worksheetName && (c = e[f].options.worksheetName.match(/(\d+)/)) && (c = parseInt(c[0]) + 1, c > b && (b = c))
                    }
                return b
            };
            k.createWorksheet = function (b) {
                if (!Q.call(this)) return !1;
                b || (b = {});
                var c = this.worksheets.length,
                    a = F.call(this, _$_eb2b[520], b, c);
                if (typeof a === _$_eb2b[84]) b = a;
                else if (!1 === a) return !1;
                b.data || b.minDimensions || (b.minDimensions = [8, 8]);
                b.worksheetId ||
                    (b.worksheetId = jSuites.guid().substring(0, 8));
                b.worksheetName || (b.worksheetName = _$_eb2b[370] + k.nextName());
                if (I(b.worksheetName)) jSuites.notification({
                    error: _$_eb2b[514],
                    message: _$_eb2b[521]
                });
                else return a = na.worksheet(this, b), a.onload(), r.current && r.current.resetSelection(), r.current = a, J.call(a, _$_eb2b[522], [b]), F.call(this, _$_eb2b[523], a, b, c), a
            };
            k.deleteWorksheet = function (b) {
                var c = this.worksheets[b];
                if (!Q.call(this, c)) return !1;
                I(c.options.worksheetName, null);
                this.element.tabs.deleteElement(b);
                this.worksheets.splice(b, 1);
                J.call(c, _$_eb2b[524], [b]);
                F.call(this, _$_eb2b[525], c, b)
            };
            k.renameWorksheet = function (b, c) {
                var a = this.worksheets[b];
                if (!Q.call(this, a)) return !1;
                var d = a.options.worksheetName;
                if (d.toLowerCase() == c.toLowerCase()) return !1;
                c = c.replace(/[^0-9A-Za-z_\s^]+/gi, _$_eb2b[3]);
                Number(c) == c && (c = _$_eb2b[370] + Number(c));
                I(c) ? alert(_$_eb2b[526]) : (I(d, null), this.element.tabs.rename(b, c), a.options.worksheetName = c, I(c, a), T.call(this, {
                    worksheet: a,
                    action: _$_eb2b[80],
                    index: b,
                    oldValue: d,
                    newValue: c
                }),
                    J.call(a, _$_eb2b[80], {
                        worksheet: b,
                        newValue: c
                    }), Ia.updateWorksheetName.call(a, d, c), W.updateWorksheetNames.call(a, d, c), F.call(this, _$_eb2b[527], a, b, c, d))
            };
            k.updateWorksheet = function (b, c) {
                var a = this.worksheets;
                if (!Q.call(this)) return !1;
                this.worksheets.splice(c, 0, a.splice(b, 1)[0]);
                T.call(this, {
                    worksheet: a[c],
                    action: _$_eb2b[81],
                    f: b,
                    t: c
                });
                J.call(a[c], _$_eb2b[81], {
                    f: b,
                    t: c
                });
                F.call(this, _$_eb2b[528], a[c], b, c)
            };
            k.moveWorksheet = function (b, c) {
                this.element.tabs.move(b, c)
            };
            k.openWorksheet = function (b) {
                var c = this.worksheets[b];
                !c || r.current && r.current == c || (this.element.tabs.getActive() != b ? this.element.tabs.open(b) : this.element.tabs.setBorder(b), r.current && r.current.resetSelection(), r.current = c, r.current.refreshBorders(), U.call(r.current), k.state.call(this, b), fa.update.call(this, r.current), F.call(this, _$_eb2b[529], c, b))
            };
            k.getWorksheet = function (b) {
                if (b && typeof b == _$_eb2b[84]) {
                    if (b = this.worksheets.indexOf(b), 0 <= b) return b
                } else
                    for (var c = 0; c < this.worksheets.length; c++)
                        if (b === this.worksheets[c].options.worksheetId) return c;
                return !1
            };
            k.getWorksheetActive = function () {
                return this.element.tabs.getActive()
            };
            k.getWorksheetInstance = function (b) {
                return b === _$_eb2b[3] || typeof b == _$_eb2b[17] ? (b = k.getWorksheet.call(this, b), !1 === b ? !1 : this.worksheets[b]) : this.worksheets[b]
            };
            k.state = function (b) {
                try {
                    if (typeof localStorage !== _$_eb2b[13]) {
                        var c = this.config.cloud ? this.config.cloud : this.element.id;
                        if (c)
                            if (typeof b !== _$_eb2b[13]) localStorage.setItem(_$_eb2b[530] + c, b);
                            else {
                                var a = parseInt(localStorage.getItem(_$_eb2b[530] + c));
                                k.openWorksheet.call(this,
                                    a)
                            }
                    }
                } catch (d) { }
            };
            return k
        }(),
        E = function () {
            var k = {
                getCaretIndex: function (b) {
                    var c = 0,
                        a = (this.config.root ? this.config.root : window).getSelection();
                    a && 0 !== a.rangeCount && (c = a.getRangeAt(0), a = c.cloneRange(), a.selectNodeContents(b), a.setEnd(c.endContainer, c.endOffset), c = a.toString().length);
                    return c
                },
                invert: function (b) {
                    for (var c = [], a = Object.keys(b), d = 0; d < a.length; d++) c[b[a[d]]] = a[d];
                    return c
                },
                getColumnName: function (b) {
                    var c = _$_eb2b[3];
                    701 < b ? (c += String.fromCharCode(64 + parseInt(b / 676)), c += String.fromCharCode(64 +
                        parseInt(b % 676 / 26))) : 25 < b && (c += String.fromCharCode(64 + parseInt(b / 26)));
                    return c += String.fromCharCode(65 + b % 26)
                },
                getColumnNameFromCoords: function (b, c) {
                    return k.getColumnName(parseInt(b)) + (parseInt(c) + 1)
                },
                getCoordsFromColumnName: function (b) {
                    var c = /^[a-zA-Z]+/.exec(b);
                    if (c) {
                        for (var a = 0, d = 0; d < c[0].length; d++) a += parseInt(c[0].charCodeAt(d) - 64) * Math.pow(26, c[0].length - 1 - d);
                        a--;
                        0 > a && (a = 0);
                        b = parseInt(/[0-9]+$/.exec(b)) || null;
                        0 < b && b--;
                        return [a, b]
                    }
                }
            };
            k.shiftFormula = W.shiftFormula;
            k.createFromTable = function (b,
                c) {
                if (b.tagName != _$_eb2b[531]) console.log(_$_eb2b[532]);
                else {
                    c || (c = {});
                    c.columns = [];
                    c.data = [];
                    var a = b.querySelectorAll(_$_eb2b[533]);
                    if (a.length)
                        for (var d = 0; d < a.length; d++) {
                            var e = a[d].style.width;
                            e || (e = a[d].getAttribute(_$_eb2b[153]));
                            e && (c.columns[d] || (c.columns[d] = {}), c.columns[d].width = e)
                        }
                    e = function (z) {
                        var D = z.getBoundingClientRect();
                        D = 50 < D.width ? D.width : 50;
                        c.columns[d] || (c.columns[d] = {});
                        z.getAttribute(_$_eb2b[534]) ? c.columns[d].type = z.getAttribute(_$_eb2b[534]) : c.columns[d].type = _$_eb2b[55];
                        c.columns[d].width = D + _$_eb2b[44];
                        c.columns[d].title = z.innerHTML;
                        z.style.textAlign && (c.columns[d].align = z.style.textAlign);
                        z.classList.contains(_$_eb2b[535]) && (c.columns[d].readOnly = !0);
                        if (D = z.getAttribute(_$_eb2b[536])) c.columns[d].name = D;
                        if (D = z.getAttribute(_$_eb2b[537])) c.columns[d].id = D
                    };
                    var f = [],
                        g = b.querySelectorAll(_$_eb2b[538]);
                    if (g.length) {
                        for (a = 0; a < g.length - 1; a++) {
                            var h = [];
                            for (d = 0; d < g[a].children.length; d++) {
                                var l = {
                                    title: g[a].children[d].innerText,
                                    colspan: g[a].children[d].getAttribute(_$_eb2b[52]) ||
                                        1
                                };
                                h.push(l)
                            }
                            f.push(h)
                        }
                        g = g[g.length - 1].children;
                        for (d = 0; d < g.length; d++) e(g[d])
                    }
                    l = 0;
                    var m = {},
                        p = {},
                        n = {},
                        t = {},
                        v = null;
                    h = b.querySelectorAll(_$_eb2b[539]);
                    for (a = 0; a < h.length; a++)
                        if (c.data[l] = [], 1 != c.parseTableFirstRowAsHeader || g.length || 0 != a) {
                            for (d = 0; d < h[a].children.length; d++) {
                                var u = h[a].children[d].getAttribute(_$_eb2b[540]);
                                u ? u.substr(0, 1) != _$_eb2b[12] && (u = _$_eb2b[12] + u) : u = h[a].children[d].innerHTML;
                                c.data[l].push(u);
                                u = E.getColumnNameFromCoords(d, a);
                                (v = h[a].children[d].getAttribute(_$_eb2b[541])) && (t[u] =
                                    v);
                                v = parseInt(h[a].children[d].getAttribute(_$_eb2b[52])) || 0;
                                var y = parseInt(h[a].children[d].getAttribute(_$_eb2b[165])) || 0;
                                if (v || y) m[u] = [v || 1, y || 1];
                                if (v = h[a].children[d].style && h[a].children[d].style.display == _$_eb2b[40]) h[a].children[d].style.display = _$_eb2b[3];
                                (v = h[a].children[d].getAttribute(_$_eb2b[100])) && (n[u] = v);
                                h[a].children[d].classList.contains(_$_eb2b[542]) && (n[u] = n[u] ? n[u] + _$_eb2b[543] : _$_eb2b[544])
                            }
                            h[a].style && h[a].style.height && (p[a] = {
                                height: h[a].style.height
                            });
                            l++
                        } else
                            for (d = 0; d < h[a].children.length; d++) e(h[a].children[d]);
                    0 < Object.keys(f).length && (c.nestedHeaders = f);
                    0 < Object.keys(n).length && (c.style = n);
                    0 < Object.keys(m).length && (c.mergeCells = m);
                    0 < Object.keys(p).length && (c.rows = p);
                    0 < Object.keys(t).length && (c.classes = t);
                    h = b.querySelectorAll(_$_eb2b[545]);
                    if (h.length) {
                        b = [];
                        for (a = 0; a < h.length; a++) {
                            e = [];
                            for (d = 0; d < h[a].children.length; d++) e.push(h[a].children[d].innerText);
                            b.push(e)
                        }
                        0 < Object.keys(b).length && (c.footers = b)
                    }
                    if (1 == c.parseTableAutoCellType)
                        for (b = [], d = 0; d < c.columns.length; d++) {
                            f = e = !0;
                            b[d] = [];
                            for (a = 0; a < c.data.length; a++)
                                if (u =
                                    c.data[a][d], b[d][u] || (b[d][u] = 0), b[d][u]++, 25 < u.length && (e = !1), 10 == u.length) {
                                    if (u.substr(4, 1) != _$_eb2b[188] || u.substr(7, 1) != _$_eb2b[188]) f = !1
                                } else f = !1;
                            a = Object.keys(b[d]).length;
                            f ? c.columns[d].type = _$_eb2b[187] : 1 == e && 1 < a && a <= parseInt(.1 * c.data.length) && (c.columns[d].type = _$_eb2b[57], c.columns[d].source = Object.keys(b[d]))
                        }
                    return c
                }
            };
            k.injectArray = function (b, c, a) {
                return b.slice(0, c).concat(a).concat(b.slice(c))
            };
            k.parseCSV = function (b, c) {
                c = c || _$_eb2b[25];
                for (var a = 0, d = 0, e = [
                    []
                ], f = 0, g = null, h = !1, l = !1,
                    m = 0; m < b.length; m++)
                    if (e[d] || (e[d] = []), e[d][a] || (e[d][a] = _$_eb2b[3]), b[m] != _$_eb2b[546])
                        if (b[m] != _$_eb2b[547] && b[m] != c || 0 != h && 1 != l && g) {
                            b[m] == _$_eb2b[190] && (h = !h);
                            if (null === g) {
                                if (g = h, 1 == g) continue
                            } else if (!0 === g && !l && b[m] == _$_eb2b[190]) {
                                b[m + 1] == _$_eb2b[190] ? (h = !0, e[d][a] += b[m], m++) : l = !0;
                                continue
                            }
                            e[d][a] += b[m]
                        } else {
                            g = null;
                            l = h = !1;
                            if (e[d][a][0] == _$_eb2b[190]) {
                                var p = e[d][a].trim();
                                p[p.length - 1] == _$_eb2b[190] && (e[d][a] = p.substr(1, p.length - 2))
                            }
                            b[m] == _$_eb2b[547] ? (a = 0, d++) : (a++, a > f && (f = a))
                        } for (b = 0; b < e.length; b++)
                    for (m =
                        0; m <= f; m++) void 0 === e[b][m] && (e[b][m] = _$_eb2b[3]);
                return e
            };
            k.getTokensFromRange = W.getTokensFromRange;
            k.getRangeFromTokens = W.getRangeFromTokens;
            return k
        }(),
        Ba = function () {
            function k(q) {
                return r.current && r.current.options.tableOverflow && q.target.tagName == _$_eb2b[572] && q.target.hasAttribute(_$_eb2b[42])
            }

            function b() {
                A.x1 = A.x2;
                A.y1 = A.y2;
                V = null
            }

            function c(q, x) {
                x || (x = 1);
                x *= .1;
                return 0 > q ? Math.min(0, Math.ceil(q / (1 + x)) + 1) : Math.max(0, Math.ceil(q / (1 + x)) - 1)
            }

            function a() {
                0 < Math.abs(A.speeds.vertical) && (A.speeds.vertical =
                    c(A.speeds.vertical, 1), r.current.scrollY.scrollTop += d(A.speeds.vertical), 0 == r.current.scrollY.scrollTop || r.current.scrollY.scrollHeight - r.current.scrollY.scrollTop === r.current.scrollY.clientHeight) && (A.speeds.vertical = 0);
                0 < Math.abs(A.speeds.horizontal) && (A.speeds.horizontal = c(A.speeds.horizontal, 1), r.current.scrollX.scrollLeft += d(A.speeds.horizontal), 0 == r.current.scrollX.scrollLeft || r.current.scrollX.scrollWidth - r.current.scrollX.scrollLeft === r.current.scrollX.clientWidth) && (A.speeds.horizontal = 0);
                0 >= Math.abs(A.speeds.vertical) && 0 >= Math.abs(A.speeds.horizontal) || requestAnimationFrame(a)
            }

            function d(q, x) {
                x || (x = 100);
                return Math.ceil(x / 1E3 * q)
            }
            var e = {},
                f = null,
                g = null,
                h = null,
                l = null,
                m = !1,
                p = null,
                n = null,
                t = null,
                v = function (q) {
                    function x(ia) {
                        ia.className && (ia.classList.contains(_$_eb2b[360]) && (H = ia), ia.classList.contains(_$_eb2b[373]) && (L = ia), ia.classList.contains(_$_eb2b[548]) && (O = ia));
                        ia.tagName == _$_eb2b[549] ? w = 1 : ia.tagName == _$_eb2b[550] ? w = 2 : ia.tagName == _$_eb2b[551] && (w = 3);
                        ia.parentNode && !H && x(ia.parentNode)
                    }
                    var w = null,
                        C = null,
                        H = null,
                        L = null,
                        O = null,
                        R = null,
                        xa = null,
                        Sa = null;
                    x(q.target);
                    if (null !== O) return !1;
                    if (H)
                        if (q.target.classList.contains(_$_eb2b[343])) C = _$_eb2b[552];
                        else if (q.target.classList.contains(_$_eb2b[351])) C = _$_eb2b[139];
                        else if (q.target.classList.contains(_$_eb2b[281])) C = _$_eb2b[423], R = parseInt(q.target.getAttribute(_$_eb2b[43]));
                        else if (q.target.parentNode && q.target.parentNode.classList && q.target.parentNode.classList.contains(_$_eb2b[179])) R = q.target.getAttribute(_$_eb2b[184]), xa = q.target.getAttribute(_$_eb2b[185]),
                            null === R ? C = _$_eb2b[552] : (C = _$_eb2b[420], R = parseInt(R), xa = parseInt(xa));
                        else if (q.target.parentNode && q.target.parentNode.classList && q.target.parentNode.classList.contains(_$_eb2b[147])) C = _$_eb2b[553];
                        else if (q.target.classList.contains(_$_eb2b[151])) C = _$_eb2b[553];
                        else if (q.target.parentNode && q.target.parentNode.classList && q.target.parentNode.classList.contains(_$_eb2b[554])) C = _$_eb2b[415], R = Array.prototype.indexOf.call(q.target.parentNode.children, q.target);
                        else if (1 == w) C = _$_eb2b[422], R = q.target.clientWidth -
                            q.offsetX, q.target.classList.contains(_$_eb2b[169]) && 3 < R && 20 > R && (C = _$_eb2b[555]), R = q.target.getAttribute(_$_eb2b[42]);
                        else if (2 == w) {
                            if (q = u(q, H)) C = _$_eb2b[424], R = q[0], xa = q[1], Sa = q[2]
                        } else if (3 == w) {
                            if (R = q.target.getAttribute(_$_eb2b[42]), xa = q.target.getAttribute(_$_eb2b[43]), R || xa) C = _$_eb2b[556]
                        } else w || jSuites.findElement(q.target, _$_eb2b[557]) && (C = _$_eb2b[87]);
                    return [H, L, w, C, R, xa, Sa]
                },
                u = function (q, x) {
                    if (q.changedTouches && q.changedTouches[0]) {
                        var w = q.changedTouches[0].clientX;
                        q = q.changedTouches[0].clientY
                    } else w =
                        q.clientX, q = q.clientY;
                    x = x.spreadsheet.config.root ? x.spreadsheet.config.root : document;
                    if (x.elementsFromPoint) var C = x.elementsFromPoint(w, q);
                    else x.msElementsFromPoint && (C = x.msElementsFromPoint(w, q));
                    for (x = 0; x < C.length; x++)
                        if (w = C[x].getAttribute(_$_eb2b[42]), q = C[x].getAttribute(_$_eb2b[43]), null != w && null != q) return w = parseInt(w), q = parseInt(q), [w, q, C[x]];
                    return !1
                },
                y = function (q) {
                    var x = v(q);
                    if (!1 === x) return !1;
                    x[0] ? x[1] ? r.current && document.activeElement.classList.contains(_$_eb2b[94]) ? document.activeElement.worksheet ||
                        (document.activeElement.worksheet = x[1].jexcel) : r.current != x[1].jexcel && (r.current && r.current.resetSelection(), r.current = x[1].jexcel) : (q = x[0].tabs.getActive(), 0 <= q && (r.current = x[0].spreadsheet.worksheets[q])) : r.current && !jSuites.findElement(q.target, _$_eb2b[228]) && (r.current.resetSelection(!0), r.current = null);
                    return x
                };
            e.input = function (q) {
                if (q.target.tagName == _$_eb2b[558] && q.target.classList.contains(_$_eb2b[94])) {
                    var x = null;
                    q.target.worksheet ? x = q.target.worksheet : r.current && (x = r.current);
                    x && la.parse.call(x,
                        q.target)
                }
            };
            e.keydown = function (q) {
                if (r.current) {
                    q.target.tagName == _$_eb2b[558] && q.target.classList.contains(_$_eb2b[94]) && la.onkeydown.call(r.current, q);
                    if (27 == q.which) {
                        if (r.current.edition) return r.current.closeEditor(r.current.edition, !1), q.preventDefault(), !1;
                        if (ra.event) return ra.cancel.call(r.current, q), q.preventDefault(), !1;
                        if (ma.event) return ma.cancel.call(r.current, q), !1;
                        if (1 == da.isVisible()) return da.close.call(r.current, !1), q.preventDefault(), !1
                    }
                    if (!q.target.classList.contains(_$_eb2b[228]))
                        if (q.target.classList.contains(_$_eb2b[393])) {
                            var x =
                                q.target;
                            p && clearTimeout(p);
                            p = setTimeout(function () {
                                da.search.call(r.current, x.value);
                                p = null
                            }, 500)
                        } else if (q.target.classList.contains(_$_eb2b[407])) x = q.target, p && clearTimeout(p), p = setTimeout(function () {
                            r.current.search(x.value);
                            p = null
                        }, 400);
                        else if (r.current.edition) {
                            var w = r.current.edition.getAttribute(_$_eb2b[42]);
                            13 == q.which ? r.current.options.columns[w].type == _$_eb2b[187] ? r.current.closeEditor(r.current.edition, !0) : r.current.options.columns[w].type != _$_eb2b[57] && r.current.options.columns[w].type !=
                                _$_eb2b[559] && r.current.options.columns[w].type != _$_eb2b[446] && (q.altKey ? r.current.parent.input.classList.contains(_$_eb2b[560]) || document.execCommand(_$_eb2b[561], !1, _$_eb2b[562]) : (r.current.closeEditor(r.current.edition, !0), r.current.parent.config.moveDownOnEnter && r.current.down(), q.preventDefault())) : 9 == q.which ? (r.current.closeEditor(r.current.edition, !0), r.current.right(), q.preventDefault()) : 39 != q.which && 37 != q.which && 38 != q.which && 40 != q.which || r.current.parent.input.selected || (r.current.closeEditor(r.current.edition,
                                    !0), 37 == q.which ? r.current.left(q.shiftKey, q.ctrlKey) : 39 == q.which ? r.current.right(q.shiftKey, q.ctrlKey) : 38 == q.which ? r.current.up(q.shiftKey, q.ctrlKey) : 40 == q.which && r.current.down(q.shiftKey, q.ctrlKey), q.preventDefault())
                        } else if (r.current.selectedCell)
                            if (33 == q.which) 0 < r.current.options.pagination ? ua.pageUp.call(r.current) : G.pageUp.call(r.current);
                            else if (34 == q.which) 0 < r.current.options.pagination ? ua.pageDown.call(r.current) : G.pageDown.call(r.current);
                            else if (37 == q.which) r.current.left(q.shiftKey, q.ctrlKey),
                                q.preventDefault();
                            else if (39 == q.which) r.current.right(q.shiftKey, q.ctrlKey), q.preventDefault();
                            else if (38 == q.which) r.current.up(q.shiftKey, q.ctrlKey), q.preventDefault();
                            else if (40 == q.which) r.current.down(q.shiftKey, q.ctrlKey), q.preventDefault();
                            else if (36 == q.which) r.current.first(q.shiftKey, q.ctrlKey), q.preventDefault();
                            else if (35 == q.which) r.current.last(q.shiftKey, q.ctrlKey), q.preventDefault();
                            else if (32 == q.which) r.current.setCheckRadioValue(), q.preventDefault();
                            else if (46 == q.which || 8 == q.which) r.current.isEditable() &&
                                r.current.setValue(r.current.getSelected(!1, !0), _$_eb2b[3]);
                            else if (13 == q.which) q.shiftKey ? r.current.up() : (1 == r.current.options.allowManualInsertRow && r.current.selectedCell[1] == (r.current.results ? r.current.results[r.current.results.length - 1] : r.current.rows.length - 1) && r.current.insertRow(), r.current.selectedCell && r.current.down()), q.preventDefault();
                            else if (9 == q.which) q.shiftKey ? r.current.left() : (w = r.current.selectedCell[0], r.current.right(), 1 == r.current.options.allowInsertColumn && 1 == r.current.options.allowManualInsertColumn &&
                                r.current.selectedCell[0] == w && (r.current.insertColumn(), r.current.right())), q.preventDefault();
                            else if (!q.ctrlKey && !q.metaKey || q.shiftKey) r.current.selectedCell && r.current.isEditable() && (w = r.current.selectedCell[1], C = r.current.selectedCell[0], r.current.options.columns[C].type != _$_eb2b[18] && (32 == q.keyCode ? r.current.options.columns[C].type == _$_eb2b[262] || r.current.options.columns[C].type == _$_eb2b[264] ? q.preventDefault() : r.current.openEditor(r.current.records[w][C].element, !0, q) : 113 == q.keyCode ? r.current.openEditor(r.current.records[w][C].element,
                                !1, q) : 1 != q.key.length && q.key != _$_eb2b[563] || q.altKey || q.ctrlKey || r.current.records[w] && r.current.openEditor(r.current.records[w][C].element, !0, q)));
                            else if (65 == q.which) r.current.selectAll(), q.preventDefault();
                            else if (83 == q.which) r.current.download(), q.preventDefault();
                            else if (89 == q.which) r.current.parent.redo(), q.preventDefault();
                            else if (90 == q.which) r.current.parent.undo(), q.preventDefault();
                            else if (67 == q.which) r.current.copy(), q.preventDefault();
                            else if (88 == q.which) r.current.cut(), q.preventDefault();
                            else if (86 == q.which) e.paste(q);
                            else if (189 == q.which)
                                if (1 == r.current.options.allowDeleteRow && t[3] == _$_eb2b[423]) {
                                    var C = r.current.getSelectedRows();
                                    C.length && (w = parseInt(C[0].getAttribute(_$_eb2b[43])), C = C.length, 0 <= w && r.current.deleteRow(w, C));
                                    q.preventDefault()
                                } else 1 == r.current.options.allowDeleteColumn && t[3] == _$_eb2b[422] && (C = r.current.getSelectedColumns(), C.length && (w = parseInt(C[0]), C = C.length, 0 <= w && r.current.deleteColumn(w, C)), q.preventDefault())
                }
            };
            var z = function (q, x, w, C, H, L, O) {
                if (document.activeElement.classList.contains(_$_eb2b[94]) &&
                    la.update.call(r.current, q, x, w, C, H, document.activeElement, O)) return !1;
                if (r.current.edition) r.current.closeEditor(r.current.edition, !0);
                else if (l && l !== L) return !1;
                r.current !== O && (r.current.resetSelection(), r.current = O);
                la.current ? la.range.call(r.current, q, x, w, C, H) : za[_$_eb2b[11]].call(r.current, q, x, w, C, H)
            };
            e.mousedown = function (q) {
                q = q || window.event;
                var x = q.buttons ? q.buttons : q.button ? q.button : q.which;
                if (r.current)
                    if (1 == da.isVisible() && (jSuites.findElement(q.target, _$_eb2b[397]) || da.close.call(r.current,
                        !1)), r.current.edition) {
                        if (jSuites.findElement(q.target, _$_eb2b[101])) return !1
                    } else if (jSuites.findElement(q.target, _$_eb2b[228])) return !1;
                var w = y(q);
                if (!1 === w) return !1;
                t = w;
                if (1 < x)
                    if (w[3] == _$_eb2b[424]) {
                        if (r.current.isSelected(w[4], w[5])) return !1
                    } else if (w[3] == _$_eb2b[423]) {
                        if (r.current.isSelected(0, w[4])) return !1
                    } else if (w[3] == _$_eb2b[422] && r.current.isSelected(w[4], 0)) return !1;
                if (r.current) {
                    if (w[3] == _$_eb2b[139]) r.current.isEditable() && (ma.event = !0);
                    else if (w[3] == _$_eb2b[555]) da.open.call(r.current,
                        w[4]);
                    else {
                        if (1 == w[2])
                            if (1 == r.current.options.columnDrag && 6 > q.target.offsetHeight - q.offsetY) ra.start.call(r.current, q);
                            else if (1 == r.current.options.columnResize && 6 > q.target.offsetWidth - q.offsetX) wa.start.call(r.current, q);
                            else {
                                x = r.current.options.columns.length - 1;
                                var C = r.current.options.data.length - 1;
                                if (w[3] == _$_eb2b[552]) z(0, 0, x, C, q, w[3], w[1].jexcel), h = g = 0;
                                else if (w[3] == _$_eb2b[420]) {
                                    var H = oa.range.call(r.current, w[5])[w[4]];
                                    H[1] > x && (H[1] = x);
                                    z(H[0], 0, H[1], C, q, w[3], w[1].jexcel);
                                    g = H[0];
                                    h = 0
                                } else w[4] &&
                                    ((q.shiftKey || q.ctrlKey) && null != g && null != h ? z(g, 0, w[4], C, q, w[3], w[1].jexcel) : (w[3] == _$_eb2b[422] && g == w[4] && 1 == r.current.options.allowRenameColumn && (n = setTimeout(function () {
                                        r.current.setHeader(w[4])
                                    }, 800)), z(w[4], 0, w[4], C, q, w[3], w[1].jexcel), g = w[4], h = 0))
                            }
                        if (2 == w[2])
                            if (w[3] == _$_eb2b[423]) 1 == r.current.options.rowDrag && 6 > q.target.offsetWidth - q.offsetX ? ra.start.call(r.current, q) : 1 == r.current.options.rowResize && 6 > q.target.offsetHeight - q.offsetY ? wa.start.call(r.current, q) : (x = r.current.options.columns.length -
                                1, C = parseInt(w[4]), (q.shiftKey || q.ctrlKey) && null != g && null != h ? z(0, h, x, C, q, w[3], w[1].jexcel) : (z(0, C, x, C, q, w[3], w[1].jexcel), g = 0, h = C));
                            else if (w[3] == _$_eb2b[424]) {
                                x = parseInt(w[4]);
                                C = parseInt(w[5]);
                                if (!r.current.edition && (H = r.current.options.columns[w[4]].type, q.target.tagName == _$_eb2b[564] && (H == _$_eb2b[265] || H == _$_eb2b[266]) && q.target.parentNode.classList.contains(_$_eb2b[126]))) {
                                    var L = q.target.innerText;
                                    H == _$_eb2b[265] && (L = _$_eb2b[565] + L);
                                    window.open(L, _$_eb2b[566])
                                } (q.shiftKey || q.ctrlKey) && r.current.selectedCell ?
                                    z(r.current.selectedCell[0], r.current.selectedCell[1], x, C, q, w[3], w[1].jexcel) : (z(x, C, x, C, q, w[3], w[1].jexcel), g = x, h = C)
                            }
                    }
                    l = w[3];
                    typeof w[3] !== _$_eb2b[13] && F.call(r.current.parent, _$_eb2b[567], r.current, w[3], w[4], w[5]);
                    m = !0
                } else m = !1
            };
            e.mouseup = function (q) {
                r.current && (ra.event ? ra.end.call(r.current, q) : wa.event ? wa.end.call(r.current, q) : ma.event ? ma.end.call(r.current, q) : q.target.classList.contains(_$_eb2b[396]) ? da.close.call(r.current, !0) : r.current.edition && q.target.classList.contains(_$_eb2b[568]) && 50 >
                    q.target.clientWidth - q.offsetX && 50 > q.offsetY ? ba.close.call(r.current, null, !0) : q.target.classList.contains(_$_eb2b[331]) && (q = q.target.innerText == _$_eb2b[218] ? 0 : q.target.innerText == _$_eb2b[219] ? q.target.getAttribute(_$_eb2b[277]) - 1 : q.target.innerText - 1, ua[_$_eb2b[11]].call(r.current, parseInt(q))));
                n && (clearTimeout(n), n = null);
                m = l = !1;
                la.current && la.close.call(r.current)
            };
            e.mousemove = function (q) {
                q = q || window.event;
                if (r.current) {
                    var x = q.target.getAttribute(_$_eb2b[42]),
                        w = q.target.getAttribute(_$_eb2b[43]);
                    if (1 == m) ra.event ? ra.update.call(r.current, q) : wa.event ? wa.update.call(r.current, q) : Pa.event && Pa.update.call(r.current, q);
                    else {
                        var C = q.target.getBoundingClientRect();
                        f && (f.style.cursor = _$_eb2b[3], f = null);
                        1 == r.current.options.editable && (x || w) && (1 == r.current.options.rowResize && q.target && x && !w && 5 > C.width - (q.clientX - C.left) ? (f = q.target, f.style.cursor = _$_eb2b[569]) : 1 == r.current.options.columnResize && q.target && !x && w && 5 > C.height - (q.clientY - C.top) ? (f = q.target, f.style.cursor = _$_eb2b[570]) : 1 == r.current.options.rowDrag &&
                            q.target && !x && w && 5 > C.width - (q.clientX - C.left) ? (f = q.target, f.style.cursor = _$_eb2b[571]) : 1 == r.current.options.columnDrag && q.target && x && !w && 5 > C.height - (q.clientY - C.top) && (f = q.target, f.style.cursor = _$_eb2b[571]))
                    }
                }
            };
            e.mouseover = function (q) {
                q = q || window.event;
                var x = !1;
                if (r.current && 1 == m) {
                    var w = v(q);
                    if (w[0]) {
                        if (!document.activeElement.classList.contains(_$_eb2b[94]) && w[1] && r.current != w[1].jexcel && r.current) return !1;
                        if (!ra.event && !wa.event)
                            if (ma.event) {
                                var C = q.target.getAttribute(_$_eb2b[42]),
                                    H = q.target.getAttribute(_$_eb2b[43]);
                                r.current.isSelected(C, H) || null === C || null === H || (ma.call(r.current, C, H), x = !0)
                            } else null !== g && null !== h && (1 == w[2] && (C = g, w[3] == _$_eb2b[422] && (C = q.target.getAttribute(_$_eb2b[42])), H = r.current.options.data.length - 1, z(g, 0, C, H, q, w[3], w[1].jexcel)), 2 == w[2] && (w[3] == _$_eb2b[423] ? (C = r.current.options.columns.length - 1, H = parseInt(q.target.getAttribute(_$_eb2b[43])), z(0, h, C, H, q, w[3], w[1].jexcel)) : w[3] == _$_eb2b[424] && (C = q.target.getAttribute(_$_eb2b[42]), H = q.target.getAttribute(_$_eb2b[43]), C && H && (z(g, h, C, H, q, w[3],
                                w[1].jexcel), x = !0))))
                    }
                }
                1 == x && !n && G.limited.call(r.current) ? n = setTimeout(function () {
                    var L = r.current.content.getBoundingClientRect();
                    if (q.changedTouches && q.changedTouches[0]) var O = q.changedTouches[0].clientX,
                        R = q.changedTouches[0].clientY;
                    else O = q.clientX, R = q.clientY;
                    50 > L.height - (R - L.top) ? G.pageDown.call(r.current, 1, null, q) : 80 > R - L.top && G.pageUp.call(r.current, 1, null, q);
                    50 > L.width - (O - L.left) ? G.pageRight.call(r.current, 1, null, q) : 80 > O - L.left && G.pageLeft.call(r.current, 1, null, q);
                    n = null
                }, 100) : n && (clearTimeout(n),
                    n = null)
            };
            e.dblclick = function (q) {
                if (r.current)
                    if (q.target.classList.contains(_$_eb2b[351])) ma.execute.call(r.current, !1);
                    else {
                        var x = v(q);
                        1 == x[2] ? 1 == r.current.options.columnSorting && (q = q.target.getAttribute(_$_eb2b[42])) && r.current.orderBy(q) : 2 == x[2] && r.current.isEditable() && !r.current.edition && x[6] && r.current.openEditor(x[6], null, q)
                    }
            };
            e.paste = function (q) {
                if (!q.target.classList.contains(_$_eb2b[228]) && r.current && r.current.selectedCell && !r.current.edition) {
                    var x = q.clipboardData || window.clipboardData;
                    x && (r.current.paste(r.current.selectedCell[0], r.current.selectedCell[1], x.getData(_$_eb2b[55])), q && q.preventDefault())
                }
            };
            e.contextmenu = function (q) {
                var x = y(q);
                !1 !== x && (x[1] || (x = t), r.current && !r.current.edition && (x = Array.prototype.slice.call(x, 3), Ra.open.call(r.current, q, x)))
            };
            var D = Date.now(),
                A = {},
                V = null,
                sa = null,
                Y = null;
            e.touchstart = function (q) {
                var x = v(q);
                x[0] ? x[1] && r.current != x[1].jexcel && (r.current && r.current.resetSelection(), r.current = x[1].jexcel) : r.current && (r.current.resetSelection(), r.current =
                    null);
                if (r.current && !r.current.edition) {
                    var w = x[4];
                    x = x[5];
                    k(q) && (Y = q.touches[0], sa = q.touches.length, D = Date.now(), A.x1 = q.touches[0].screenX, A.y1 = q.touches[0].screenY, A.x2 = q.touches[0].screenX, A.y2 = q.touches[0].screenY, V = setTimeout(b, 250), null != A.speeds && (A.speeds.vertical = 0, A.speeds.horizontal = 0));
                    if (null !== w && null !== x) {
                        r.current.updateSelectionFromCoords(w, x, w, x);
                        var C = q.target;
                        n = setTimeout(function () {
                            r.current.openEditor(C, !1, q);
                            n = null;
                            q.preventDefault();
                            q.stopPropagation();
                            q.stopImmediatePropagation()
                        },
                            750)
                    }
                }
            };
            e.touchmove = function (q) {
                n && (clearTimeout(n), n = null);
                if (k(q) && 1 == sa) {
                    var x = q.changedTouches[0];
                    Y && (r.current.scrollY.scrollTop += -1 * (x.clientY - Y.clientY), r.current.scrollX.scrollLeft += -1 * (x.clientX - Y.clientX));
                    Y = x;
                    A.x2 = q.touches[0].screenX;
                    A.y2 = q.touches[0].screenY;
                    null != A.speeds && (A.speeds.vertical = 0, A.speeds.horizontal = 0);
                    null == V && (V = setTimeout(b, 250));
                    q.cancelable && r.current.options.tableOverflow && q.preventDefault()
                }
            };
            e.touchend = function (q) {
                n && (clearTimeout(n), n = null);
                V && (clearTimeout(V),
                    V = null);
                Y = sa = null;
                if (k(q)) {
                    q = Date.now() - D;
                    var x = A.x1,
                        w = A.y1,
                        C = A.x2,
                        H = A.y2,
                        L = x == x && w == H ? 0 : Math.sqrt(Math.pow(x - x, 2) + Math.pow(w - H, 2));
                    H > w && (L = -L);
                    10 > Math.abs(H - w) && (L = 0);
                    H = 0 >= q ? -1 : Math.ceil(L / (q / 1E3));
                    w = x == C && w == w ? 0 : Math.sqrt(Math.pow(x - C, 2) + Math.pow(w - w, 2));
                    C > x && (w = -w);
                    10 > Math.abs(C - x) && (w = 0);
                    A.speeds = {
                        vertical: H,
                        horizontal: 0 >= q ? -1 : Math.ceil(w / (q / 1E3))
                    };
                    (250 > q && 200 < Math.abs(A.speeds.vertical) || 250 > q && 200 < Math.abs(A.speeds.horizontal)) && requestAnimationFrame(a)
                }
            };
            e.resize = function () {
                r.current && 1 == r.current.parent.config.fullscreen &&
                    G.refresh.call(r.current)
            };
            return e
        }(),
        r = function (k, b, c) {
            b || (b = {});
            if (b.cloud && !c) typeof b.plugins.cloud == _$_eb2b[85] ? b.plugins.cloud.load(b, function (d) {
                r(k, d, !0)
            }) : console.error(_$_eb2b[573]);
            else {
                if (k.spreadsheet) a = k.spreadsheet, Da[_$_eb2b[11]].call(a, b);
                else var a = na.spreadsheet(k, b);
                a.processing = !0;
                1 == a.config.loadingSpin && jSuites.loading.show();
                jSuites.ajax.oncomplete[a.name] = function () {
                    for (var d = 0; d < a.worksheets.length; d++) typeof a.worksheets[d].onload == _$_eb2b[85] && (a.worksheets[d].onload(),
                        a.worksheets[d].onload = null);
                    a.processing = !1;
                    if (Array.isArray(a.queue) && 0 < a.queue.length)
                        for (; d = a.queue.shift();) d[0].updateCell(d[1], d[2], M.call(d[0], d[1], d[2]), !0);
                    a.config.toolbar && fa.show.call(a);
                    for (d = 0; d < a.worksheets.length; d++) aa.refresh.call(a.worksheets[d]);
                    F.call(a, _$_eb2b[574], a);
                    ha.state.call(a);
                    for (d = 0; d < a.worksheets.length; d++) X.update.call(a.worksheets[d]);
                    jSuites.loading.hide()
                };
                Array.isArray(b.worksheets) && (b = b.worksheets);
                if (Array.isArray(b)) {
                    for (c = 0; c < b.length; c++) na.worksheet(a,
                        b[c]);
                    b = a.worksheets
                } else b = na.worksheet(a, b);
                jSuites.ajax.pending(a.name) || (jSuites.ajax.oncomplete[a.name](), jSuites.ajax.oncomplete[a.name] = null);
                return b
            }
        };
    r.license = null;
    r.setLicense = function (k) {
        r.license = k
    };
    r.spreadsheet = [];
    r.picker = la;
    r.setDictionary = function (k) {
        jSuites.setDictionary(k)
    };
    r.extensions = {};
    r.setExtensions = function (k) {
        for (var b = Object.keys(k), c = 0; c < b.length; c++) typeof k[b[c]] === _$_eb2b[85] && (r[b[c]] = k[b[c]], r.extensions[b[c]] = k[b[c]], r.license && typeof r[b[c]].license == _$_eb2b[85] &&
            r[b[c]].license.call(r, r.license))
    };
    r.destroy = function (k, b) {
        if (k.spreadsheet) {
            b && na.unbind(k);
            b = r.spreadsheet.indexOf(k.spreadsheet);
            r.spreadsheet.splice(b, 1);
            for (b = 0; b < k.spreadsheet.worksheets.length; b++) I(k.spreadsheet.worksheets[b].options.worksheetName, null);
            k.spreadsheet = null;
            k.jexcel = null;
            k.innerHTML = _$_eb2b[3];
            k.classList.remove(_$_eb2b[575]);
            k.classList.remove(_$_eb2b[360])
        }
    };
    r.version = K;
    r.helpers = E;
    r.events = Ba;
    typeof formula !== _$_eb2b[13] && (r.formula = formula);
    r.editors = function () {
        var k = {},
            b = function (c, a) {
                return 1 == c[a] || c.options && 1 == c.options[a] ? !0 : c[a] == _$_eb2b[237] || c.options && c.options[a] == _$_eb2b[237] ? !0 : !1
            };
        k.createEditor = function (c, a, d, e) {
            e.parent.input.setAttribute(_$_eb2b[102], !1);
            c == _$_eb2b[227] ? (c = document.createElement(_$_eb2b[227]), c.type = _$_eb2b[55], c.value = d) : c = document.createElement(_$_eb2b[20]);
            c.style.width = a.offsetWidth - 2 + _$_eb2b[44];
            c.style.height = a.offsetHeight - 1 + _$_eb2b[44];
            e.parent.input.appendChild(c);
            return c
        };
        k.createDialog = function (c, a, d, e, f, g) {
            c = k.createEditor(_$_eb2b[20],
                c, a, f);
            c.classList.add(_$_eb2b[576]);
            c.classList.add(_$_eb2b[568]);
            800 > window.innerWidth ? (f.parent.input.style.top = _$_eb2b[145], f.parent.input.style.left = _$_eb2b[145]) : (a = f.parent.input.getBoundingClientRect(), window.innerHeight < a.bottom && (f.parent.input.style.marginTop = -f.parent.input.offsetHeight));
            return c
        };
        k.general = function () {
            var c = {},
                a = function (e) {
                    return (_$_eb2b[3] + e)[0] == _$_eb2b[12] ? !0 : !1
                },
                d = function (e) {
                    if (e.format || e.mask || e.locale) {
                        var f = {};
                        e.mask ? f.mask = e.mask : e.format ? f.mask = e.format : (f.locale =
                            e.locale, f.options = e.options);
                        e.decimal && (f.options || (f.options = {}), f.options = {
                            decimal: e.decimal
                        });
                        return f
                    }
                    return null
                };
            c.createCell = function (e, f, g, h, l, m) {
                m.type != _$_eb2b[14] && m.type != _$_eb2b[320] && m.type != _$_eb2b[319] || e.classList.add(_$_eb2b[577]);
                return c.updateCell(e, f, g, h, l, m)
            };
            c.updateCell = function (e, f, g, h, l, m) {
                //Dev_Edit
                g = parseInt(g);
                h = parseInt(h);
                if (e) {
                    var p = d(m),
                        n = null;
                    f === _$_eb2b[3] || a(f) || typeof f === _$_eb2b[14] || (p ? (p = jSuites.mask.extract(f, p, !0)) && p.value !== _$_eb2b[3] && (n = p.value) : 1 == (!1 === l.parent.config.autoCasting ||
                        !1 === m.autoCasting ? !1 : !0) && (m.type == _$_eb2b[14] || m.type == _$_eb2b[320] || m.type == _$_eb2b[319] ? (p = jSuites.mask.extract(f, m, !0)) && p.value !== _$_eb2b[3] && (n = p.value) : jSuites.isNumeric(Number(f)) && (n = /*Number(f)*/f)));
                    null !== n && (f = n);
                    f = c.parseValue(g, h, f, l, m, e);
                    f instanceof Element || f instanceof HTMLDocument ? (e.innerHTML = _$_eb2b[3], e.appendChild(f)) : 1 == l.parent.config.stripHTML ? e.innerText = f : e.innerHTML = f;
                    if (null !== n) return n
                }
                return f;
            };
            c.openEditor = function (e, f, g, h, l, m) {
                var p = l.parent.input;
                p.onblur = function () {
                    p.classList.contains(_$_eb2b[94]) ||
                        l.closeEditor(e, !0)
                };
                if (m.type == _$_eb2b[14] || m.type == _$_eb2b[320] || m.type == _$_eb2b[319]) p.classList.add(_$_eb2b[560]), a(f) || m.inputmode || (m.inputmode = _$_eb2b[578]);
                g = null;
                a(f) || (m.inputmode && p.setAttribute(_$_eb2b[98], m.inputmode), (g = d(m)) ? m.disabledMaskOnEdition || (m.mask ? (h = m.mask.split(_$_eb2b[220]), p.setAttribute(_$_eb2b[96], h[0])) : m.locale && p.setAttribute(_$_eb2b[579], m.locale), g.input = p, p.mask = g, jSuites.mask.render(f, g, !1)) : f && m.type == _$_eb2b[319] && (f *= 100));
                if (!g || m.disabledMaskOnEdition) l.parent.config.stripHTML ?
                    p.innerText = f : p.innerHTML = f, jSuites.focus(p)
            };
            c.closeEditor = function (e, f, g, h, l, m) {
                e = l.parent.input;
                f = f && e.innerText.trim().length ? e.innerText : _$_eb2b[3];
                a(f) ? f = f.replace(/(\r\n|\n|\r)/gm, _$_eb2b[3]) : m.type == _$_eb2b[319] && (f += _$_eb2b[580], e.classList.remove(_$_eb2b[581]));
                return f
            };
            c.parseValue = function (e, f, g, h, l, m) {
                1 == h.parent.config.parseFormulas && (h.resetArray(e, f), a(g) && (g = h.executeFormula(g, e, f)));
                g instanceof Element || g instanceof HTMLDocument || !l || a(g) || !(f = d(l)) || (e = jSuites.mask.render(g, f,
                    !0), m && f.mask && (f = f.mask.split(_$_eb2b[220]), f[1] && (f[1].match(new RegExp(_$_eb2b[582], _$_eb2b[389])) && (0 > g ? m.classList.add(_$_eb2b[583]) : m.classList.remove(_$_eb2b[583])), f[1].match(new RegExp(_$_eb2b[584], _$_eb2b[389])) && 0 > g && (e = _$_eb2b[215] + e + _$_eb2b[240]))), e && (g = e));
                return g
            };
            c[_$_eb2b[51]] = function (e, f) {
                var g;
                return (g = d(e)) ? jSuites.mask.render(f, g, !0) : e.type == _$_eb2b[319] ? 100 * parseFloat(f) : f
            };
            c.destroyCell = function (e) {
                e.classList.remove(_$_eb2b[577])
            };
            return c
        }();
        k.text = k.general;
        k.number = k.general;
        k.numeric = k.general;
        k.percent = k.general;
        k.notes = function () {
            var c = {},
                a = null;
            c.updateCell = function (d, e, f, g, h, l) {
                d && (f = document.createElement(_$_eb2b[20]), f.classList.add(_$_eb2b[585]), 1 == h.parent.config.stripHTML ? f.innerText = e : f.innerHTML = e, d.innerHTML = _$_eb2b[3], d.appendChild(f))
            };
            c.createCell = c.updateCell;
            c.openEditor = function (d, e, f, g, h, l) {
                var m = l && l.options ? l.options : {};
                m.focus = !0;
                m.value = e;
                m.border = !1;
                m.height = _$_eb2b[586];
                m.toolbar = !1;
                a = k.createDialog(d, e, f, g, h, l);
                a = jSuites.editor(a, m)
            };
            c.closeEditor =
                function (d, e, f, g, h, l) {
                    return e ? a.getData() : _$_eb2b[3]
                };
            return c
        }();
        k.dropdown = function () {
            var c = {
                createCell: function (a, d, e, f, g, h) {
                    h.render == _$_eb2b[587] ? a.classList.add(_$_eb2b[588]) : a.classList.add(_$_eb2b[589]);
                    if (a = c.updateCell(a, d, e, f, g, h)) return a
                },
                destroyCell: function (a) {
                    a.classList.remove(_$_eb2b[589])
                },
                updateCell: function (a, d, e, f, g, h) {
                    if (a) {
                        var l = c.getItem(a, d, e, f, g, h);
                        l ? a.innerHTML = l : d ? (d = c.fromLabel(a, d, e, f, g, h), d.length ? (l = c.getItem(a, d, e, f, g, h), a.innerHTML = l) : a.innerHTML = _$_eb2b[3]) : a.innerHTML =
                            _$_eb2b[3];
                        return d
                    }
                },
                openEditor: function (a, d, e, f, g, h) {
                    var l = k.createEditor(_$_eb2b[20], a, d, g),
                        m = h.delimiter || _$_eb2b[220];
                    h = Object.create(h);
                    h.options || (h.options = {});
                    typeof h.filter == _$_eb2b[85] && (h.source = h.filter(g.el, a, e, f, h.source));
                    typeof h.filterOptions == _$_eb2b[85] && (h = h.filterOptions(g, a, e, f, d, h));
                    e = h.options;
                    h.source && (e.data = JSON.parse(JSON.stringify(h.source)));
                    b(h, _$_eb2b[590]) && (e.multiple = !0);
                    b(h, _$_eb2b[56]) && (e.autocomplete = !0);
                    e.format = !0;
                    e.opened = !0;
                    e.width = a.offsetWidth - 2;
                    e.onclose =
                        function () {
                            g.closeEditor(a, !0)
                        };
                    b(h, _$_eb2b[590]) ? d && (e.value = (_$_eb2b[3] + d).split(m)) : e.value = d;
                    jSuites.dropdown(l, e)
                },
                closeEditor: function (a, d, e, f, g, h) {
                    for (var l = g.parent.input.children[0], m = h.delimiter || _$_eb2b[220], p = l.dropdown.getText(!0), n = l.dropdown.getValue(!0), t = 0; t < n.length; t++) !1 === c.getItem(a, n[t], e, f, g, h) && h.source.push({
                        id: n[t],
                        name: p[t]
                    });
                    l.dropdown.close(!0);
                    if (d) return n.join(m)
                },
                fromLabel: function (a, d, e, f, g, h) {
                    a = h.delimiter || _$_eb2b[220];
                    e = [];
                    f = [];
                    Array.isArray(d) || (d = (_$_eb2b[3] +
                        d).split(a));
                    for (g = 0; g < d.length; g++) f[d[g].trim()] = !0;
                    d = Object.keys(f);
                    !b(h, _$_eb2b[590]) && 1 < Object.keys(d).length && (f = [], f[d[0]] = !0);
                    for (d = 0; d < h.source.length; d++) f[h.source[d].name] && e.push(h.source[d].id);
                    return e.join(a)
                },
                getItem: function (a, d, e, f, g, h) {
                    f = h.delimiter || _$_eb2b[220];
                    a = [];
                    e = [];
                    Array.isArray(d) || (d = (_$_eb2b[3] + d).split(f));
                    for (f = 0; f < d.length; f++) e[d[f].trim()] = !0;
                    f = Object.keys(e);
                    !b(h, _$_eb2b[590]) && 1 < Object.keys(f).length && (d = [], d[f[0]] = !0, e = d);
                    if (h.source && typeof h.source[0] !== _$_eb2b[84])
                        for (d =
                            0; d < h.source.length; d++) f = h.source[d], h.source[d] = {
                                id: f,
                                name: f
                            };
                    for (d = 0; d < h.source.length; d++) typeof e[h.source[d].id] !== _$_eb2b[13] && a.push(h.render == _$_eb2b[267] ? h.source[d].color : h.render == _$_eb2b[591] ? h.source[d].image : h.source[d].name);
                    if (a.length) {
                        if (typeof h.render !== _$_eb2b[13]) {
                            for (d = 0; d < a.length; d++) e = d, f = a[d], f = h.render == _$_eb2b[267] ? _$_eb2b[592] + f + _$_eb2b[593] : h.render == _$_eb2b[591] ? _$_eb2b[592] + f + _$_eb2b[593] : h.render == _$_eb2b[587] ? _$_eb2b[595] + (h.source[d].color || _$_eb2b[594]) + _$_eb2b[596] +
                                f + _$_eb2b[597] : void 0, a[e] = f;
                            return a.join(_$_eb2b[3])
                        }
                        for (d = 0; d < a.length; d++) a[d] = (_$_eb2b[3] + a[d]).replace(_$_eb2b[218], _$_eb2b[242]);
                        return a.join(_$_eb2b[308])
                    }
                    return !1
                }
            };
            c[_$_eb2b[51]] = function (a, d) {
                for (var e = 0; e < a.source.length; e++)
                    if (a.source[e].id == d) return a.render == _$_eb2b[267] ? a.source[e].color : a.render == _$_eb2b[591] ? a.source[e].image : a.source[e].name
            };
            return c
        }();
        k.autocomplete = k.dropdown;
        k.calendar = function () {
            var c = {},
                a = null;
            c.updateCell = function (d, e, f, g, h, l) {
                if (d) {
                    f = e;
                    l = c.getFormat(l);
                    0 < e && Number(e) == e && (f = jSuites.calendar.numToDate(e));
                    g = _$_eb2b[3] + e;
                    g.substr(4, 1) == _$_eb2b[188] && g.substr(7, 1) == _$_eb2b[188] ? g = !0 : (g = g.split(_$_eb2b[188]), g = 4 == g[0].length && g[0] == Number(g[0]) && 2 == g[1].length && g[1] == Number(g[1]) ? !0 : !1);
                    g || (g = jSuites.calendar.extractDateFromString(e, l)) && (f = g);
                    if (f = jSuites.calendar.getDateString(f, l)) return d.innerText = f, e;
                    d.innerText = _$_eb2b[3];
                    return _$_eb2b[3]
                }
            };
            c.createCell = c.updateCell;
            c.openEditor = function (d, e, f, g, h, l) {
                l = Object.create(l);
                typeof l.filterOptions ==
                    _$_eb2b[85] && (l = l.filterOptions(h, d, f, g, e, l));
                f = l.options || {};
                f.opened = !0;
                f.onclose = function (m, p) {
                    h.closeEditor(d, !0)
                };
                typeof l.timepicker !== _$_eb2b[13] && (f.time = b(l, _$_eb2b[598]) ? !0 : !1);
                typeof f.readonly !== _$_eb2b[13] && (f.readonly = f.readonly);
                f.value = e || null;
                f.placeholder = f.format;
                a = k.createEditor(_$_eb2b[227], d, e, h);
                jSuites.calendar(a, f);
                !1 === a.calendar.options.readonly && a.focus()
            };
            c.closeEditor = function (d, e, f, g, h) {
                d = a.calendar.close(!0, a.value ? !0 : !1);
                d = a.value ? d : _$_eb2b[3];
                if (e) return d
            };
            c.getFormat =
                function (d) {
                    return d && d.format ? d.format : d && d.options && d.options.format ? d.options.format : _$_eb2b[599]
                };
            c[_$_eb2b[51]] = function (d, e) {
                d = c.getFormat(d);
                return jSuites.calendar.getDateString(e, d)
            };
            return c
        }();
        k.color = function () {
            var c = {},
                a = null;
            c.updateCell = function (d, e, f, g, h, l) {
                d && (l.render == _$_eb2b[600] ? (f = document.createElement(_$_eb2b[20]), f.className = _$_eb2b[267], f.style.backgroundColor = e, d.innerText = _$_eb2b[3], d.appendChild(f)) : (d.style.color = e, d.innerText = e))
            };
            c.createCell = c.updateCell;
            c.openEditor =
                function (d, e, f, g, h, l) {
                    l = Object.create(l);
                    typeof l.filterOptions == _$_eb2b[85] && (l = l.filterOptions(h, d, f, g, e, l));
                    l.value = e;
                    l.onclose = function (m, p) {
                        h.closeEditor(d, !0)
                    };
                    a = k.createEditor(_$_eb2b[227], d, e, h);
                    a = jSuites.color(a, l);
                    a.open()
                };
            c.closeEditor = function (d, e, f, g, h, l) {
                d = a.close(!0);
                if (e) return d
            };
            c[_$_eb2b[51]] = function (d, e) {
                d = document.createElement(_$_eb2b[20]);
                d.style.width = _$_eb2b[601];
                d.style.height = _$_eb2b[22];
                d.style.backgroundColor = e;
                return d.outerHTML
            };
            return c
        }();
        k.checkbox = function () {
            var c = {
                createCell: function (a, d, e, f, g, h) {
                    d = d && d != _$_eb2b[602] && d != _$_eb2b[603] ? !0 : !1;
                    e = document.createElement(_$_eb2b[227]);
                    e.type = _$_eb2b[262];
                    e.checked = d;
                    e.onclick = function () {
                        g.setValue(a, this.checked)
                    };
                    (h && 1 == h.readOnly || !g.isEditable()) && e.setAttribute(_$_eb2b[604], _$_eb2b[604]);
                    a.innerHTML = _$_eb2b[3];
                    a.appendChild(e);
                    return d
                },
                updateCell: function (a, d) {
                    d = d && d != _$_eb2b[602] && d != _$_eb2b[603] ? !0 : !1;
                    a && (a.children[0].checked = d);
                    return d
                },
                openEditor: function (a, d, e, f, g) {
                    d = a.children[0].checked ? !1 : !0;
                    g.setValue(a,
                        d);
                    return !1
                },
                closeEditor: function (a, d) {
                    return !1
                }
            };
            c[_$_eb2b[51]] = function (a, d) {
                return 1 == d || 1 == d || d == _$_eb2b[237] ? jSuites.translate(_$_eb2b[237]) : jSuites.translate(_$_eb2b[602])
            };
            return c
        }();
        k.radio = function () {
            var c = {
                createCell: function (a, d, e, f, g, h) {
                    a.getAttribute(_$_eb2b[42]);
                    e = document.createElement(_$_eb2b[227]);
                    e.type = _$_eb2b[264];
                    e.checked = 1 == d || 1 == d || d == _$_eb2b[237] ? !0 : !1;
                    e.onclick = function () {
                        g.setValue(a, !0)
                    };
                    (h && 1 == h.readOnly || !g.isEditable()) && e.setAttribute(_$_eb2b[604], _$_eb2b[604]);
                    a.innerHTML =
                        _$_eb2b[3];
                    a.appendChild(e)
                },
                updateCell: function (a, d, e, f, g) {
                    d = 1 == d || 1 == d || d == _$_eb2b[237] || d == _$_eb2b[605] ? !0 : !1;
                    a && (a.children[0].checked = d);
                    if (1 == d) {
                        a = [];
                        for (var h = 0; h < g.options.data.length; h++) f != h && g.options.data[h][e] && a.push({
                            x: e,
                            y: h,
                            value: 0
                        });
                        a.length && g.setValue(a)
                    }
                    return d
                },
                openEditor: function (a, d, e, f, g) {
                    return !1
                },
                closeEditor: function (a, d) {
                    return !1
                }
            };
            c[_$_eb2b[51]] = function (a, d) {
                return 1 == d || 1 == d || d == _$_eb2b[237] ? _$_eb2b[237] : _$_eb2b[602]
            };
            return c
        }();
        k.autonumber = function () {
            var c = {},
                a = null;
            c.createCell = function (d, e, f, g, h, l) {
                (e = parseInt(e)) || 0 < parseInt(d.innerText) && (e = parseInt(d.innerText));
                l.sequence || (l.sequence = 0);
                e ? c.isValid(e, f, g, h) || (e = _$_eb2b[254]) : e = l.sequence + 1;
                e > l.sequence && (l.sequence = e);
                return d.innerText = e
            };
            c.updateCell = c.createCell;
            c.openEditor = function (d, e, f, g, h, l) {
                a = k.createEditor(_$_eb2b[227], d, e, h);
                a.onblur = function () {
                    h.closeEditor(d, !0)
                };
                a.focus();
                a.value = e
            };
            c.closeEditor = function (d, e, f, g, h) {
                return e ? a.value : _$_eb2b[3]
            };
            c.isValid = function (d, e, f, g) {
                for (var h, l = 0; l <
                    g.options.data.length; l++)
                    if (h = g.value(e, l), h == d && l != f) return !1;
                return !0
            };
            return c
        }();
        k.progressbar = function () {
            var c = {},
                a = null;
            c.createCell = function (d, e) {
                e = parseInt(e);
                if (100 < e) e = 100;
                else if (!e || 0 > e) e = 0;
                if (d.children[0] && d.children[0].tagName == _$_eb2b[558]) var f = d.children[0];
                else f = document.createElement(_$_eb2b[20]), f.classList.add(_$_eb2b[268]), d.innerText = _$_eb2b[3], d.classList.add(_$_eb2b[606]), d.appendChild(f);
                f.style.width = parseInt(e) + _$_eb2b[580];
                f.setAttribute(_$_eb2b[277], parseInt(e) + _$_eb2b[580])
            };
            c.destroyCell = function (d) {
                d.classList.remove(_$_eb2b[606])
            };
            c.updateCell = c.createCell;
            c.openEditor = function (d, e, f, g, h, l) {
                a = k.createEditor(_$_eb2b[227], d, e, h);
                a.type = _$_eb2b[14];
                a.setAttribute(_$_eb2b[607], 0);
                a.setAttribute(_$_eb2b[608], 100);
                a.onblur = function () {
                    h.closeEditor(d, !0)
                };
                a.focus();
                a.value = e
            };
            c.closeEditor = function (d, e, f, g, h) {
                return e ? a.value : _$_eb2b[3]
            };
            return c
        }();
        k.rating = function () {
            var c = {},
                a = null;
            c.createCell = function (d, e, f, g, h, l) {
                return c.setCell(d, e, l)
            };
            c.destroyCell = function (d) {
                d.classList.remove(_$_eb2b[609])
            };
            c.updateCell = c.createCell;
            c.openEditor = function (d, e, f, g, h, l) {
                a = k.createEditor(_$_eb2b[227], d, e, h);
                a.type = _$_eb2b[14];
                a.setAttribute(_$_eb2b[607], 0);
                a.setAttribute(_$_eb2b[608], 5);
                a.onblur = function () {
                    h.closeEditor(d, !0)
                };
                a.focus();
                a.value = e
            };
            c.closeEditor = function (d, e, f, g, h, l) {
                return e ? a.value : _$_eb2b[3]
            };
            c.setCell = function (d, e, f) {
                e = parseInt(e);
                if (5 < e) e = 5;
                else if (!e || 0 > e) e = 0;
                if (d) {
                    f = f && f.color ? f.color : _$_eb2b[583];
                    var g = document.createElement(_$_eb2b[20]);
                    g.setAttribute(_$_eb2b[277], parseInt(e) + _$_eb2b[610]);
                    g.classList.add(_$_eb2b[269]);
                    for (var h = 0; h < e; h++) {
                        var l = document.createElement(_$_eb2b[455]);
                        l.className = _$_eb2b[509];
                        l.style.color = f;
                        l.innerHTML = _$_eb2b[611];
                        g.appendChild(l)
                    }
                    d.innerHTML = _$_eb2b[3];
                    d.className = _$_eb2b[609];
                    d.appendChild(g)
                }
                return e
            };
            return c
        }();
        k.email = function () {
            var c = {
                createCell: function (a, d, e, f, g, h) {
                    a && (a.children && a.children[0] && a.children[0].tagName == _$_eb2b[564] ? e = a.children[0] : (e = document.createElement(_$_eb2b[204]), a.innerText = _$_eb2b[3], a.appendChild(e), h.options && h.options.url &&
                        e.setAttribute(_$_eb2b[612], h.options.url)), e.innerText = d)
                }
            };
            c.updateCell = c.createCell;
            c.openEditor = function (a, d, e, f, g, h) {
                g.parent.input.classList.add(_$_eb2b[560]);
                g.parent.input.onblur = function () {
                    g.closeEditor(a, !0)
                };
                g.parent.input.innerText = d;
                jSuites.focus(g.parent.input)
            };
            c.closeEditor = function (a, d, e, f, g) {
                g.parent.input.classList.remove(_$_eb2b[560]);
                return d ? g.parent.input.innerText.replace(new RegExp(/\n/, _$_eb2b[191]), _$_eb2b[3]) : _$_eb2b[3]
            };
            return c
        }();
        k.url = k.email;
        k.image = function () {
            var c = {},
                a = null;
            c.createCell = function (d, e, f, g, h, l) {
                if (d)
                    if (e)
                        if (d.children && d.children[0] && d.children[0].tagName == _$_eb2b[613]) d.children[0].src = e;
                        else {
                            var m = document.createElement(_$_eb2b[37]);
                            l.render == _$_eb2b[614] && m.classList.add(_$_eb2b[614]);
                            l.options && (m.style.maxWidth = _$_eb2b[615], l.options.absolute && (d.classList.add(_$_eb2b[616]), m.setAttribute(_$_eb2b[617], -1), m.classList.add(_$_eb2b[228]), m.classList.add(_$_eb2b[618]), m.settings = function () {
                                h.openEditor(d, e, f, g, h, l)
                            }, m.dblclick = function () {
                                h.openEditor(d,
                                    !0)
                            }, m[_$_eb2b[619]] = function () {
                                h.setValueFromCoords(f, g, _$_eb2b[3])
                            }), l.options.width && (m.style.width = parseInt(l.options.width) + _$_eb2b[44]), l.options.height && (m.style.height = parseInt(l.options.height) + _$_eb2b[44]));
                            m.src = e;
                            d.innerHTML = _$_eb2b[3];
                            d.appendChild(m)
                        }
                    else d.innerHTML = _$_eb2b[3]
            };
            c.updateCell = c.createCell;
            c.openEditor = function (d, e, f, g, h, l) {
                (l && l.options ? l.options : {}).value = e;
                a = k.createDialog(d, e, f, g, h, l);
                e && (d = document.createElement(_$_eb2b[37]), d.src = e, d.classList.add(_$_eb2b[620]),
                    d.style.width = _$_eb2b[615], a.appendChild(d));
                jSuites.image(a, l)
            };
            c.closeEditor = function (d, e, f, g, h, l) {
                if (e && (d = a.children[0])) return d.tagName == _$_eb2b[613] ? d.src : _$_eb2b[3]
            };
            c[_$_eb2b[51]] = function (d, e) {
                return _$_eb2b[621] + e + _$_eb2b[622]
            };
            return c
        }();
        k.html = function () {
            var c = {},
                a = null;
            c.createCell = function (d, e) {
                d.classList.add(_$_eb2b[623]);
                var f = document.createElement(_$_eb2b[20]);
                f.innerHTML = e;
                d.appendChild(f)
            };
            c.updateCell = function (d, e) {
                d && (d.firstChild.innerHTML = e)
            };
            c.destroyCell = function (d) {
                d.classList.remove(_$_eb2b[623])
            };
            c.openEditor = function (d, e, f, g, h, l) {
                var m = l && l.options ? l.options : {};
                m.focus = !0;
                m.value = e;
                m.border = !1;
                m.height = _$_eb2b[586];
                e = k.createDialog(d, e, f, g, h, l);
                a = jSuites.editor(e, m);
                a.close = function () {
                    h.closeEditor(d, !0)
                };
                jSuites.tracking(a, !0)
            };
            c.closeEditor = function (d, e, f, g, h, l) {
                jSuites.tracking(a, !1);
                d = a.getData();
                if (e) return d
            };
            return c
        }();
        k.hidden = function () {
            return {
                createCell: function (c, a, d, e, f) {
                    c.style.display = _$_eb2b[40];
                    c.innerText = a
                },
                updateCell: function (c, a, d, e, f) {
                    c && (c.innerText = a)
                },
                openEditor: function (c,
                    a, d, e, f) {
                    return !1
                },
                closeEditor: function (c, a) {
                    return !1
                }
            }
        }();
        k.tags = function () {
            var c = {},
                a = null;
            c.createCell = function (d, e) {
                d.classList.add(_$_eb2b[624]);
                c.updateCell(d, e)
            };
            c.updateCell = function (d, e) {
                d && (d.innerHTML = e)
            };
            c.destroyCell = function (d) {
                d.classList.remove(_$_eb2b[624])
            };
            c.openEditor = function (d, e, f, g, h, l) {
                var m = l && l.options ? l.options : {};
                m.value = e;
                a = k.createDialog(d, e, f, g, h, l);
                d = document.createElement(_$_eb2b[20]);
                d.style.margin = _$_eb2b[22];
                d.style.marginRight = _$_eb2b[625];
                a.appendChild(d);
                a = jSuites.tags(d,
                    m);
                d.focus();
                jSuites.focus(d)
            };
            c.closeEditor = function (d, e, f, g, h, l) {
                d = a.getValue();
                if (e) return d
            };
            return c
        }();
        k.record = function () {
            var c = {},
                a = null;
            c.createCell = function (d, e, f, g, h, l) {
                d.classList.add(_$_eb2b[589]);
                var m;
                (m = h.parent.getWorksheetInstance(l.worksheetId)) ? m.rows ? c.updateCell(d, e, f, g, h, l) : (d.innerText = _$_eb2b[254], h.parent.queue.push([h, f, g])) : (d.innerText = _$_eb2b[254], h.parent.queue.push([h, f, g]))
            };
            c.destroyCell = function (d) {
                d.classList.remove(_$_eb2b[589])
            };
            c.updateCell = function (d, e, f, g,
                h, l) {
                d && (typeof l.worksheetId == _$_eb2b[13] ? d.innerText = _$_eb2b[254] : d.innerHTML = c.getValue(d, e, f, g, h, l))
            };
            c.openEditor = function (d, e, f, g, h, l) {
                var m = l.delimiter || _$_eb2b[220];
                l = Object.create(l);
                l.options || (l.options = {});
                typeof l.filter == _$_eb2b[85] && (l.source = l.filter(h.el, d, f, g, l.source));
                typeof l.filterOptions == _$_eb2b[85] && (l = l.filterOptions(h, d, f, g, e, l));
                f = l.options;
                f.data = c.getSource(h, l);
                b(l, _$_eb2b[590]) && (f.multiple = !0);
                b(l, _$_eb2b[56]) && (f.autocomplete = !0);
                f.format = !0;
                f.opened = !0;
                f.width = d.offsetWidth -
                    2;
                f.onclose = function () {
                    h.closeEditor(d, !0)
                };
                b(l, _$_eb2b[590]) ? e && (f.value = (_$_eb2b[3] + e).split(m)) : f.value = e;
                a = k.createEditor(_$_eb2b[20], d, e, h);
                a = jSuites.dropdown(a, f)
            };
            c.closeEditor = function (d, e, f, g, h, l) {
                d = l.delimiter || _$_eb2b[220];
                f = a.getValue(!0);
                a.close(!0);
                if (e) return f.join(d)
            };
            c.getValue = function (d, e, f, g, h, l) {
                d = [];
                if (typeof l.worksheetId == _$_eb2b[17] && (h = h.parent.getWorksheetInstance(l.worksheetId))) {
                    f = l.worksheetColumn ? l.worksheetColumn : 0;
                    0 <= l.worksheetImage && (f = l.worksheetImage);
                    if (e == _$_eb2b[3] ||
                        0 == e || null == e || void 0 == e) return _$_eb2b[3];
                    e = (_$_eb2b[3] + e).split(_$_eb2b[220]);
                    for (g = 0; g < e.length; g++) {
                        var m = e[g].trim();
                        var p = h.name(f);
                        m = h.getRowById(m);
                        p = !1 === m ? _$_eb2b[626] : m[p];
                        0 <= l.worksheetImage && (p = _$_eb2b[627] + p + _$_eb2b[628]);
                        d.push(p)
                    }
                    return 0 <= l.worksheetImage ? d.join(_$_eb2b[3]) : d.join(_$_eb2b[308])
                }
                return _$_eb2b[254]
            };
            c.getSource = function (d, e) {
                var f = e.worksheetColumn ? e.worksheetColumn : 0,
                    g = null;
                typeof e.worksheetId == _$_eb2b[17] && (g = d.parent.getWorksheetInstance(e.worksheetId));
                if (g) {
                    var h,
                        l = [];
                    for (d = 0; d < g.rows.length; d++)
                        if (h = g.value(f, d)) h = {
                            id: g.getRowId(d),
                            name: h
                        }, void 0 != e.worksheetImage && (h.image = g.value(e.worksheetImage, d)), l.push(h)
                } else console.error(_$_eb2b[629] + e.worksheetId);
                return l
            };
            c[_$_eb2b[51]] = function (d, e) {
                for (var f = 0; f < d.source.length; f++)
                    if (d.source[f].id == e) return d.render == _$_eb2b[267] ? d.source[f].color : d.render == _$_eb2b[591] ? d.source[f].image : d.source[f].name
            };
            return c
        }();
        return k
    }();
    typeof jQuery !== _$_eb2b[13] && function (k) {
        k.fn.jspreadsheet = k.fn.jexcel = function (b) {
            var c =
                k(this)[_$_eb2b[51]](0);
            if (c.jspreadsheet) {
                if (typeof arguments[0] == _$_eb2b[14]) var a = arguments[0],
                    d = 2;
                else a = 0, d = 1;
                return c.jspreadsheet[a][b].apply(c.jspreadsheet[a], Array.prototype.slice.call(arguments, d))
            }
            return r(k(this)[_$_eb2b[51]](0), arguments[0])
        }
    }(jQuery);
    return r
});