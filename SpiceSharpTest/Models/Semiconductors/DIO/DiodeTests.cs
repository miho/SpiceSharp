﻿using System;
using System.Collections.Generic;
using System.Numerics;
using NSubstitute;
using NUnit.Framework;
using SpiceSharp;
using SpiceSharp.Behaviors;
using SpiceSharp.Components;
using SpiceSharp.Components.DiodeBehaviors;
using SpiceSharp.Entities;
using SpiceSharp.Simulations;

namespace SpiceSharpTest.Models
{
    /// <summary>
    /// From LTSpice
    /// .model 1N914 D(Is= 2.52n Rs = .568 N= 1.752 Cjo= 4p M = .4 tt= 20n Iave = 200m Vpk= 75 mfg= OnSemi type= silicon)
    /// </summary>
    [TestFixture]
    public class DiodeTests : Framework
    {
        private Diode CreateDiode(string name, string anode, string cathode, string model)
        {
            var d = new Diode(name, anode, cathode, model);
            return d;
        }

        private DiodeModel CreateDiodeModel(string name, string parameters)
        {
            var dm = new DiodeModel(name);
            ApplyParameters(dm, parameters);
            return dm;
        }

        [Test]
        public void When_SimpleDC_Expect_Spice3f5Reference()
        {
            /*
             * DC voltage shunted by a diode
             * Current is to behave like the reference
             */

            // Build circuit
            var ckt = new Circuit();
            ckt.Add(
                CreateDiode("D1", "OUT", "0", "1N914"),
                CreateDiodeModel("1N914", "Is=2.52e-9 Rs=0.568 N=1.752 Cjo=4e-12 M=0.4 tt=20e-9"),
                new VoltageSource("V1", "OUT", "0", 0.0)
                );

            // Create simulation
            var dc = new DC("DC", "V1", -1.0, 1.0, 10e-3);

            // Create exports
            IExport<double>[] exports = { new RealPropertyExport(dc, "V1", "i") };

            // Create reference
            double[][] references =
            {
                new[] { 2.520684772022719e-09, 2.520665232097485e-09, 2.520645248083042e-09, 2.520624819979389e-09, 2.520603725741921e-09, 2.520582409459848e-09, 2.520560649088566e-09, 2.520538000538863e-09, 2.520515129944556e-09, 2.520491593216434e-09, 2.520467612399102e-09, 2.520442965447955e-09, 2.520417652362994e-09, 2.520391229055008e-09, 2.520364583702417e-09, 2.520336828126801e-09, 2.520307962328161e-09, 2.520278874484916e-09, 2.520248454374041e-09, 2.520216701995537e-09, 2.520184505527823e-09, 2.520150754747874e-09, 2.520115449655691e-09, 2.520079700474298e-09, 2.520041952891461e-09, 2.520002650996389e-09, 2.519962460922898e-09, 2.519919828358752e-09, 2.519875419437767e-09, 2.519829456204548e-09, 2.519781050480674e-09, 2.519730646355356e-09, 2.519677799739384e-09, 2.519621844498943e-09, 2.519563668812452e-09, 2.519502162456888e-09, 2.519437547476855e-09, 2.519369379783143e-09, 2.519297437331147e-09, 2.519221276031658e-09, 2.519140673840070e-09, 2.519055408711779e-09, 2.518964592468365e-09, 2.518868003065222e-09, 2.518765085390839e-09, 2.518655506378309e-09, 2.518538155804606e-09, 2.518412922647428e-09, 2.518278474639146e-09, 2.518133923601340e-09, 2.517978381355590e-09, 2.517810848701174e-09, 2.517629882348160e-09, 2.517434039006616e-09, 2.517221764364308e-09, 2.516991060019791e-09, 2.516739705527016e-09, 2.516465702484538e-09, 2.516165609200982e-09, 2.515836872163391e-09, 2.515475161501968e-09, 2.515076480413825e-09, 2.514635832895351e-09, 2.514147445786818e-09, 2.513604324683172e-09, 2.512998475978634e-09, 2.512320573799798e-09, 2.511559182849510e-09, 2.510700980451475e-09, 2.509729757349533e-09, 2.508625973618450e-09, 2.507366425597013e-09, 2.505921525841615e-09, 2.504256357838130e-09, 2.502326790221332e-09, 2.500077533884593e-09, 2.497439421933478e-09, 2.494324247148683e-09, 2.490618655759391e-09, 2.486175321170236e-09, 2.480800564974572e-09, 2.474236482363779e-09, 2.466134130241215e-09, 2.456014613905211e-09, 2.443208080293857e-09, 2.426758793916406e-09, 2.405272869765440e-09, 2.377086694149710e-09, 2.341755483969976e-09, 2.297702500486665e-09, 2.242774105321033e-09, 2.174284835509965e-09, 2.088886258411193e-09, 1.982402894618041e-09, 1.849628367134315e-09, 1.684070757845824e-09, 1.477634958835239e-09, 1.220227058285062e-09, 8.992606936875092e-10, 4.990415580774510e-10, -4.208324063460023e-23, -6.222658915921997e-10, -1.398183520351370e-09, -2.365693620165477e-09, -3.572105541915782e-09, -5.076410555804323e-09, -6.952166481388744e-09, -9.291094477115180e-09, -1.220756418174318e-08, -1.584418615752092e-08, -2.037878504834723e-08, -2.603309548487864e-08, -3.308360396747645e-08, -4.187506874586688e-08, -5.283737797290300e-08, -6.650657008444583e-08, -8.355104497148602e-08, -1.048042475026989e-07, -1.313054202034536e-07, -1.643504193848955e-07, -2.055550786805860e-07, -2.569342167357824e-07, -3.210001533471285e-07, -4.008855497006358e-07, -5.004965768495850e-07, -6.247039000539800e-07, -7.795808144028804e-07, -9.727001679671332e-07, -1.213504582209257e-06, -1.513768057126441e-06, -1.888171507258285e-06, -2.355020333966173e-06, -2.937139061076621e-06, -3.662986687191783e-06, -4.568047149322574e-06, -5.696562662471649e-06, -7.103694343424394e-06, -8.858215224893939e-06, -1.104586649613992e-05, -1.377353976839135e-05, -1.717448782878606e-05, -2.141481549700064e-05, -2.670156304629412e-05, -3.329276978536466e-05, -4.150999799246158e-05, -5.175391113643180e-05, -6.452363948283857e-05, -8.044083562419591e-05, -1.002795274224200e-04, -1.250031216609715e-04, -1.558102032684916e-04, -1.941911156088105e-04, -2.419976971548277e-04, -3.015289829039203e-04, -3.756361388815854e-04, -4.678503519079946e-04, -5.825377853404534e-04, -7.250859365310891e-04, -9.021256392514054e-04, -1.121792314356274e-03, -1.394028558000970e-03, -1.730927332259435e-03, -2.147110349238757e-03, -2.660129130047428e-03, -3.290866177790397e-03, -4.063900589218683e-03, -5.007786885759424e-03, -6.155179858715831e-03, -7.542725667060601e-03, -9.210636215301937e-03, -1.120187715360643e-02, -1.356093587232876e-02, -1.633219609264214e-02, -1.955802291413677e-02, -2.327673904312388e-02, -2.752072521737903e-02, -3.231488373640667e-02, -3.767565498752745e-02, -4.361068391378264e-02, -5.011912351695047e-02, -5.719246701131020e-02, -6.481574162201031e-02, -7.296888192813844e-02, -8.162812138207687e-02, -9.076728201979800e-02, -1.003588891563969e-01, -1.103750792457605e-01, -1.207882998568939e-01, -1.315718201008491e-01, -1.427000796200868e-01, -1.541489071231517e-01, -1.658956380366401e-01, -1.779191572005734e-01, -1.901998880621483e-01, -2.027197453741645e-01, -2.154620644191900e-01, -2.284115164341036e-01, -2.415540172223232e-01, -2.548766338536659e-01, -2.683674927728656e-01, -2.820156914701786e-01 }
            };

            // Run test
            AnalyzeDC(dc, ckt, exports, references);
            DestroyExports(exports);
        }

