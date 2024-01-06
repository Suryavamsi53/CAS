﻿using Antlr.Runtime.Misc;
using CASServiceLayer.Models;
using DALLayer;
using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.UI.WebControls.WebParts;
using static System.Collections.Specialized.BitVector32;

namespace CASUILayer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ServiceOperations service; //service object define

        public HomeController()
        {
            service = new ServiceOperations();//object create
        }

        public ActionResult Index()//landing page
        {
            return View();            //view is Html code
        }

        public ActionResult AdminLogin()  
        {                                  
            Session["SId"] = 1;            //Session  id  for  admin  =1, Sid is the variable name used to store the id
            return View();                //Admin login page
        }

        [HttpPost]//whenever we call the button in form it call the post method
        public ActionResult AdminLogin(Admin admin)
        {
            if (ModelState.IsValid)
            {
                if (service.checkAdminLogin(admin)) //checks the admin details are vaild or not
                {
                    Admin ad = service.FindAdminByEmail(admin.EmailId);  //used to find the admin id details by using admin emailid
                    Session["AdminObject"] = ad; // stores the admin object in session to used the data of the admin in the view,used in profile change

                    return RedirectToAction("AdminIndex", "Admins");// return the views to the AdminIndex view of Admins controller 
                }
                else
                {
                    ModelState.AddModelError("", "InCorrect Email ID and Password");
                    return View();    //Admin login
                }
            }
            return View();
        }

        public ActionResult DoctorLogin()
        {
            Session["SId"] = 2;
            return View();
        }

        [HttpPost]
        public ActionResult DoctorLogin(Doctor doctor)
        {
            if (doctor.Email!=null && doctor.Password!=null)
            {
                if (service.checkDoctorLogin(doctor))
                {
                    Doctor doc = service.FindDoctorByEmail(doctor.Email);
                    Session["DocObject"] = doc;
                    return RedirectToAction("AppointmentList", "Doctors");
                }
                else
                {
                    ModelState.AddModelError("", "InCorrect Email ID and Password");
                    return View();
                }
            }
            return View();
         }

        public ActionResult PatientLogin()
        {
            Session["SId"] = 3;
            return View();
        }

        [HttpPost]
        public ActionResult PatientLogin(Patient patient)
            //ActionResult is a Return type which represents a View, Redirection or any other relative responces
            //Patient patient: This is a parameter of type Patient.It implies that this action method expects data from the client in the form of a Patient object. The data is likely sent as part of an HTTP request (e.g., a form submission).
        {
            if (patient.Email != null && patient.Password != null)
            {
                if (service.checkPatientLogin(patient))
                 {
                    Patient patient1 = service.FindPatientByEmail(patient.Email);
                    Session["PatientObj"] = patient1;
                    return RedirectToAction("Index", "Patients");
                }
                else
                {
                    ModelState.AddModelError("", "InCorrect Email ID and Password");
                    return View();
                }
            }
            return View();
        }

        public ActionResult PatientSignUp() 
        { 
            return View(); 
        }


        [HttpPost]
        public ActionResult PatientSignUp(Patient patient)
        {
            //int age = DateTime.Now.Year - patient.DOB.Year;
            //04<03--false 
            if (DateTime.Now < patient.DOB)
            {
                ModelState.AddModelError("DOB", "Please select a valid date of birth.");
                return View();
            }
            else if (ModelState.IsValid)
            {
               service.InsertPatient(patient);
               return RedirectToAction("PatientLogin");
             }
             else
              {
                ModelState.AddModelError("", "InCorrect Email ID and Password");
                 return View();
              }
        }

        public ActionResult FrontOfficeLogin()
        {
            Session["SId"] = 4;
        
            return View();
        }

        [HttpPost]
        public ActionResult FrontOfficeLogin(FrontOfficeExecutive frontOffice)
        {
            if (frontOffice.Email != null && frontOffice.Password != null)
            {
                if (service.checkFOLogin(frontOffice))
                {
                    FrontOfficeExecutive FE1 = service.FindFrontOfficeByEmail(frontOffice.Email);
                    Session["FOObject"] = FE1;
                    return RedirectToAction("ViewAppointment", "FrontOfficeExecutives");  
                }
                else
                {
                    ModelState.AddModelError("", "InCorrect Email ID and Password");
                    return View();
                }
            }
            return View();
        }

        public ActionResult PharmacistsLogin()
        {
            Session["SId"] = 5;
            return View();
        }

        [HttpPost]
        public ActionResult PharmacistsLogin(Pharmacist pharmacist)
        {
            if (pharmacist.Email != null && pharmacist.Password != null)
            {

                if (service.checkPharmacistLogin(pharmacist))
                {
                    Pharmacist Ph1 = service.FindPharmacistByEmail(pharmacist.Email);
                    Session["PhObject"] = Ph1; //This line stores the Ph1(the pharmacist object) in the session state.
                                                //Session state is a way to persist data across multiple requests for the same user.Here, it seems to be storing the pharmacist information for later use.
                    return RedirectToAction("Index", "Pharmacists");
                }
                else
                {
                    ModelState.AddModelError("", "InCorrect Email ID and Password");
                    return View();
                }
            }
            return View();
        }

        public ActionResult ResetPass()
        {
            return View();
        }

        [HttpPost]//get the values from the resetform page,(email,pass1,pass2,session id of the specific login page)
        public ActionResult ResetPass(string email,string pass1,string pass2,int SId)
        {
            switch (SId) //go to specific case 
            {
                case 1:
                    if (service.Checkpass(pass1, pass2)) //validate the 2 password are same using check pass method in service class
                    {
                        Admin ad = service.FindAdminByEmail(email); //find the email id is found or not, if not found it returns null
                        if (ad == null)
                        {
                            ModelState.AddModelError("", "Email id not found");
                            return View();
                        }
                        ad.Password = pass1;
                        service.UpdateAdmin(ad);
                        return RedirectToAction("AdminLogin");
                    }
                  
                    break;
                case 2:
                    if (service.Checkpass(pass1, pass2))
                    {
                        Doctor doc = service.FindDoctorByEmail(email);
                        if(doc== null)
                        {
                            ModelState.AddModelError("", "Email id not found");
                            return View();
                        }
                        doc.Password = pass1;
                        service.UpdateDoctor(doc);
                        return RedirectToAction("DoctorLogin");
                    }
                    break;
                case 3:
                    if (service.Checkpass(pass1, pass2))
                    {
                        
                        Patient patient = service.FindPatientByEmail(email);
                        if (patient == null)
                        {
                            ModelState.AddModelError("", "Email id not found");
                            return View();
                        }
                        patient.Password = pass1;
                        service.UpdatePatient(patient);
                        return RedirectToAction("PatientLogin");
                    }
                    break;
                case 4:
                   
                        FrontOfficeExecutive Fo = service.FindFrontOfficeByEmail(email);
                        if (Fo == null)
                        {
                            ModelState.AddModelError("", "Email id not found");
                            return View();
                        }
                        Fo.Password = pass1;
                        service.UpdateFrontOfficeExecutive(Fo);
                        return RedirectToAction("FrontOfficeLogin");
                        
                case 5:
                    if (service.Checkpass(pass1, pass2))
                    {
                        Pharmacist ph = service.FindPharmacistByEmail(email);
                        if (ph == null)
                        {
                            ModelState.AddModelError("", "Email id not found");
                            return View();
                        }
                        ph.Password = pass1;
                        service.UpdatePharmacist(ph);
                        return RedirectToAction("PharmacistsLogin");
                    }
                    break;
                default:
                    break;
            }

            return View();
        }

      /*  public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }*/

        public ActionResult LogOut() 
        {
            Session.Clear();//clear the data which is present in the session.
            return RedirectToAction("Index","Home");
         }
    }
}