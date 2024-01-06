using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;

namespace DALLayer.Repostitory
{
    public class PatientRepository //class name
    {
        private readonly ClinicalDbContext _Db; //define database class object
                                                // declares a private, read-only field named _Db of type ClinicalDbContext
        public PatientRepository() 
        {
            _Db = new ClinicalDbContext(); //intialize database object 
        }

        public IEnumerable<Patient> GetAllPatients()
        //IEnumerable is an interface in C# that represents a collection of objects that can be enumerated (iterated) one at a time.
       //<Patient> specifies the type of elements in the collection.In this case, the collection contains elements of type Patient.
        {
            return _Db.Patients.ToList(); //used to get all data of patients;
        }
        
                                 //int   b 
                    //methodname  class object name
        public void InsertPatient(Patient patient)
        {
            _Db.Patients.Add(patient);
            Save();
        }
        public void UpdatePatient(Patient patient)
        {
            _Db.Entry(patient).State = EntityState.Modified;
            Save();
        }
        public bool checkPatientLogin(Patient patient)
        {
            return _Db.Patients.Any(d => d.Email == patient.Email && d.Password == patient.Password);
        }
        public void DeletePatient(int id)
        {
            Patient found = _Db.Patients.Find(id);
            if (found != null)
            {
                _Db.Patients.Remove(found);
                Save();
            }
        }
        public void Save()
        {
            _Db.SaveChanges();
        }
        public Patient FindPatientById(int id)
        {
            return _Db.Patients.Find(id); // select * from patients where patientId=id;
        }
        public Patient FindPatientByEmail(string Email)
        {
            return _Db.Patients.FirstOrDefault(pro => pro.Email == Email); //select * from patients where patientEmail=Email;
        }
              //datatype
        public IEnumerable<Patient> FindPatientWithName(string Name)
        {
            return _Db.Patients.Where(meds => meds.Name.ToLower().Contains(Name.ToLower()));
        }   //select * from patients where lower(patientname)=lower(name);
    }
}