        [Test]
        public void When_SimpleSmallSignal_Expect_Spice3f5Reference()
        {
            /*
             * DC voltage source shunted by a diode
             * Current is expected to behave like the reference
             */
            // Build circuit
            var ckt = new Circuit();
            ckt.Add(
                CreateDiode("D1", "0", "OUT", "1N914"),
                CreateDiodeModel("1N914", "Is=2.52e-9 Rs=0.568 N=1.752 Cjo=4e-12 M=0.4 tt=20e-9"),
                new VoltageSource("V1", "OUT", "0", 1.0)
                    .SetParameter("acmag", 1.0)
                );

            // Create simulation
            var ac = new AC("ac", new DecadeSweep(1e3, 10e6, 5));

            // Create exports
            IExport<Complex>[] exports = { new ComplexPropertyExport(ac, "V1", "i") };

            // Create references
            double[] riRef = { -1.945791742986885e-12, -1.904705637099517e-08, -1.946103289747125e-12, -3.018754997881332e-08, -1.946885859826953e-12, -4.784404245850086e-08, -1.948851586992178e-12, -7.582769719229839e-08, -1.953789270386556e-12, -1.201788010800761e-07, -1.966192170307985e-12, -1.904705637099495e-07, -1.997346846331992e-12, -3.018754997881245e-07, -2.075603854314768e-12, -4.784404245849736e-07, -2.272176570837208e-12, -7.582769719228451e-07, -2.765944910274710e-12, -1.201788010800207e-06, -4.006234902415568e-12, -1.904705637097290e-06, -7.121702504803603e-12, -3.018754997872460e-06, -1.494740330300116e-11, -4.784404245814758e-06, -3.460467495474045e-11, -7.582769719089195e-06, -8.398150889530617e-11, -1.201788010744768e-05, -2.080105080892987e-10, -1.904705636876583e-05, -5.195572682013223e-10, -3.018754996993812e-05, -1.302127347221150e-09, -4.784404242316795e-05, -3.267854507347871e-09, -7.582769705163549e-05, -8.205537869558709e-09, -1.201788005200868e-04, -2.060843758802494e-08, -1.904705614805916e-04 };
            Complex[][] references = new Complex[1][];
            references[0] = new Complex[riRef.Length / 2];
            for (int i = 0; i < riRef.Length; i += 2)
            {
                references[0][i / 2] = new Complex(riRef[i], riRef[i + 1]);
            }

            // Run test
            AnalyzeAC(ac, ckt, exports, references);
            DestroyExports(exports);
        }

