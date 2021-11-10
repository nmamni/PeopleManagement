using PM.Common.CommonModels;
using PM.Common.Exceptions;
using PM.Domain.BaseClasses;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PM.Domain.People
{
    public class Person : BaseEntity, IAggregateRoot
    {
        private const string LETTER_DUPPLICATION_PATTERN = "(^[a-zA-Z]+$|^[ა-ჰ]+$)";

        private List<RelatedPerson> _relatedPeople;
        private GenderTypes _gender;
        private string _personalNumber;
        private DateTime _birthDate;
        private string _city;
        private int? _cityID;
        private string _firstName;
        private string _lastName;
        private string _imageUrl;
        private PhoneNumber _phoneNumber;

        private Dictionary<string, Func<Result>> _validators;


        public Person(string firstName,
                        string lastName,
                        GenderTypes gender,
                        string personalNumber,
                        DateTime birthDate
            )
        {
            InitValidators();

            FirstName = firstName;
            Gender = gender;
            PersonalNumber = personalNumber;
            BirthDate = birthDate;
            LastName = lastName;

            _relatedPeople = new List<RelatedPerson>();
        }

        public void RemoveRelatedPerson(int id)
        {
            _relatedPeople.RemoveAll(p => p.RelationID == id);
        }

        public void AddRelatedPerson(Person rp, RelationTypes type)
        {
            var relPerson = new RelatedPerson
            {
                BirthDate = rp.BirthDate,
                City = rp.City,
                CityID = rp.CityID,
                FirstName = rp.FirstName,
                Gender = rp.Gender,
                ImageUrl = rp.ImageUrl,
                ID = rp.ID,
                LastName = rp.LastName,
                PersonalNumber = rp.PersonalNumber,
                PhoneNumber = rp.PhoneNumber.Number.Value,
                PhoneNumberType = rp.PhoneNumber.PhoneNumberType,
                RelationType = type
            };
            _relatedPeople.Add(relPerson);
            //TODO: VALIDATIOn
        }

        public void AddRelatedPerson(RelatedPerson relatedPerson)
        {
            _relatedPeople.Add(relatedPerson);
        }

        public void AddRelatedPeople(IEnumerable<RelatedPerson> relatedPeople)
        {
            foreach (var rp in relatedPeople)
                _relatedPeople.Add(rp);
        }

        private void Validate(params string[] fieldNames)
        {
            Result result;
            foreach (var fieldName in fieldNames)
            {
                result = _validators[fieldName]();
                if (!result.IsSuccess)
                    throw new LocalizableException(result.Message, result.Message);
            }
        }

        public IReadOnlyCollection<RelatedPerson> RelatedPeople { get => _relatedPeople; set => AddRelatedPeople(value); }

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                Validate("FirstName");
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                Validate("LastName");
            }
        }

        public GenderTypes Gender
        {
            get => _gender;
            set
            {
                _gender = value;
                Validate("Gender");
            }
        }

        public string PersonalNumber
        {
            get => _personalNumber;
            set
            {
                _personalNumber = value;
                Validate("PersonalNumber");
            }
        }

        public DateTime BirthDate
        {
            get => _birthDate;
            set
            {
                _birthDate = value;
                Validate("BirthDate");
            }
        }

        public string City
        {
            get => _city;
            set
            {
                _city = value;
            }
        }

        public int? CityID
        {
            get => _cityID;
            set
            {
                _cityID = value;
            }
        }

        public PhoneNumber PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                Validate("PhoneNumber");
            }
        }

        public string ImageUrl
        {
            get => _imageUrl;
            set
            {
                _imageUrl = value;
            }
        }

        private void InitValidators()
        {
            _validators = new Dictionary<string, Func<Result>>()
            {
                {"ID", () => {
                    if (ID < 0)
                         return new Result(-1, false, "ID_SHOULD_BE_POSITIVE");
                    return  Result.GetSuccessInstance();
                } },

                { "FirstName", () => {
                    if(string.IsNullOrEmpty(_firstName))
                       return new Result(-1, false, "FIRSTNAME_IS_EMPTY");
                    if(_firstName.Length < 2 || _firstName.Length > 50)
                        return new Result(-1, false, "FIRSTNAME_IS_INVALID_LENGTH");
                    if(!Regex.IsMatch(_firstName, LETTER_DUPPLICATION_PATTERN))
                       return new Result(-1, false, "FIRSTNAME_CONTAINS_MULTIPLE_LANGUAGE_LETTERS");
                    return  Result.GetSuccessInstance();
                }},

                 { "LastName", () => {
                    if(string.IsNullOrEmpty(_lastName))
                       return new Result(-1, false, "LASTNAME_IS_EMPTY");
                    if(_lastName.Length < 2 || _lastName.Length > 50)
                        return new Result(-1, false, "LASTNAME_IS_INVALID_LENGTH");
                    if(!Regex.IsMatch(_lastName, LETTER_DUPPLICATION_PATTERN))
                       return new Result(-1, false, "LASTNAME_CONTAINS_MULTIPLE_LANGUAGE_LETTERS");
                    return  Result.GetSuccessInstance();
                }},

                 { "Gender", () => {
                    if((int)_gender > 1)
                          return new Result(-1, false, "GENDER_IS_INVALID");
                    return  Result.GetSuccessInstance();
                }},

                  { "PersonalNumber", () => {
                    if(string.IsNullOrEmpty(_personalNumber))
                       return new Result(-1, false, "PERSONALNUMBER_IS_EMPTY");
                    if(_personalNumber.Length != 11)
                        return new Result(-1, false, "PERSONALNUMBER_IS_INVALID_LENGTH");
                    return  Result.GetSuccessInstance();
                }},

                  { "BirthDate", () => {
                    if(_birthDate == DateTime.MinValue)
                       return new Result(-1, false, "BIRTHDATE_IS_INVALID");
                    var age = DateTime.Today.Year - _birthDate.Year;
                    if (_birthDate.Date > DateTime.Today.AddYears(-age)) age--;
                    if(age < 18)
                        return new Result(-1, false, "AGE_IS_INVALID");
                    return  Result.GetSuccessInstance();
                }},

                  { "PhoneNumber", () => {
                    if(_phoneNumber == null || _phoneNumber.Number == null || _phoneNumber.Number.Value == null)
                       return  Result.GetSuccessInstance();
                    if(_phoneNumber.Number.Value.Length < 4 || _phoneNumber.Number.Value.Length > 50)
                           return new Result(-1, false, "PHONENUMBER_IS_INVALID_LENGTH");
                    return  Result.GetSuccessInstance();
                }},
            };
        }

    }
}
