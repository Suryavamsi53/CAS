﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using DALLayer;
using CASServiceLayer.Models;

namespace CASUILayer.Controllers
{
    public class PatientsController : Controller
    {
        //private ClinicalDbContext db = new ClinicalDbContext();
        private readonly ServiceOperations service;
        
        public PatientsController()
        {
            service=new ServiceOperations();
        }
       
        // GET: Patients
        public ActionResult Index()
        {
            if (Session["SId"] == null)
            {
                return RedirectToAction("PatientLogin", "Home");
            }
            return View(service.GetAllDoctors());
        }
     
        public ActionResult BookAppoint(int id)
        { 
            Doctor d = service.GetDoctorById(id);
            //TempData["DoctorName"] = d.DoctorName;
            //TempData["Timings"] = d.Timings;
            //TempData["SpecializationName"] = d.Specialization.SpecializationName;
            //TempData["DocId"] = id;
            Session["DoctorName"] = d.DoctorName;
            Session["Timings"] = d.Timings;
            Session["SpecializationName"] = d.Specialization.SpecializationName;
            Session["DocId"] = id;

            return View();//ViewBag() passes data from Action to view 
        }

        [HttpPost]
        public ActionResult BookAppoint(Appointment appointment)
        {
            if (Session["SId"] == null)
            {
                return RedirectToAction("PatientLogin", "Home");
            }
      
            if (appointment.StartDateTime.Date >= DateTime.Now.Date)
            {

                var obj = Session["PatientObj"] as Patient;

                appointment.Status = "Pending";
                appointment.MsgLimit = 0;
                appointment.IsApprove = false;
                appointment.DoctorId = (int)Session["DocId"];
                appointment.PatientId = obj.PatientId;
                service.AddAppointment(appointment);
                return RedirectToAction("ViewAppointments");
            }
            else
            {
                ModelState.AddModelError("StartDateTime", "Appointment date must be in the present/future.");
                return View();
            }
        }
        //ViewAppointment

        public ActionResult ViewAppointments()
        {
            if (Session["SId"] == null)
            {
                return RedirectToAction("PatientLogin", "Home");
            }
            var obj = Session["PatientObj"] as Patient;
            List<Appointment> appointment = service.GetAppointmentsByPatientId(obj.PatientId);
            return View(appointment);
        }

        public ActionResult MessageDoctor()
        {
            if (Session["SId"] == null)
            {
                return RedirectToAction("PatientLogin", "Home");
            }

            var obj = Session["PatientObj"] as Patient;
            var appointments = service.SearchbyPatientIdApproved(obj.PatientId);
            ViewBag.Appointments = appointments;
            Session["PatientId"]=obj.PatientId;
            //TempData["MsgLimit"]=obj.MsgLimit;
            TempData["id"]  =obj.PatientId;
            
            return View();
        }

        [HttpPost]
        public ActionResult MessageDoctor(int id)
        {

                var obj = Session["PatientObj"] as Patient;
                Doctor doctor = service.GetDoctorById(id);
                Session["DocId"] = doctor.DoctorId;
                Session["Dname"] = doctor.DoctorName;
                IEnumerable<Message> messages = service.GetBySenderIdAndRecieverId(obj.PatientId, id);
                var appointments = service.SearchbyPatientIdApproved(obj.PatientId);
                ViewBag.Appointments = appointments;
                return View(messages);
            
        }


      


        [HttpPost]
        public ActionResult AddMessage(int id, string txtMessage)
        {
            var obj = Session["PatientObj"] as Patient;
            var apt = service.GetAppointmentByDocPat(id, obj.PatientId);
            if (apt.MsgLimit < 2)
            {
                apt.MsgLimit++;
                service.UpdateAppointment(apt);

                Message message = new Message();
                message.SenderId = (int)TempData["id"];
                message.MessageTime = DateTime.Now;
                message.ReceiverId = id;
                message.Status = "Sent";
                message.Message1 = txtMessage;
                service.AddMessage(message);

                return RedirectToAction("MessageDoctor", new { id = message.ReceiverId });
            }
            else
            {
                ModelState.AddModelError("", "Limit Exceeded");

                // Retrieve necessary data for the view
                IEnumerable<Message> messages = service.GetBySenderIdAndRecieverId(obj.PatientId, id);

                // Pass the error message and data to the view
                TempData["ErrorMessage"] = "Message limit exceeded. You cannot ask more than 2 Queries.";
                TempData["Messages"] = messages;

                return RedirectToAction("MessageDoctor");
            }
        } 


        public ActionResult ProfileChange()
        {
            if (Session["SId"] == null)
            {
                return RedirectToAction("PatientLogin", "Home");
            }
            var obj = Session["PatientObj"] as Patient;
            return View(obj);
        }

        [HttpPost]
        public ActionResult ProfileChange([Bind(Include = "PatientId,Name,Phone,Address,DOB,Gender,Email,Password")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                service.UpdatePatient(patient);
                return RedirectToAction("Index");
            }
            return View();
        }


        public ActionResult ViewMedicine()
        {
            if (Session["SId"] == null)
            {
                return RedirectToAction("PatientLogin", "Home");
            }
            List<Medicine> MedList =service.GetAllMedicines();
            foreach(var item in MedList)
            {
                item.Price=service.CalculateDiscountedPrice(item.Price,item.Tax);
            }
            return View(MedList);
        }

        [HttpPost]
        public ActionResult ViewMedicine(string MediName)
        {
            IEnumerable<Medicine> result = service.FindMedicineByName(MediName);
            return View(result);
        }






    }
}