        [Test]
        public void When_RectifierTransient_Expect_Spice3f5Reference()
        {
            /*
             * Pulsed voltage source towards a resistive voltage divider between 0V and 5V
             * Output voltage is expected to behavior like the reference
             */
            // Build circuit
            var ckt = new Circuit();
            ckt.Add(
                new VoltageSource("V1", "in", "0", new Pulse(0, 5, 1e-6, 10e-9, 10e-9, 1e-6, 2e-6)),
                new VoltageSource("Vsupply", "vdd", "0", 5.0),
                new Resistor("R1", "vdd", "out", 10.0e3),
                new Resistor("R2", "out", "0", 10.0e3),
                CreateDiode("D1", "in", "out", "1N914"),
                CreateDiodeModel("1N914", "Is = 2.52e-9 Rs = 0.568 N = 1.752 Cjo = 4e-12 M = 0.4 tt = 20e-9")
            );

            // Create simulation
            var tran = new Transient("tran", 1e-9, 10e-6);

            // Create exports
            IExport<double>[] exports = { new RealVoltageExport(tran, "out") };

            // Create references
            double[][] references =
            {
                new[] { 2.499987387600927e+00, 2.499987387600927e+00, 2.499987387600927e+00, 2.499987387600931e+00, 2.499987387600930e+00, 2.499987387600930e+00, 2.499987387600929e+00, 2.499987387600930e+00, 2.499987387600933e+00, 2.499987387600932e+00, 2.499987387600934e+00, 2.499987387600934e+00, 2.499987387600928e+00, 2.499987387600928e+00, 2.499987387600927e+00, 2.499987387600927e+00, 2.499987387600926e+00, 2.499987387600928e+00, 2.499987387600927e+00, 2.499987387600927e+00, 2.499987387600927e+00, 2.961897877874934e+00, 3.816739529558831e+00, 5.191526587205253e+00, 6.033962809915079e+00, 5.858560892037079e+00, 5.542221840643947e+00, 5.047948278928924e+00, 4.566221446174492e+00, 4.496222403679634e+00, 4.470638440255675e+00, 4.461747124949804e+00, 4.458828418109562e+00, 4.458043432344522e+00, 4.458104168570753e+00, 4.458101940017489e+00, 4.458103007267981e+00, 4.458102313327546e+00, 4.458102774372732e+00, 4.458102484881135e+00, 3.958696587137276e+00, 2.960963748330885e+00, 9.726613059613628e-01, -5.081214667849954e-01, -5.008294438061146e-01, -4.823457266305946e-01, -4.093596340082378e-01, 2.927533877100476e-01, 1.580990201373001e+00, 2.297731671229221e+00, 2.465955512323740e+00, 2.496576390164448e+00, 2.500278606332533e+00, 2.499872222320282e+00, 2.500061542030393e+00, 2.499929263366373e+00, 2.500032946810125e+00, 2.499951677124709e+00, 2.500009659840817e+00, 2.961918369398067e+00, 3.816756624765787e+00, 5.191537959402029e+00, 6.033971024560395e+00, 5.858568428923525e+00, 5.542228175717445e+00, 5.047952819852995e+00, 4.566222065264992e+00, 4.496222553506734e+00, 4.470638537994252e+00, 4.461747150642569e+00, 4.458828425715773e+00, 4.458043431871857e+00, 4.458104168387089e+00, 4.458101940091086e+00, 4.458103007220202e+00, 4.458102313359292e+00, 4.458102774351643e+00, 4.458102484892605e+00, 3.958696587148540e+00, 2.960963748341582e+00, 9.726613059724690e-01, -5.081214667705706e-01, -5.008294437900119e-01, -4.823457266089376e-01, -4.093596339411046e-01, 2.927533879025108e-01, 1.580990201486391e+00, 2.297731671249843e+00, 2.465955512329101e+00, 2.496576390165203e+00, 2.500278606332566e+00, 2.499872222320265e+00, 2.500061542030405e+00, 2.499929263366364e+00, 2.500032946810133e+00, 2.499951677124703e+00, 2.500009659840779e+00, 2.961918369398032e+00, 3.816756624765757e+00, 5.191537959401995e+00, 6.033971024560496e+00, 5.858568428923665e+00, 5.542228175717561e+00, 5.047952819853082e+00, 4.566222065265002e+00, 4.496222553506737e+00, 4.470638537994254e+00, 4.461747150642569e+00, 4.458828425715771e+00, 4.458043431871858e+00, 4.458104168387090e+00, 4.458101940091087e+00, 4.458103007220201e+00, 4.458102313359291e+00, 4.458102774351643e+00, 4.458102484892708e+00, 3.958696587148749e+00, 2.960963748341791e+00, 9.726613059726750e-01, -5.081214667704262e-01, -5.008294437900225e-01, -4.823457266089518e-01, -4.093596339411494e-01, 2.927533879023823e-01, 1.580990201486315e+00, 2.297731671249816e+00, 2.465955512329098e+00, 2.496576390165200e+00, 2.500278606332570e+00, 2.499872222320263e+00, 2.500061542030407e+00, 2.499929263366362e+00, 2.500032946810134e+00, 2.499951677124702e+00, 2.500009659840780e+00, 2.961918369398029e+00, 3.816756624765742e+00, 5.191537959401949e+00, 6.033971024560461e+00, 5.858568428923622e+00, 5.542228175717498e+00, 5.047952819852998e+00, 4.566222065264983e+00, 4.496222553506735e+00, 4.470638537994251e+00, 4.461747150642568e+00, 4.458828425715771e+00, 4.458043431871856e+00, 4.458104168387091e+00, 4.458101940091087e+00, 4.458103007220201e+00, 4.458102313359291e+00, 4.458102774351643e+00, 4.458102484892708e+00, 3.958696587148324e+00, 2.960963748341793e+00, 9.726613059726769e-01, -5.081214667704226e-01, -5.008294437900186e-01, -4.823457266089463e-01, -4.093596339411322e-01, 2.927533879024313e-01, 1.580990201486346e+00, 2.297731671249819e+00, 2.465955512329098e+00, 2.496576390165201e+00, 2.500278606332567e+00, 2.499872222320263e+00, 2.500061542030407e+00, 2.499929263366362e+00, 2.500032946810135e+00, 2.499951677124702e+00, 2.500009659840780e+00, 2.961918369398421e+00, 3.816756624765698e+00, 5.191537959401959e+00, 6.033971024560470e+00, 5.858568428923640e+00, 5.542228175717541e+00, 5.047952819853064e+00, 4.566222065265000e+00, 4.496222553506738e+00, 4.470638537994254e+00, 4.461747150642569e+00, 4.458828425715772e+00, 4.458043431871857e+00, 4.458104168387089e+00, 4.458101940091087e+00, 4.458103007220202e+00, 4.458102313359291e+00, 4.458102774351643e+00, 4.458102489285038e+00 }
            };

            // Run test
            AnalyzeTransient(tran, ckt, exports, references);
            DestroyExports(exports);
        }

