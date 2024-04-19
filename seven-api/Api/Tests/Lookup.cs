using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using seven_library.Api.Library.Lookup;

namespace Seven.Api.Tests {
    [TestFixture]
    public class Lookup {
        private static void AssertCarrier(Carrier actual, Carrier expected) {
            Assert.That(actual.Country, Is.EqualTo(expected.Country));
            Assert.That(actual.Name, Is.EqualTo(expected.Name));
            Assert.That(actual.NetworkCode, Is.EqualTo(expected.NetworkCode));
            Assert.That(actual.NetworkType, Is.EqualTo(expected.NetworkType));
        }

        [Test]
        public async Task CallerName()
        {
            const string number = "4917612345678";
            var lookupParams = new LookupParams(number);
            var cnams = await BaseTest.Client.Lookup.CallerName(lookupParams);
            Assert.Equals(1, cnams.Length);

            var cnam = cnams.First();
            Assert.That(cnam.Code, Is.EqualTo("100"));
            Assert.That(cnam.Name, Is.EqualTo("GERMANY"));
            Assert.That(cnam.Number, Is.EqualTo(number));
            Assert.That(cnam.Success, Is.EqualTo("true"));
        }

        [Test]
        public async Task Format() {
            var lookupParams = new LookupParams("4917612345678");
            var formats = await BaseTest.Client.Lookup.Format(lookupParams);
            Assert.Equals(1, formats.Length);
            
            var format = formats.First();
            Assert.That(format.Carrier, Is.Not.Empty);
            Assert.That(format.CountryCode, Is.Not.Empty);
            Assert.That(format.CountryIso, Is.Not.Empty);
            Assert.That(format.CountryName, Is.Not.Empty);
            Assert.That(format.International, Is.Not.Empty);
            Assert.That(format.InternationalFormatted, Is.Not.Empty);
            Assert.That(format.National, Is.Not.Empty);
            Assert.That(format.NetworkType, Is.Not.Empty);
            Assert.That(format.Success, Is.True);
        }

        [Test]
        public async Task Hlr() {
            var lookupParams = new LookupParams("4917612345678");
            var hlrs = await BaseTest.Client.Lookup.HomeLocationRegister(lookupParams);
            Assert.Equals(1, hlrs.Length);
            
            var carrier = new Carrier {
                NetworkCode = "26207",
                Name = "Telefónica Germany GmbH & Co. oHG (O2)",
                Country = "DE",
                NetworkType = "mobile"
            };
            var dummy = new HlrLookup {
                CountryCode = "DE",
                CountryName = "Germany",
                CountryPrefix = "49",
                CurrentCarrier = carrier,
                GsmCode = null,
                GsmMessage = null,
                InternationalFormatNumber = "4917612345678",
                InternationalFormatted = "+49 176 12345678",
                LookupOutcome = true,
                LookupOutcomeMessage = "success",
                NationalFormatNumber = "0176 12345678",
                OriginalCarrier = carrier,
                Ported = "assumed_not_ported",
                Reachable = "unknown",
                Roaming = "not_roaming",
                Status = true,
                StatusMessage = "success",
                ValidNumber = "valid",
            };
            var hlr = hlrs.First();
            Assert.That(hlr.Status, Is.EqualTo(dummy.Status));
            Assert.That(hlr.StatusMessage, Is.EqualTo(dummy.StatusMessage));
            Assert.That(hlr.LookupOutcome, Is.EqualTo(dummy.LookupOutcome));
            Assert.That(hlr.LookupOutcomeMessage, Is.EqualTo(dummy.LookupOutcomeMessage));
            Assert.That(hlr.InternationalFormatNumber, Is.EqualTo(dummy.InternationalFormatNumber));
            Assert.That(hlr.InternationalFormatted, Is.EqualTo(dummy.InternationalFormatted));
            Assert.That(hlr.NationalFormatNumber, Is.EqualTo(dummy.NationalFormatNumber));
            Assert.That(hlr.CountryCode, Is.EqualTo(dummy.CountryCode));
            Assert.That(hlr.CountryName, Is.EqualTo(dummy.CountryName));
            Assert.That(hlr.CountryPrefix, Is.EqualTo(dummy.CountryPrefix));
            AssertCarrier(hlr.CurrentCarrier, carrier);
            AssertCarrier(hlr.OriginalCarrier, carrier);
            Assert.That(hlr.ValidNumber, Is.EqualTo(dummy.ValidNumber));
            Assert.That(hlr.Reachable, Is.EqualTo(dummy.Reachable));
            Assert.That(hlr.Ported, Is.EqualTo(dummy.Ported));
            Assert.That(hlr.Roaming, Is.EqualTo(dummy.Roaming));
            Assert.That(hlr.GsmCode, Is.EqualTo(dummy.GsmCode));
            Assert.That(hlr.GsmMessage, Is.EqualTo(dummy.GsmMessage));
        }

        [Test]
        public async Task Mnp() {
            var lookupParams = new LookupParams("4917612345678");
            var mnps = await BaseTest.Client.Lookup.MobileNumberPortability(lookupParams);
            Assert.Equals(1, mnps.Length);

            var mnp = mnps.First();
            Assert.That(mnp.Code, Is.EqualTo(100));
            Assert.That(mnp.Mnp.Country, Is.Not.Empty);
            Assert.That(mnp.Mnp.InternationalFormatted, Is.Not.Empty);
            Assert.That(mnp.Mnp.IsPorted, Is.False);
            Assert.That(mnp.Mnp.MobileCountryCodeMobileNetworkCode, Is.Not.Empty);
            Assert.That(mnp.Mnp.NationalFormat, Is.Not.Empty);
            Assert.That(mnp.Mnp.Network, Is.Not.Empty);
            Assert.That(mnp.Mnp.Number, Is.Not.Empty);
            Assert.That(mnp.Success, Is.True);
        }
    }
}