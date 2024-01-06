using CASTesting;
using DALLayer;
using NUnit.Framework;

namespace CASTesting
{
    /*is a container for a set of related test cases,
    providing a way to group them together.
    Test fixtures help organize and structure your tests */
    // This class represents a set of unit tests for the Patient class.
    [TestFixture]
    public class PatientTest
    {
        // Test case: Name property should be required.
        [Test]
        public void NameShouldBeRequired()
        {
            // Arrange: Create a patient object with a null Name.
            var patient = new Patient { Name = null };

            // Act and Assert: Validate the model and expect an exception to be thrown.
            //verifys certain conditions Assert.That(())
            Assert.That(() => ValidateModel(patient), Throws.Exception);
        }

        // Test case: Phone property should be required.
        [Test]
        public void PhoneShouldBeRequired()
        {
            // Arrange: Create a patient object with an empty or invalid Phone.
            var patient = new Patient { Phone = "0" };

            // Act and Assert: Validate the model and expect an exception to be thrown.
            Assert.That(() => ValidateModel(patient), Throws.Exception);
        }

        // Test case: Address property should be required.
        [Test]
        public void AddressShouldBeRequired()
        {
            // Arrange: Create a patient object with a null Address.
            var patient = new Patient { Address = null };

            // Act and Assert: Validate the model and expect an exception to be thrown.
            Assert.That(() => ValidateModel(patient), Throws.Exception);
        }

        // Test case: DOB (Date of Birth) property should be required.
        [Test]
        public void DOBShouldBeRequired()
        {
            // Arrange: Create a patient object with a default (invalid) DOB value.
            var patient = new Patient { DOB = DateTime.MinValue };

            // Act and Assert: Validate the model and expect an exception to be thrown.
            Assert.That(() => ValidateModel(patient), Throws.Exception);
        }

        // Test case: Gender property should be required.
        [Test]
        public void GenderShouldBeRequired()
        {
            // Arrange: Create a patient object with a null Gender.
            var patient = new Patient { Gender = null };

            // Act and Assert: Validate the model and expect an exception to be thrown.
            Assert.That(() => ValidateModel(patient), Throws.Exception);
        }

        // Test case: Email property should be required.
        [Test]
        public void EmailShouldBeRequired()
        {
            // Arrange: Create a patient object with a null Email.
            var patient = new Patient { Email = null };

            // Act and Assert: Validate the model and expect an exception to be thrown.
            Assert.That(() => ValidateModel(patient), Throws.Exception);
        }

        // Test case: Password property should be required.
        [Test]
        public void PasswordShouldBeRequired()
        {
            // Arrange: Create a patient object with a null Password.
            var patient = new Patient { Password = null };

            // Act and Assert: Validate the model and expect an exception to be thrown.
            Assert.That(() => ValidateModel(patient), Throws.Exception);
        }

        // You can add more test cases for other properties and validations...

        // Helper method to perform model validation using DataAnnotations.
        private void ValidateModel(Patient patient)
        {
            // Create a validation context and a list to store validation results.
            var context = new System.ComponentModel.DataAnnotations.ValidationContext(patient, null, null);
            var results = new System.Collections.Generic.List<System.ComponentModel.DataAnnotations.ValidationResult>();

            // Perform model validation and check if it is valid.
            bool isValid = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(patient, context, results, true);

            // If not valid, throw a validation exception with the error message.
            if (!isValid)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException(results[0].ErrorMessage);
            }
        }
    }
}