        [Test]
        public void When_SimpleNoise_Expect_Spice3f5Reference()
        {
            // Build the circuit
            var ckt = new Circuit(
                new VoltageSource("V1", "in", "0", 1.0)
                    .SetParameter("acmag", 1.0),
                new Resistor("R1", "in", "out", 10e3),
                CreateDiode("D1", "out", "0", "1N914"),
                CreateDiodeModel("1N914", "Is=2.52e-9 Rs=0.568 N=1.752 Cjo=4e-12 M=0.4 tt=20e-9 Kf=1e-14 Af=0.9")
            );

            // Create the noise, exports and reference values
            var noise = new Noise("Noise", "out", new DecadeSweep(10, 10e9, 10));
            IExport<double>[] exports = { new InputNoiseDensityExport(noise), new OutputNoiseDensityExport(noise) };
            double[][] references =
            {
                new[]
                {
                    1.458723146141516e-11, 1.158744449455564e-11, 9.204629008621218e-12, 7.311891390005248e-12,
                    5.808436458613777e-12, 4.614199756974080e-12, 3.665583825917671e-12, 2.912071407970300e-12,
                    2.313535219179340e-12, 1.838101024918420e-12, 1.460450220663580e-12, 1.160471523977629e-12,
                    9.221899753841859e-13, 7.329162135225890e-13, 5.825707203834432e-13, 4.631470502194742e-13,
                    3.682854571138349e-13, 2.929342153191007e-13, 2.330805964400094e-13, 1.855371770139243e-13,
                    1.477720965884518e-13, 1.177742269198749e-13, 9.394607206055956e-14, 7.501869587444578e-14,
                    5.998414656060420e-14, 4.804177954432334e-14, 3.855562023394363e-14, 3.102049605476259e-14,
                    2.503513416731735e-14, 2.028079222544473e-14, 1.650428418406463e-14, 1.350449721905778e-14,
                    1.112168173606098e-14, 9.228944122102516e-15, 7.725489198094821e-15, 6.531252508160282e-15,
                    5.582636595658667e-15, 4.829124207122907e-15, 4.230588064951540e-15, 3.755153944584402e-15,
                    3.377503257451775e-15, 3.077524746402662e-15, 2.839243492037182e-15, 2.649970196512345e-15,
                    2.499625442488393e-15, 2.380202943769798e-15, 2.285343207313612e-15, 2.209994908152325e-15,
                    2.150145953086284e-15, 2.102609925372879e-15, 2.064856560106980e-15, 2.034877257821301e-15,
                    2.011078530414255e-15, 1.992197793764384e-15, 1.977237163343344e-15, 1.965411950136735e-15,
                    1.956111467419073e-15, 1.948870621182985e-15, 1.943351658816035e-15, 1.939336510665159e-15,
                    1.936731545908949e-15, 1.935588529057677e-15, 1.936148492385046e-15, 1.938919734396916e-15,
                    1.944808160599577e-15, 1.955329191908373e-15, 1.972947835070999e-15, 2.001620965174601e-15,
                    2.047659303373011e-15, 2.121095285435582e-15, 2.237851669884242e-15, 2.423177364004633e-15,
                    2.717087559609432e-15, 3.182970682239311e-15, 3.921190189710068e-15, 5.090542302029580e-15,
                    6.942013717266990e-15, 9.871657130196278e-15, 1.450283032947425e-14, 2.181265815258579e-14,
                    3.332290820407716e-14, 5.137924773110117e-14, 7.953823062476673e-14, 1.230515662817079e-13,
                    1.893480090868467e-13, 2.882127220525774e-13, 4.310267857547614e-13, 6.281208435935691e-13,
                    8.836523208957566e-13, 1.189393904473825e-12, 1.521954697611584e-12
                },
                new[]
                {
                    8.534638344391308e-14, 6.779535126890105e-14, 5.385407086373468e-14, 4.278011820970267e-14,
                    3.398376494660514e-14, 2.699657318711771e-14, 2.144644949112401e-14, 1.703782953318393e-14,
                    1.353593822442315e-14, 1.075428708293890e-14, 8.544743042104929e-15, 6.789639824603726e-15,
                    5.395511784087090e-15, 4.288116518683889e-15, 3.408481192374137e-15, 2.709762016425394e-15,
                    2.154749646826024e-15, 1.713887651032016e-15, 1.363698520155939e-15, 1.085533406007514e-15,
                    8.645790019241169e-16, 6.890686801739967e-16, 5.496558761223330e-16, 4.389163495820129e-16,
                    3.509528169510376e-16, 2.810808993561634e-16, 2.255796623962265e-16, 1.814934628168257e-16,
                    1.464745497292180e-16, 1.186580383143755e-16, 9.656259790603582e-17, 7.901156573102381e-17,
                    6.507028532585746e-17, 5.399633267182545e-17, 4.519997940872793e-17, 3.821278764924051e-17,
                    3.266266395324682e-17, 2.825404399530675e-17, 2.475215268654597e-17, 2.197050154506173e-17,
                    1.976095750422776e-17, 1.800585428672656e-17, 1.661172624620993e-17, 1.550433098080673e-17,
                    1.462469565449698e-17, 1.392597647854824e-17, 1.337096410894887e-17, 1.293010211315486e-17,
                    1.257991298227878e-17, 1.230174786813036e-17, 1.208079346404696e-17, 1.190528314229684e-17,
                    1.176587033824518e-17, 1.165513081170486e-17, 1.156716727907389e-17, 1.149729536147901e-17,
                    1.144179412451907e-17, 1.139770792493967e-17, 1.136268901185206e-17, 1.133487250043722e-17,
                    1.131277706002888e-17, 1.129522602785387e-17, 1.128128474744870e-17, 1.127021079479467e-17,
                    1.126141444153157e-17, 1.125442724977209e-17, 1.124887712607609e-17, 1.124446850611815e-17,
                    1.124096661480939e-17, 1.123818496366791e-17, 1.123597541962708e-17, 1.123422031640957e-17,
                    1.123282618836906e-17, 1.123171879310365e-17, 1.123083915777734e-17, 1.123014043860140e-17,
                    1.122958542623180e-17, 1.122914456423600e-17, 1.122879437510513e-17, 1.122851620999098e-17,
                    1.122829525558689e-17, 1.122811974526514e-17, 1.122798033246109e-17, 1.122786959293455e-17,
                    1.122778162940192e-17, 1.122771175748433e-17, 1.122765625624737e-17, 1.122761217004779e-17,
                    1.122757715113470e-17, 1.122754933462328e-17, 1.122752723918288e-17
                }
            };
            AnalyzeNoise(noise, ckt, exports, references);
            DestroyExports(exports);
        }

        [Test]
        public void When_MultipliersDC_Expect_Reference()
        {
            var model = CreateDiodeModel("1N914", "Is=2.52e-9 Rs=0.568 N=1.752 Cjo=4e-12 M=0.4 tt=20e-9");
            var cktReference = new Circuit(
                new VoltageSource("V1", "in", "0", 0.0), model);
            ParallelSeries(cktReference, name => new Diode(name, "", "", model.Name), "in", "0", 3, 2);
            var cktActual = new Circuit(
                new VoltageSource("V1", "in", "0", 0.0), model,
                new Diode("D1", "in", "0", model.Name).SetParameter("m", 3.0).SetParameter("n", 2.0));

            var dc = new DC("dc", "V1", -1, 1, 0.1);
            var exports = new IExport<double>[] { new RealCurrentExport(dc, "V1") };

            Compare(dc, cktReference, cktActual, exports);
            DestroyExports(exports);
        }

