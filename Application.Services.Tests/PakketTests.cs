using Core.Domain;
using Xunit;
using Moq;
using Core.DomainServices.Services;
using Core.DomainServices.Interfaces;

namespace Core.DomainServices.Tests
{
    public class PakketTests
    {
        private List<Pakket> pakketLijst = new List<Pakket>()
        {
            new Pakket{
            Id=1,
            titel="Broodje ei",
            prijs=3.5,
            pickup=new DateTime(2023, 11, 10, 0, 0, 0),
            pickUpMax=new DateTime(2023, 11, 11, 0, 0, 0),
            stad=Stad.Breda,
            volwassen=false,
            producten=new List<Product>(),
            maaltijd=Maaltijd.Brood,
            gereserveerd=null,
            warm=false,
            kantine=new Kantine{Id=1,
            stad=Stad.Breda,
            locatie=Locatie.LA,
            naam="The Fifth",
            warm=true
            }
            },

            new Pakket{
            Id=2,
            titel="Tosti",
            prijs=3,
            pickup=new DateTime(2023, 11, 10, 0, 0, 0),
            pickUpMax=new DateTime(2023, 11, 11, 0, 0, 0),
            stad=Stad.Breda,
            volwassen=false,
            producten=new List<Product>(),
            maaltijd=Maaltijd.Brood,
            gereserveerd=null,
            warm=true,
            kantine=new Kantine{Id=1,
            stad=Stad.Breda,
            locatie=Locatie.LA,
            naam="The Fifth",
            warm=false
            }
            },

            new Pakket{
            Id=3,
            titel="Broodje carpaccio + wijn",
            prijs=5.5,
            pickup=new DateTime(2023, 11, 10, 0, 0, 0),
            pickUpMax=new DateTime(2023, 11, 11, 0, 0, 0),
            stad=Stad.Breda,
            volwassen=true,
            producten=new List<Product>(),
            maaltijd=Maaltijd.Brood,
            gereserveerd=new Student
            {
                Id=1,
                naam="Peter Pannenkoek",
                studentnummer=1,
                email="peter@mail.com",
                stad="Dordrecht",
                geboortedatum=new DateTime(2000, 11, 22)
            },
            warm=true,
            kantine=new Kantine{Id=1,
            stad=Stad.Breda,
            locatie=Locatie.LA,
            naam="The Fifth",
            warm=false
            }
            }
        };

        [Fact]
        public void GetPakketten()
        {
            var _pakketRepoMock = new Mock<IPakketRepo>();
            var _pakketService = new PakketService(_pakketRepoMock.Object);

            _pakketRepoMock.Setup(p => p.GetPakketten()).Returns(pakketLijst);

            var pakketten = _pakketService.GetPakketten();

            Assert.Equal(3, pakketten.Count());
            Assert.Equal("Broodje ei", pakketten.ElementAt(0).titel);
            Assert.Equal("Tosti", pakketten.ElementAt(1).titel);
            Assert.Equal("Broodje carpaccio + wijn", pakketten.ElementAt(2).titel);

        }

        [Fact]
        public void GetPakketById()
        {
            var _pakketRepoMock = new Mock<IPakketRepo>();
            var _pakketService = new PakketService(_pakketRepoMock.Object);
            var vergelijkPakket = new Pakket
            {
                Id = 3,
                titel = "Broodje carpaccio + wijn",
                prijs = 5.5,
                pickup = new DateTime(2023, 11, 10, 0, 0, 0),
                pickUpMax = new DateTime(2023, 11, 11, 0, 0, 0),
                stad = Stad.Breda,
                volwassen = true,
                producten = new List<Product>(),
                maaltijd = Maaltijd.Brood,
                gereserveerd = new Student
                {
                    Id = 1,
                    naam = "Peter Pannenkoek",
                    studentnummer = 1,
                    email = "peter@mail.com",
                    stad = "Dordrecht",
                    geboortedatum = new DateTime(2000, 11, 22)
                },
                warm = true,
                kantine = new Kantine
                {
                    Id = 1,
                    stad = Stad.Breda,
                    locatie = Locatie.LA,
                    naam = "The Fifth",
                    warm = false
                }
            };
            _pakketRepoMock.Setup(p => p.GetPakketById(3)).Returns(vergelijkPakket);
            var pakket = _pakketService.GetPakketById(3);


            Assert.Equal("Broodje carpaccio + wijn", pakket.titel);

        }


