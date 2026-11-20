
using Taxiturne.Model;
using Taxiturne.Model.Interfaces;
using Taxiturne.Services;

namespace Taxiturne.Tests
{
    public class AuthentificationServiceTests
    {
        private readonly Mock<IUserDAL> _userDAL;
        private readonly AuthentificationService _service;

        public AuthentificationServiceTests()
        {
            _userDAL = new Mock<IUserDAL>(MockBehavior.Strict);

            _service = new AuthentificationService(
                _userDAL.Object);
        }

        [Theory]
        [InlineData(null, "password123")]
        [InlineData("", "password123")]
        [InlineData("   ", "password123")]
        [InlineData("mtremblay", null)]
        [InlineData("mtremblay", "")]
        [InlineData("mtremblay", "   ")]
        [InlineData("m", "password123")]
        public void Authentifier_ChampsInvalides_RefuseSansConsulterDAL(
            string? username,
            string? password)
        {
            var resultat = _service.Authentifier(
                username, password);

            Assert.Null(resultat);

            _userDAL.Verify(
                d => d.FindByUsernameAndPassword(
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public void Authentifier_IdentifiantsValides_RetourneUtilisateur()
        {
            var utilisateur = new User
            {
                Id = 1,
                Prenom = "Marc",
                Nom = "Tremblay",
                Username = "mtremblay",
                Password = "password123",
                Role = "Repartiteur"
            };

            _userDAL.Setup(d =>
                    d.FindByUsernameAndPassword(
                        "mtremblay",
                        "password123"))
                .Returns(utilisateur);

            var resultat = _service.Authentifier(
                "mtremblay",
                "password123");

            Assert.NotNull(resultat);
            Assert.Same(utilisateur, resultat);
            Assert.Equal("Repartiteur", resultat.Role);

            _userDAL.Verify(
                d => d.FindByUsernameAndPassword(
                    "mtremblay",
                    "password123"),
                Times.Once);
        }

        [Theory]
        [InlineData("mtremblay", "mauvaisMotDePasse")]
        [InlineData("utilisateurInconnu", "password123")]
        [InlineData("inconnu", "mauvaisMotDePasse")]
        public void Authentifier_IdentifiantsInvalides_RetourneNull(
            string username,
            string password)
        {
            _userDAL.Setup(d =>
                    d.FindByUsernameAndPassword(
                        username,
                        password))
                .Returns((User?)null);

            var resultat = _service.Authentifier(
                username,
                password);

            Assert.Null(resultat);

            _userDAL.Verify(
                d => d.FindByUsernameAndPassword(
                    username,
                    password),
                Times.Once);
        }
    }
}