        [Test]
        public void When_MultipliersSmallSignal_Expect_Reference()
        {
            var model = CreateDiodeModel("1N914", "Is=2.52e-9 Rs=0.568 N=1.752 Cjo=4e-12 M=0.4 tt=20e-9");
            var cktReference = new Circuit(
                new VoltageSource("V1", "in", "0", 0.0).SetParameter("acmag", 1.0), model);
            ParallelSeries(cktReference, name => new Diode(name, "", "", model.Name), "in", "0", 3, 2);
            var cktActual = new Circuit(
                new VoltageSource("V1", "in", "0", 0.0).SetParameter("acmag", 1.0), model,
                new Diode("D1", "in", "0", model.Name).SetParameter("m", 3.0).SetParameter("n", 2.0));

            var ac = new AC("ac", new DecadeSweep(0.1, 1e6, 5));
            var exports = new IExport<Complex>[] { new ComplexCurrentExport(ac, "V1") };

            Compare(ac, cktReference, cktActual, exports);
            DestroyExports(exports);
        }

        [Test]
        public void When_MultipliersNoise_Expect_Reference()
        {
            var model = CreateDiodeModel("1N914", "Is=2.52e-9 Rs=5680 N=1.752 Cjo=4e-12 M=0.4 tt=20e-9 Kf=1e-10 Af=0.9");
            var cktReference = new Circuit(
                new VoltageSource("V1", "in", "0", 3).SetParameter("acmag", 1.0),
                new Resistor("R1", "in", "out", 10e3),
                model);
            ParallelSeries(cktReference, name => new Diode(name, "", "", model.Name), "out", "0", 3, 2);
            var cktActual = new Circuit(
                new VoltageSource("V1", "in", "0", 3).SetParameter("acmag", 1.0),
                new Resistor("R1", "in", "out", 10e3), model,
                new Diode("D1", "out", "0", model.Name).SetParameter("m", 3.0).SetParameter("n", 2.0));

            var noise = new Noise("noise", "out", new DecadeSweep(0.1, 1e6, 5));
            var exports = new IExport<double>[] { new InputNoiseDensityExport(noise), new OutputNoiseDensityExport(noise) };

            Compare(noise, cktReference, cktActual, exports);
            DestroyExports(exports);
        }

        [Test]
        public void When_MultipliersTransient_Expect_Reference()
        {
            /*
             * Pulsed voltage source towards a resistive voltage divider between 0V and 5V
             * Output voltage is expected to behavior like the reference
             */
            // Build circuit
            var model = CreateDiodeModel("1N914", "Is = 2.52e-9 Rs = 0.568 N = 1.752 Cjo = 4e-12 M = 0.4 tt = 20e-9");
            var cktReference = new Circuit(
                new VoltageSource("V1", "in", "0", new Pulse(0, 5, 1e-6, 10e-9, 10e-9, 1e-6, 2e-6)),
                new VoltageSource("Vsupply", "vdd", "0", 5.0),
                new Resistor("R1", "vdd", "out", 10.0e3),
                new Resistor("R2", "out", "0", 10.0e3),
                model
            );
            ParallelSeries(cktReference, name => new Diode(name, "", "", model.Name), "in", "out", 3, 2);
            var cktActual = new Circuit(
                new VoltageSource("V1", "in", "0", new Pulse(0, 5, 1e-6, 10e-9, 10e-9, 1e-6, 2e-6)),
                new VoltageSource("Vsupply", "vdd", "0", 5.0),
                new Resistor("R1", "vdd", "out", 10.0e3),
                new Resistor("R2", "out", "0", 10.0e3),
                model,
                new Diode("D1", "in", "out", model.Name).SetParameter("m", 3.0).SetParameter("n", 2.0));

            // Create simulation
            var tran = new Transient("tran", 1e-9, 10e-6);
            IExport<double>[] exports = { new RealVoltageExport(tran, "out") };
            Compare(tran, cktReference, cktActual, exports);
            DestroyExports(exports);
        }