        [Fact]
        public void GetBeschikbarePakketten_ReturnsBeschikbarePakketten()
        {
            var _pakketRepoMock = new Mock<IPakketRepo>();
            var _pakketService = new PakketService(_pakketRepoMock.Object);

            _pakketRepoMock.Setup(p => p.GetBeschikbarePakketten()).Returns(pakketLijst);

            var pakketten = _pakketService.GetBeschikbarePakketten();

            Assert.Equal(3, pakketten.Count());
        }
        [Fact]
        public void GetBeschikbarePakketten_ReturnsFalseWhenNoAvailable()
        {
            var _pakketRepoMock = new Mock<IPakketRepo>();
            var _pakketService = new PakketService(_pakketRepoMock.Object);

            //Nieuwe lijst meegeven zodat de count 0 is van de nieuwe lijst
            _pakketRepoMock.Setup(p => p.GetBeschikbarePakketten()).Returns(new List<Pakket>());

            var pakketten = _pakketService.GetBeschikbarePakketten();


            Assert.Equal(0, pakketten.Count());
        }


        [Fact]
        public void GetGereserveerdePakketten_ReturnsGereserveerdePakketten()
        {
            var _pakketRepoMock = new Mock<IPakketRepo>();
            var _pakketService = new PakketService(_pakketRepoMock.Object);

            var studentId = 1;

            _pakketRepoMock.Setup(p => p.GetGereserveerdePakketten(studentId)).Returns(pakketLijst);

            var pakketten = _pakketService.GetGereserveerdePakketten(studentId);

            Assert.Equal(3, pakketten.Count());
        }

        [Fact]
        public void GetAlleGereserveerdePakketten()
        {
            var _pakketRepoMock = new Mock<IPakketRepo>();
            var _pakketService = new PakketService(_pakketRepoMock.Object);

            _pakketRepoMock.Setup(p => p.GetAlleGereserveerdePakketten()).Returns(pakketLijst);

            var pakketten = _pakketService.GetAlleGereserveerdePakketten();

            Assert.Equal(3, pakketten.Count());
        }

