using EnterpriseApps.Portable.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseApps.Portable.Tests
{
    [TestClass]
    public class MappingServiceTests
    {
        private const string _firstName = "Philipp";
        private const string _lastName = "Bauknecht";
        private const string _cell = "+491743148676";
        private const string _phone = "+4972311332580";
        private const string _email = "bauknecht@medialesson.de";
        private const string _thumnailUrl = "philipp_thumb.jpg";
        private const string _pictureUrl = "philipp_large.jpg";

        [TestMethod]
        public void TestMapUserWithValidDTO()
        {
            // Arrange
            var po = new PrivateObject(new MappingService());
            var dto = new DTO.Result()
            {
                User = new DTO.User()
                {
                    Name = new DTO.Name()
                    {
                        First = _firstName,
                        Last = _lastName
                    },
                    Cell = _cell,
                    Phone = _phone,
                    Email = _email,
                    Picture = new DTO.Picture()
                    {
                        Thumbnail = _thumnailUrl,
                        Large = _pictureUrl
                    }
                }
            };

            // Act
            var model = (Model.User)po.Invoke("MapUser", dto);

            // Assert
            Assert.AreEqual(_firstName, model.FirstName);
            Assert.AreEqual(_lastName, model.LastName);
            Assert.AreEqual(_cell, model.Cell);
            Assert.AreEqual(_phone, model.Phone);
            Assert.AreEqual(_email, model.Email);
            Assert.AreEqual(_thumnailUrl, model.ThumbnailUrl);
            Assert.AreEqual(_pictureUrl, model.PictureUrl);
        }

        [TestMethod]
        public void TestMapUserWithEmptyResult()
        {
            // Arrange
            var po = new PrivateObject(new MappingService());
            var dto = new DTO.Result();

            // Act
            var model = (Model.User)po.Invoke("MapUser", dto);

            // Assert
            Assert.IsNull(model);
        }

        [TestMethod]
        public void TestMapUserWithEmptyUser()
        {
            // Arrange
            var po = new PrivateObject(new MappingService());
            var dto = new DTO.Result()
            {
                User = new DTO.User()
            };

            // Act
            var model = (Model.User)po.Invoke("MapUser", dto);

            // Assert
            Assert.IsNotNull(model);
        }

        [TestMethod]
        public void TestMapUserWithoutName()
        {
            // Arrange
            var po = new PrivateObject(new MappingService());
            var dto = new DTO.Result()
            {
                User = new DTO.User()
                {
                    Cell = _cell,
                    Phone = _phone,
                    Email = _email,
                    Picture = new DTO.Picture()
                    {
                        Thumbnail = _thumnailUrl
                    }
                }
            };

            // Act
            var model = (Model.User)po.Invoke("MapUser", dto);

            // Assert
            Assert.IsNotNull(model);
        }

        [TestMethod]
        public void TestMapUserWithoutPicture()
        {
            // Arrange
            var po = new PrivateObject(new MappingService());
            var dto = new DTO.Result()
            {
                User = new DTO.User()
                {
                    Name = new DTO.Name()
                    {
                        First = _firstName,
                        Last = _lastName
                    },
                    Cell = _cell,
                    Phone = _phone,
                    Email = _email
                }
            };

            // Act
            var model = (Model.User)po.Invoke("MapUser", dto);

            // Assert
            Assert.IsNotNull(model);
        }

        [TestMethod]
        public void TestMapUsersWithOneValidDTO()
        {
            // Arrange
            var mappingService = new MappingService();
            var dto = new DTO.Result()
            {
                User = new DTO.User()
                {
                    Name = new DTO.Name()
                    {
                        First = _firstName,
                        Last = _lastName
                    },
                    Cell = _cell,
                    Phone = _phone,
                    Email = _email,
                    Picture = new DTO.Picture()
                    {
                        Thumbnail = _thumnailUrl
                    }
                }
            };
            var dtos = new List<DTO.Result>() { dto };

            // Act
            var models = mappingService.MapUsers(dtos);

            // Assert
            Assert.AreEqual(1, models.Count());
        }
    }
}