        /*
        [TestCaseSource(nameof(ModelTemperature))]
        public void When_ModelTemperatureBehavior_Expect_Reference(Proxy<IBindingContext> context, IDictionary<string, double> expected)
        {
            var behavior = new ModelTemperatureBehavior("model", context.Value);
            ((ITemperatureBehavior)behavior).Temperature();
            Check.Properties(behavior, expected);
        }
        [TestCaseSource(nameof(Temperature))]
        public void When_TemperatureBehavior_Expect_Reference(Proxy<IComponentBindingContext> context, IDictionary<string, double> expected)
        {
            var behavior = new TemperatureBehavior("entity", context.Value);
            ((ITemperatureBehavior)behavior).Temperature();
            Check.Properties(behavior, expected);
        }
        [TestCaseSource(nameof(Biasing))]
        public void When_BiasingBehavior_Expect_Reference(Proxy<IComponentBindingContext> context, double[] expected)
        {
            var behavior = new BiasingBehavior("entity", context.Value);
            behavior.DoBias(context.Value);
            Check.Solver(context.Value.GetState<IBiasingSimulationState>().Solver, expected);
        }
        [TestCaseSource(nameof(Frequency))]
        public void When_FrequencyBehavior_Expect_Reference(Proxy<IComponentBindingContext> context, Complex[] expected)
        {
            var behavior = new FrequencyBehavior("entity", context.Value);
            behavior.DoBias(context.Value);
            ((IFrequencyBehavior)behavior).InitializeParameters();
            ((IFrequencyBehavior)behavior).Load();
            Check.Solver(context.Value.GetState<IComplexSimulationState>().Solver, expected);
        }
        [TestCaseSource(nameof(Transient))]
        public void When_TimeBehavior_Expect_Reference(Proxy<IComponentBindingContext> context, double[] expected)
        {
            var behavior = new TimeBehavior("entity", context.Value);
            behavior.DoTransient(context.Value);
            Check.Solver(context.Value.GetState<IBiasingSimulationState>().Solver, expected);
        }

        private static IDictionary<string, double> ExpectedModelTemperature(
            double conductance, double vtNominal, double xfc, double f2, double f3)
        {
            return new Dictionary<string, double>
            {
                { nameof(ModelTemperatureBehavior.Conductance), conductance },
                { nameof(ModelTemperatureBehavior.VtNominal), vtNominal },
                { nameof(ModelTemperatureBehavior.Xfc), xfc },
                { nameof(ModelTemperatureBehavior.F2), f2 },
                { nameof(ModelTemperatureBehavior.F3), f3 }
            };
        }
        private static IDictionary<string, double> ExpectedTemperature(
            double tempJunctionCap, double tempJunctionPot, double tempSaturationCurrent,
            double tempFactor1, double tempDepletionCap, double tempVCritical, double tempBreakdownVoltage,
            double vt, double vte)
        {
            return new Dictionary<string, double>
            {
                { nameof(TemperatureBehavior.TempJunctionCap), tempJunctionCap },
                { nameof(TemperatureBehavior.TempJunctionPot), tempJunctionPot },
                { nameof(TemperatureBehavior.TempSaturationCurrent), tempSaturationCurrent },
                { nameof(TemperatureBehavior.TempFactor1), tempFactor1 },
                { nameof(TemperatureBehavior.TempDepletionCap), tempDepletionCap },
                { nameof(TemperatureBehavior.TempVCritical), tempVCritical },
                { nameof(TemperatureBehavior.TempBreakdownVoltage), tempBreakdownVoltage },
                { "Vt", vt },
                { "Vte", vte }
            };
        }
        public static IEnumerable<TestCaseData> ModelTemperature
        {
            get
            {
                IBindingContext context;

                /// 1N914 Is= 2.52n Rs = .568 N= 1.752 Cjo= 4p M = .4 tt= 20n
                context = Substitute.For<IBindingContext>()
                    .Temperature().Parameter(new ModelBaseParameters
                    {
                        SaturationCurrent = 2.52e-9,
                        Resistance = 0.568,
                        EmissionCoefficient = 1.752,
                        TransitTime = 20e-9,
                        GradingCoefficient = 0.4,
                        JunctionCap = 4e-12
                    });
                yield return new TestCaseData(context.AsProxy(), ExpectedModelTemperature(
                    conductance: 1.7605633802816902,
                    vtNominal: 0.025864186384551461, 
                    xfc: -0.69314718055994529, 
                    f2: 0.37892914162759955, 
                    f3: 0.3)).SetName("{m}(1N914)");

                // Check default values and non-default temperature
                context = Substitute.For<IBindingContext>()
                    .Temperature(360).Parameter(new ModelBaseParameters());
                yield return new TestCaseData(context.AsProxy(), ExpectedModelTemperature(
                    conductance: 0,
                    vtNominal: 0.025864186384551461,
                    xfc: -0.69314718055994529,
                    f2: 0.35355339059327379,
                    f3: 0.25)).SetName("{m}(default)");
            }
        }
        public static IEnumerable<TestCaseData> Temperature
        {
            get
            {
                IComponentBindingContext context;

                context = Substitute.For<IComponentBindingContext>()
                    .Temperature(300.0)
                    .ModelTemperature(new ModelBaseParameters
                    {
                        EmissionCoefficient = 1.2,
                        GradingCoefficient = 0.6,
                        SaturationCurrentExp = 1.4,
                        SaturationCurrent = 1e-9,
                        DepletionCapCoefficient = 1.2,
                        JunctionCap = 1e-12,
                        JunctionPotential = 0.6
                    }, bc => new ModelTemperatureBehavior("1N914", bc))
                    .Parameter(new BaseParameters());
                yield return new TestCaseData(context.AsProxy(), ExpectedTemperature(
                    tempJunctionCap: 9.9962771890637727E-13,
                    tempJunctionPot: 0.60033628105424341,
                    tempSaturationCurrent: 9.817043778553681E-10,
                    tempFactor1: 1.048023829997228,
                    tempDepletionCap: 0.57031946700153124,
                    tempVCritical: 0.52494861470597376,
                    tempBreakdownVoltage: 0,
                    vt: 0.025851260754174377,
                    vte: 0.031021512905009249
                    )).SetName("{m}(regular)");

                context = Substitute.For<IComponentBindingContext>()
                    .Temperature(350).SimulationParameter(new BiasingParameters())
                    .ModelTemperature(new ModelBaseParameters
                    {
                        BreakdownVoltage = 5,
                        BreakdownCurrent = 1e-3
                    }, bc => new ModelTemperatureBehavior("breakdown", bc))
                    .Parameter(new BaseParameters());
                yield return new TestCaseData(context.AsProxy(), ExpectedTemperature(
                    tempJunctionCap: 0,
                    tempJunctionPot: 0.9529143523274588,
                    tempSaturationCurrent: 7.1586116556187464E-12,
                    tempFactor1: 0.55820430381345143,
                    tempDepletionCap: 0.4764571761637294,
                    tempVCritical: 0.6579326978972041,
                    tempBreakdownVoltage: 4.434354418206393,
                    vt: 0.030159804213203439,
                    vte: 0.030159804213203439
                    )).SetName("{m}(breakdown)");
            }
        }
        public static IEnumerable<TestCaseData> Biasing
        {
            get
            {
                // Init Junction - use initial condition that allows us to
                // converge quickly to wherever. Automatically biases to V=n*Ut
                {
                    var context = Substitute.For<IComponentBindingContext>()
                    .Nodes("a", "b").Temperature(300.0).Bias()
                    .ModelTemperature(new ModelBaseParameters
                    {
                        EmissionCoefficient = 1.2,
                        GradingCoefficient = 0.6,
                        SaturationCurrentExp = 1.4,
                        SaturationCurrent = 1e-9,
                        DepletionCapCoefficient = 1.2,
                        JunctionCap = 1e-12,
                        JunctionPotential = 0.6
                    }, bc => new ModelTemperatureBehavior("model", bc))
                    .Parameter(new BaseParameters());
                    yield return new TestCaseData(context.AsProxy(), new double[]
                    {
                    0.707106781187548, -0.707106781187548, 0.349259204076985,
                    -0.707106781187548, 0.707106781187548, -0.349259204076985
                    }).SetName("{m}(V=Vcrit)");
                }

                // Init Float regular forward bias
                {
                    var context = Substitute.For<IComponentBindingContext>()
                        .Nodes("a", "b")
                        .Temperature(300.0).Bias(
                            state => state.Solution[1] = 0.7, 
                            state => state.Mode.Returns(IterationModes.Float))
                        .ModelTemperature(new ModelBaseParameters
                        {
                            EmissionCoefficient = 1.2,
                            GradingCoefficient = 0.6,
                            SaturationCurrentExp = 1.4,
                            SaturationCurrent = 1e-9,
                            DepletionCapCoefficient = 1.2,
                            JunctionCap = 1e-12,
                            JunctionPotential = 0.6
                        }, bc => new ModelTemperatureBehavior("model", bc))
                        .Parameter(new BaseParameters());
                    yield return new TestCaseData(context.AsProxy(), new double[]
                    {
                    199.602784638856, -199.602784638856, 133.52996888863,
                    -199.602784638856, 199.602784638856, -133.52996888863
                    }).SetName("{m}(V=0.7)");
                }

                // Init Float with extra node and series/parallel multiplier
                {
                    var context = Substitute.For<IComponentBindingContext>()
                        .Nodes("a", "b").CreateVariable(new Variable("posprime", Units.Volt))
                        .Temperature(450.0).Bias(state =>
                        {
                            state.Solution[1] = 0.6;
                            state.Solution[3] = 0.6;
                        }, state => state.Mode.Returns(IterationModes.Float))
                        .ModelTemperature(new ModelBaseParameters
                        {
                            Resistance = 2.0,
                            SaturationCurrent = 1e-9
                        }, bc => new ModelTemperatureBehavior("model", bc))
                        .Parameter(new BaseParameters
                        {
                            SeriesMultiplier = 2,
                            ParallelMultiplier = 3,
                        });
                    yield return new TestCaseData(context.AsProxy(), new double[]
                    {
                        0.75, double.NaN, -0.75, double.NaN,
                        double.NaN, 480.478402405751, -480.478402405751, -251.040391835992,
                        -0.75, -480.478402405751, 481.228402405751, 251.040391835992
                    }).SetName("{m}(Rs, n=2, m=3)");
                }

                // Init Float in reverse bias with breakdown and area of 2
                {
                    var context = Substitute.For<IComponentBindingContext>()
                        .Nodes("a", "b")
                        .Bias(state => state.Solution[1] = -4.05,
                            state => state.Mode.Returns(IterationModes.Float))
                        .ModelTemperature(new ModelBaseParameters
                        {
                            BreakdownCurrent = 1e-3,
                            BreakdownVoltage = 4
                        }, bc => new ModelTemperatureBehavior("model", bc))
                        .Parameter(new BaseParameters { Area = 2.0 });
                    yield return new TestCaseData(context.AsProxy(), new double[]
                    {
                        0.534439358280788, -0.534439358280788, -2.15065656185935,
                        -0.534439358280788, 0.534439358280788, 2.15065656185935
                    }).SetName("{m}(V=-4.06, Vz=4, A=2)");
                }
            }
        }
        public static IEnumerable<TestCaseData> Frequency
        {
            get
            {
                // Init Junction - use initial condition that allows us to
                // converge quickly to wherever. Automatically biases to V=n*Ut
                {
                    var context = Substitute.For<IComponentBindingContext>()
                    .Nodes("a", "b").Temperature(300.0).Frequency(1e6)
                    .ModelTemperature(new ModelBaseParameters
                    {
                        EmissionCoefficient = 1.2,
                        GradingCoefficient = 0.6,
                        SaturationCurrentExp = 1.4,
                        SaturationCurrent = 1e-9,
                        DepletionCapCoefficient = 1.2,
                        JunctionCap = 1e-12,
                        JunctionPotential = 0.6
                    }, bc => new ModelTemperatureBehavior("model", bc))
                    .Parameter(new BaseParameters());
                    var gr = new Complex(0.707106781187548, 2.18621908461587E-05);
                    yield return new TestCaseData(context.AsProxy(), new Complex[]
                    {
                        gr, -gr, double.NaN,
                        -gr, gr, double.NaN
                    }).SetName("{m}(V=Vcrit, f=1MHz)");
                }

                // Init Float regular forward bias
                {
                    var context = Substitute.For<IComponentBindingContext>()
                        .Nodes("a", "b")
                        .Temperature(300.0).Bias(
                            state => state.Solution[1] = 0.7,
                            state => state.Mode.Returns(IterationModes.Float))
                        .Frequency(50e6)
                        .ModelTemperature(new ModelBaseParameters
                        {
                            EmissionCoefficient = 1.2,
                            GradingCoefficient = 0.6,
                            SaturationCurrentExp = 1.4,
                            SaturationCurrent = 1e-9,
                            DepletionCapCoefficient = 1.2,
                            JunctionCap = 1e-12,
                            JunctionPotential = 0.6,
                            TransitTime = 1e-9
                        }, bc => new ModelTemperatureBehavior("model", bc))
                        .Parameter(new BaseParameters());
                    var g = new Complex(199.602784638856, 62.7138861377778);
                    yield return new TestCaseData(context.AsProxy(), new Complex[]
                    {
                        g, -g, double.NaN,
                        -g, g, double.NaN
                    }).SetName("{m}(V=0.7, f=50MHz)");
                }

                // Init Float with extra node and series/parallel multiplier
                {
                    var context = Substitute.For<IComponentBindingContext>()
                        .Nodes("a", "b").CreateVariable(new Variable("posprime", Units.Volt))
                        .Temperature(450.0).Bias(state =>
                        {
                            state.Solution[1] = 0.6;
                            state.Solution[3] = 0.6;
                        }, state => state.Mode.Returns(IterationModes.Float))
                        .Frequency(20e6)
                        .ModelTemperature(new ModelBaseParameters
                        {
                            Resistance = 2.0,
                            SaturationCurrent = 1e-9,
                            JunctionCap = 1e-10,
                            DepletionCapCoefficient = 1.5
                        }, bc => new ModelTemperatureBehavior("model", bc))
                        .Parameter(new BaseParameters
                        {
                            SeriesMultiplier = 2,
                            ParallelMultiplier = 3,
                        });
                    var g = new Complex(480.478402405751, 0.0249045599396782);
                    yield return new TestCaseData(context.AsProxy(), new Complex[]
                    {
                        0.75, double.NaN, -0.75, double.NaN,
                        double.NaN, g, -g, double.NaN,
                        -0.75, -g, g + 0.75, double.NaN
                    }).SetName("{m}(Rs, n=2, m=3)");
                }

                // Init Float in reverse bias with breakdown and area of 2
                {
                    var context = Substitute.For<IComponentBindingContext>()
                        .Nodes("a", "b")
                        .Bias(state => state.Solution[1] = -4.05,
                            state => state.Mode.Returns(IterationModes.Float))
                        .Frequency(100e6)
                        .ModelTemperature(new ModelBaseParameters
                        {
                            BreakdownCurrent = 1e-3,
                            BreakdownVoltage = 4,
                            JunctionCap = 10e-9,
                            GradingCoefficient = 0.6,
                            DepletionCapCoefficient = 1.5
                        }, bc => new ModelTemperatureBehavior("model", bc))
                        .Parameter(new BaseParameters { Area = 2.0 });
                    var g = new Complex(0.534439358280788, 4.75592543681738);
                    yield return new TestCaseData(context.AsProxy(), new Complex[]
                    {
                        g, -g, double.NaN,
                        -g, g, double.NaN
                    }).SetName("{m}(V=-4.06, Vz=4, A=2)");
                }
            }
        }
        public static IEnumerable<TestCaseData> Transient
        {
            get
            {
                // Init Junction - use initial condition that allows us to
                // converge quickly to wherever. Automatically biases to V=n*Ut
                {
                    var context = Substitute.For<IComponentBindingContext>()
                    .Nodes("a", "b").Temperature(300.0).Transient(0.5, 1e-6)
                    .ModelTemperature(new ModelBaseParameters
                    {
                        EmissionCoefficient = 1.2,
                        GradingCoefficient = 0.6,
                        SaturationCurrentExp = 1.4,
                        SaturationCurrent = 1e-9,
                        DepletionCapCoefficient = 1.2,
                        JunctionCap = 1e-12,
                        JunctionPotential = 0.6
                    }, bc => new ModelTemperatureBehavior("model", bc))
                    .Parameter(new BaseParameters());
                    yield return new TestCaseData(context.AsProxy(), new double[]
                    {
                        0.707107780815267, -0.707107780815267, -0.650740795923015,
                        -0.707107780815267, 0.707107780815267, 0.650740795923015
                    }).SetName("{m}(V=Vcrit, f=1MHz)");
                }

                // Init Float regular forward bias
                {
                    var context = Substitute.For<IComponentBindingContext>()
                        .Nodes("a", "b")
                        .Temperature(300.0).Bias(
                            state => state.Solution[1] = 0.7,
                            state => state.Mode.Returns(IterationModes.Float))
                        .Transient(0.5, 1.0 / 50e6)
                        .ModelTemperature(new ModelBaseParameters
                        {
                            EmissionCoefficient = 1.2,
                            GradingCoefficient = 0.6,
                            SaturationCurrentExp = 1.4,
                            SaturationCurrent = 1e-9,
                            DepletionCapCoefficient = 1.2,
                            JunctionCap = 1e-12,
                            JunctionPotential = 0.6,
                            TransitTime = 1e-9
                        }, bc => new ModelTemperatureBehavior("model", bc))
                        .Parameter(new BaseParameters());
                    yield return new TestCaseData(context.AsProxy(), new double[]
                    {
                        209.584009618186, -209.584009618186, 139.516826374161,
                        -209.584009618186, 209.584009618186, -139.516826374161
                    }).SetName("{m}(V=0.7, f=50MHz)");
                }

                // Init Float with extra node and series/parallel multiplier
                {
                    var context = Substitute.For<IComponentBindingContext>()
                        .Nodes("a", "b").CreateVariable(new Variable("posprime", Units.Volt))
                        .Temperature(450.0).Bias(state =>
                        {
                            state.Solution[1] = 0.6;
                            state.Solution[3] = 0.6;
                        }, state => state.Mode.Returns(IterationModes.Float))
                        .Transient(0.5, 1.0 / 20e6)
                        .ModelTemperature(new ModelBaseParameters
                        {
                            Resistance = 2.0,
                            SaturationCurrent = 1e-9,
                            JunctionCap = 1e-10,
                            DepletionCapCoefficient = 1.5
                        }, bc => new ModelTemperatureBehavior("model", bc))
                        .Parameter(new BaseParameters
                        {
                            SeriesMultiplier = 2,
                            ParallelMultiplier = 3,
                        });
                    yield return new TestCaseData(context.AsProxy(), new double[]
                    {
                        0.75, double.NaN, -0.75, double.NaN,
                        double.NaN, 480.48236608957, -480.48236608957, -248.042770046284,
                        -0.75, -480.48236608957, 481.23236608957, 248.042770046284
                    }).SetName("{m}(Rs, n=2, m=3)");
                }

                // Init Float in reverse bias with breakdown and area of 2
                {
                    var context = Substitute.For<IComponentBindingContext>()
                        .Nodes("a", "b")
                        .Bias(state => state.Solution[1] = -4.05,
                            state => state.Mode.Returns(IterationModes.Float))
                        .Transient(0.5, 1.0 / 100e6)
                        .ModelTemperature(new ModelBaseParameters
                        {
                            BreakdownCurrent = 1e-3,
                            BreakdownVoltage = 4,
                            JunctionCap = 10e-9,
                            GradingCoefficient = 0.6,
                            DepletionCapCoefficient = 1.5
                        }, bc => new ModelTemperatureBehavior("model", bc))
                        .Parameter(new BaseParameters { Area = 2.0 });
                    yield return new TestCaseData(context.AsProxy(), new double[]
                    {
                        1.29136840052676, -1.29136840052676, -6.21621918295552,
                        -1.29136840052676, 1.29136840052676, 6.21621918295552
                    }).SetName("{m}(V=-4.06, Vz=4, A=2)");
                }
            }
        }
        */
    }
}