        [Fact]
        public void AddPakket_Succeeds_WhenValidPakket()
        {
            var _pakketRepoMock = new Mock<IPakketRepo>();
            var _pakketService = new PakketService(_pakketRepoMock.Object);

            var newPakket = new Pakket
            {
                Id = 4,
                titel = "Pizza pepperoni",
                prijs = 5.5,
                pickup = DateTime.Now,
                pickUpMax = DateTime.Now.AddDays(2),
                stad = Stad.Breda,
                volwassen = false,
                producten = new List<Product>
                {
                    new Product
                    {
                        naam="Brood",
                        alcohol=false,
                        foto="https://www.google.com/imgres?imgurl=https%3A%2F%2Fmegafoodstunter.nl%2Fwp-content%2Fuploads%2F2023%2F08%2Fpistolet-wit.jpg&tbnid=jAMccVyusQl81M&vet=12ahUKEwiLgoK00eiBAxWziP0HHQswDTkQMygAegQIARBP..i&imgrefurl=https%3A%2F%2Fmegafoodstunter.nl%2Fwinkel%2Fbakkerij%2Fbrood%2Fpistolet-wit-75x70-gram%2F&docid=Mr_bEd8psnSF8M&w=1000&h=1000&q=pistolet&ved=2ahUKEwiLgoK00eiBAxWziP0HHQswDTkQMygAegQIARBP",
                    }
                },
                maaltijd = Maaltijd.Avondmaaltijd,
                gereserveerd = null,
                warm = true,
                kantine = new Kantine
                {
                    Id = 1,
                    stad = Stad.Breda,
                    locatie = Locatie.LA,
                    naam = "The Fifth",
                    warm = true
                }
            };

            _pakketRepoMock.Setup(p => p.AddPakket(newPakket)).Returns(newPakket);

            var result = _pakketService.AddPakket(newPakket);

            Assert.Equal("Pizza pepperoni", result.titel);
            Assert.Equal(4, result.Id);

        }
        [Fact]
        public void AddPakket_ThrowsException_WhenPickupIsAfterPickUpMax()
        {
            var _pakketRepoMock = new Mock<IPakketRepo>();
            var _pakketService = new PakketService(_pakketRepoMock.Object);

            var newPakket = new Pakket
            {
                Id = 4,
                titel = "Pizza pepperoni",
                prijs = 5.5,
                pickup = DateTime.Now.AddDays(1),
                pickUpMax = DateTime.Now,
                stad = Stad.Breda,
                volwassen = false,
                producten = new List<Product>
        {
            new Product
            {
                naam = "Brood",
                alcohol = false,
                foto = "https://www.google.com/imgres?imgurl=https%3A%2F%2Fmegafoodstunter.nl%2Fwp-content%2Fuploads%2F2023%2F08%2Fpistolet-wit.jpg&tbnid=jAMccVyusQl81M&vet=12ahUKEwiLgoK00eiBAxWziP0HHQswDTkQMygAegQIARBP..i&imgrefurl=https%3A%2F%2Fmegafoodstunter.nl%2Fwinkel%2Fbakkerij%2Fbrood%2Fpistolet-wit-75x70-gram%2F&docid=Mr_bEd8psnSF8M&w=1000&h=1000&q=pistolet&ved=2ahUKEwiLgoK00eiBAxWziP0HHQswDTkQMygAegQIARBP",
            }
        },
                maaltijd = Maaltijd.Avondmaaltijd,
                gereserveerd = null,
                warm = true,
                kantine = new Kantine
                {
                    Id = 1,
                    stad = Stad.Breda,
                    locatie = Locatie.LA,
                    naam = "The Fifth",
                    warm = true
                }
            };
            var exception = Assert.Throws<ArgumentException>(() => _pakketService.AddPakket(newPakket));

            Assert.Equal("De maximale ophaaltijd moet na de eerste ophaalmogelijkheid zijn!", exception.Message);
        }

        [Fact]
        public void AddPakket_ThrowsException_WhenPickupIsMoreThan2DaysLater()
        {
            var _pakketRepoMock = new Mock<IPakketRepo>();
            var _pakketService = new PakketService(_pakketRepoMock.Object);

            var newPakket = new Pakket
            {
                Id = 4,
                titel = "Pizza pepperoni",
                prijs = 5.5,
                pickup = DateTime.Now.AddDays(3),
                pickUpMax = DateTime.Now.AddDays(4),
                stad = Stad.Breda,
                volwassen = false,
                producten = new List<Product>
        {
            new Product
            {
                naam = "Brood",
                alcohol = false,
                foto = "https://www.google.com/imgres?imgurl=https%3A%2F%2Fmegafoodstunter.nl%2Fwp-content%2Fuploads%2F2023%2F08%2Fpistolet-wit.jpg&tbnid=jAMccVyusQl81M&vet=12ahUKEwiLgoK00eiBAxWziP0HHQswDTkQMygAegQIARBP..i&imgrefurl=https%3A%2F%2Fmegafoodstunter.nl%2Fwinkel%2Fbakkerij%2Fbrood%2Fpistolet-wit-75x70-gram%2F&docid=Mr_bEd8psnSF8M&w=1000&h=1000&q=pistolet&ved=2ahUKEwiLgoK00eiBAxWziP0HHQswDTkQMygAegQIARBP",
            }
        },
                maaltijd = Maaltijd.Avondmaaltijd,
                gereserveerd = null,
                warm = true,
                kantine = new Kantine
                {
                    Id = 1,
                    stad = Stad.Breda,
                    locatie = Locatie.LA,
                    naam = "The Fifth",
                    warm = true
                }
            };

            var exception = Assert.Throws<ArgumentException>(() => _pakketService.AddPakket(newPakket));

            Assert.Equal("Er mag geen pakket toegevoegd worden wat pas over 2 dagen op te halen is!", exception.Message);
        }
        [Fact]
        public void AddPakket_ThrowsException_WhenNoProducts()
        {
            var _pakketRepoMock = new Mock<IPakketRepo>();
            var _pakketService = new PakketService(_pakketRepoMock.Object);

            var newPakket = new Pakket
            {
                Id = 4,
                titel = "Pizza pepperoni",
                prijs = 5.5,
                pickup = DateTime.Now.AddDays(1),
                pickUpMax = DateTime.Now.AddDays(3),
                stad = Stad.Breda,
                volwassen = false,
                producten = null,
                maaltijd = Maaltijd.Avondmaaltijd,
                gereserveerd = null,
                warm = true,
                kantine = new Kantine
                {
                    Id = 1,
                    stad = Stad.Breda,
                    locatie = Locatie.LA,
                    naam = "The Fifth",
                    warm = true
                }
            };
            var exception = Assert.Throws<ArgumentException>(() => _pakketService.AddPakket(newPakket));

            Assert.Equal("Er moet minimaal een product toegevoeg worden aan een pakket!", exception.Message);
        }

