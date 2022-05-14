using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Cliente.Models;
using ClosedXML.Excel;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Cliente.Controllers
{
    public class EmpleadoController : Controller
    {
        string URL = "http://localhost:60608/";
        // GET: Empleado


        //Muestra un listado de los empleados
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            List<Empleado> lista = new List<Empleado>();
            using (var empleado = new HttpClient()) {
                empleado.BaseAddress = new Uri(URL);
                empleado.DefaultRequestHeaders.Clear();
                empleado.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage respuesta = await empleado.GetAsync("api/Empleado");
                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = respuesta.Content.ReadAsStringAsync().Result;
                    lista = JsonConvert.DeserializeObject<List<Empleado>>(contenido);
                   

                }
            }

            return View(lista);

          
        }

        public ActionResult Create()
        {
            return View();
        }

        //Consume el servicio del web api para la creación de empleado nuevo
        [HttpPost]
        public ActionResult Create(Empleado datos)
        { 
            using(var empleado = new HttpClient())
            {
                empleado.BaseAddress = new Uri("http://localhost:60608/api/Empleado");
                var nuevo = empleado.PostAsJsonAsync<Empleado>("empleado", datos);
                nuevo.Wait();
                var resultado = nuevo.Result;
                if (resultado.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError(string.Empty, "No se logro ingresar empleado");
            return View(datos);
        }

        //Metodo para mostrar la información de la consulta, en este caso de la edición del registro

        [Route("Edit/{id:int}")]

        public ActionResult Edit([Bind(Exclude = "Id")] int id)
        {

            Empleado emp = null;

            using (var empleado = new HttpClient())
            {
                empleado.BaseAddress = new Uri("http://localhost:60608/");
                var responseTask = empleado.GetAsync("api/Empleado/" + id.ToString());
                responseTask.Wait();
                var result = responseTask.Result;
                // var consulta = empleado.GetAsync("api/Empleado/" + id.ToString());
                // consulta.Wait();
                // var resultado = consulta.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Empleado>();
                    readTask.Wait();
                    emp = readTask.Result;

                }

            }
            return View(emp);

        }



       
        [HttpPost]
        public ActionResult Edit(Empleado datos)
        {
            using (var empleado = new HttpClient())
            {
                empleado.BaseAddress = new Uri("http://localhost:60608/");
                var actualizar = empleado.PutAsJsonAsync($"api/Empleado/{datos.Id_empleado}", datos);
                actualizar.Wait();
                var resultado = actualizar.Result;
                if (resultado.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(datos);
        }


       
        //Metodo para mostrar registro 


        public ActionResult Ingreso(int id)
        {

            Empleado emp = null;
            using (var empleado = new HttpClient())
            {
                empleado.BaseAddress = new Uri("http://localhost:60608/");
                var responseTask = empleado.GetAsync("api/Empleado/" + id.ToString());
                responseTask.Wait();
                var result = responseTask.Result;
                //var consulta = empleado.GetAsync("api/Empleado/" + id.ToString());
                //consulta.Wait();
                //var resultado = consulta.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Empleado>();
                    readTask.Wait();
                    emp = readTask.Result;

                }

            }
            return View(emp);

        }

        //Consume web api para borrado logico

        [HttpPost]
        public ActionResult Ingreso(Empleado datos)
        {
            using (var empleado = new HttpClient())
            {
                empleado.BaseAddress = new Uri("http://localhost:60608/");
                var eliminar = empleado.PutAsJsonAsync($"api/Empleado/{datos.Id_empleado}", datos);
                eliminar.Wait();
                var resultado = eliminar.Result;
                if (resultado.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(datos);
        }




        //Metodo de mostrar la información de un registro 

        public ActionResult Delete(int id)
        {

            Empleado emp = null;
            using (var empleado = new HttpClient())
            {
                empleado.BaseAddress = new Uri("http://localhost:60608/");
                var responseTask = empleado.GetAsync("api/Empleado/" + id.ToString());
                responseTask.Wait();
                var result = responseTask.Result;
                //var consulta = empleado.GetAsync("api/Empleado/" + id.ToString());
                //consulta.Wait();
                //var resultado = consulta.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Empleado>();
                    readTask.Wait();
                    emp = readTask.Result;

                }

            }
            return View(emp);

        }

        //Metodo de eliminar registro 

        [HttpPost]
        public ActionResult Delete(Empleado datos, int id)
        {
            using (var empleado = new HttpClient())
            {
                empleado.BaseAddress = new Uri("http://localhost:60608/");
                var eliminar = empleado.DeleteAsync($"api/Empleado/" + id.ToString());
                eliminar.Wait();
                var resultado = eliminar.Result;
                if (resultado.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(datos);
        }


        /// 

        //
        [HttpGet]
        public async Task<FileResult> Exportar(string Id_empleado)

        {
           // Id_empleado = 1;
            DataTable dt = new DataTable();
          
                List<Empleado> lista = new List<Empleado>();
                using (var empleado = new HttpClient())
                {
                    empleado.BaseAddress = new Uri(URL);
                    empleado.DefaultRequestHeaders.Clear();
                    empleado.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage respuesta = await empleado.GetAsync("api/Empleado");
                    if (respuesta.IsSuccessStatusCode)
                    {
                        var contenido = respuesta.Content.ReadAsStringAsync().Result;
                        lista = JsonConvert.DeserializeObject<List<Empleado>>(contenido);

                    }
               
                   // lista.Fill(dt);
                    dt.TableName = "Datos";

                    using (XLWorkbook libro = new XLWorkbook()) // variable para crear libro con CLOSEDXML
                    {
                        var hoja = libro.Worksheets.Add(dt);
                        hoja.ColumnsUsed().AdjustToContents();

                        using (MemoryStream stream = new MemoryStream()) //donde se almacenara
                        {
                            libro.SaveAs(stream);
                            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reporte " + DateTime.Now.ToString() + ".xlsx");
                        }
                    }
               
          
                 
                }

            
        }

        


    }


}