        [Fact]
        public void UpdatePakket_Succeeds_WhenValidPakket()
        {
            var _pakketRepoMock = new Mock<IPakketRepo>();
            var _pakketService = new PakketService(_pakketRepoMock.Object);

            var newPakket = new Pakket
            {
                Id = 4,
                titel = "Pizza pepperoni",
                prijs = 5.5,
                pickup = DateTime.Now,
                pickUpMax = DateTime.Now.AddDays(2),
                stad = Stad.Breda,
                volwassen = false,
                producten = new List<Product>
                {
                    new Product
                    {
                        naam="Brood",
                        alcohol=false,
                        foto="https://www.google.com/imgres?imgurl=https%3A%2F%2Fmegafoodstunter.nl%2Fwp-content%2Fuploads%2F2023%2F08%2Fpistolet-wit.jpg&tbnid=jAMccVyusQl81M&vet=12ahUKEwiLgoK00eiBAxWziP0HHQswDTkQMygAegQIARBP..i&imgrefurl=https%3A%2F%2Fmegafoodstunter.nl%2Fwinkel%2Fbakkerij%2Fbrood%2Fpistolet-wit-75x70-gram%2F&docid=Mr_bEd8psnSF8M&w=1000&h=1000&q=pistolet&ved=2ahUKEwiLgoK00eiBAxWziP0HHQswDTkQMygAegQIARBP",
                    }
                },
                maaltijd = Maaltijd.Avondmaaltijd,
                gereserveerd = null,
                warm = true,
                kantine = new Kantine
                {
                    Id = 1,
                    stad = Stad.Breda,
                    locatie = Locatie.LA,
                    naam = "The Fifth",
                    warm = true
                }
            };

            var result = _pakketService.UpdatePakket(newPakket);

            Assert.Equal("Pizza pepperoni", result.titel);
            Assert.Equal(4, result.Id);

        }
        [Fact]
        public void UpdatePakket_ThrowsException_WhenPickupIsAfterPickUpMax()
        {
            var _pakketRepoMock = new Mock<IPakketRepo>();
            var _pakketService = new PakketService(_pakketRepoMock.Object);

            var newPakket = new Pakket
            {
                Id = 4,
                titel = "Pizza pepperoni",
                prijs = 5.5,
                pickup = DateTime.Now.AddDays(1),
                pickUpMax = DateTime.Now,
                stad = Stad.Breda,
                volwassen = false,
                producten = new List<Product>
        {
            new Product
            {
                naam = "Brood",
                alcohol = false,
                foto = "https://www.google.com/imgres?imgurl=https%3A%2F%2Fmegafoodstunter.nl%2Fwp-content%2Fuploads%2F2023%2F08%2Fpistolet-wit.jpg&tbnid=jAMccVyusQl81M&vet=12ahUKEwiLgoK00eiBAxWziP0HHQswDTkQMygAegQIARBP..i&imgrefurl=https%3A%2F%2Fmegafoodstunter.nl%2Fwinkel%2Fbakkerij%2Fbrood%2Fpistolet-wit-75x70-gram%2F&docid=Mr_bEd8psnSF8M&w=1000&h=1000&q=pistolet&ved=2ahUKEwiLgoK00eiBAxWziP0HHQswDTkQMygAegQIARBP",
            }
        },
                maaltijd = Maaltijd.Avondmaaltijd,
                gereserveerd = null,
                warm = true,
                kantine = new Kantine
                {
                    Id = 1,
                    stad = Stad.Breda,
                    locatie = Locatie.LA,
                    naam = "The Fifth",
                    warm = true
                }
            };
            var exception = Assert.Throws<ArgumentException>(() => _pakketService.UpdatePakket(newPakket));

            Assert.Equal("De maximale ophaaltijd moet na de eerste ophaalmogelijkheid zijn!", exception.Message);
        }

        [Fact]
        public void UpdatePakket_ThrowsException_WhenPickupIsMoreThan2DaysLater()
        {
            var _pakketRepoMock = new Mock<IPakketRepo>();
            var _pakketService = new PakketService(_pakketRepoMock.Object);

            var newPakket = new Pakket
            {
                Id = 4,
                titel = "Pizza pepperoni",
                prijs = 5.5,
                pickup = DateTime.Now.AddDays(3),
                pickUpMax = DateTime.Now.AddDays(4),
                stad = Stad.Breda,
                volwassen = false,
                producten = new List<Product>
        {
            new Product
            {
                naam = "Brood",
                alcohol = false,
                foto = "https://www.google.com/imgres?imgurl=https%3A%2F%2Fmegafoodstunter.nl%2Fwp-content%2Fuploads%2F2023%2F08%2Fpistolet-wit.jpg&tbnid=jAMccVyusQl81M&vet=12ahUKEwiLgoK00eiBAxWziP0HHQswDTkQMygAegQIARBP..i&imgrefurl=https%3A%2F%2Fmegafoodstunter.nl%2Fwinkel%2Fbakkerij%2Fbrood%2Fpistolet-wit-75x70-gram%2F&docid=Mr_bEd8psnSF8M&w=1000&h=1000&q=pistolet&ved=2ahUKEwiLgoK00eiBAxWziP0HHQswDTkQMygAegQIARBP",
            }
        },
                maaltijd = Maaltijd.Avondmaaltijd,
                gereserveerd = null,
                warm = true,
                kantine = new Kantine
                {
                    Id = 1,
                    stad = Stad.Breda,
                    locatie = Locatie.LA,
                    naam = "The Fifth",
                    warm = true
                }
            };

            var exception = Assert.Throws<ArgumentException>(() => _pakketService.UpdatePakket(newPakket));

            Assert.Equal("Er mag geen pakket toegevoegd worden wat pas over 2 dagen op te halen is!", exception.Message);
        }
        [Fact]
        public void UpdatePakket_ThrowsException_WhenNoProducts()
        {
            var _pakketRepoMock = new Mock<IPakketRepo>();
            var _pakketService = new PakketService(_pakketRepoMock.Object);

            var newPakket = new Pakket
            {
                Id = 4,
                titel = "Pizza pepperoni",
                prijs = 5.5,
                pickup = DateTime.Now.AddDays(1),
                pickUpMax = DateTime.Now.AddDays(3),
                stad = Stad.Breda,
                volwassen = false,
                producten = null,
                maaltijd = Maaltijd.Avondmaaltijd,
                gereserveerd = null,
                warm = true,
                kantine = new Kantine
                {
                    Id = 1,
                    stad = Stad.Breda,
                    locatie = Locatie.LA,
                    naam = "The Fifth",
                    warm = true
                }
            };
            var exception = Assert.Throws<ArgumentException>(() => _pakketService.UpdatePakket(newPakket));

            Assert.Equal("Er moet minimaal een product toegevoeg worden aan een pakket!", exception.Message);
        }

    }
}