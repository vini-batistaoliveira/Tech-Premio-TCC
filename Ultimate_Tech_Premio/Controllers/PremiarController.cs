using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ultimate_Tech_Premio.Models;

namespace Ultimate_Tech_Premio.Controllers
{
    public class PremiarController : Controller
    {
        private tech_premioEntities2 db = new tech_premioEntities2();

        public DateTime? start;
        public DateTime? end;
        public string sortOrder;

        // GET: Premiar
        public ActionResult Index(DateTime? start, DateTime? end, int? sortOrder)
        {
            if (start != null && end != null)
            {
                List<Resultado> lista = db.Chamado.Where(c => c.data_chamado >= start)
                    .Where(c => c.data_chamado <= end).GroupBy(c => c.Tecnico1)
                    .Select(obj => new Resultado
                    {
                        tecnico_id = obj.FirstOrDefault().Tecnico1,
                        tecnico_nome = obj.FirstOrDefault().Tecnico_nome,
                        soma = obj.Sum(x => x.nota_avaliacao) +
                        obj.Sum(x => x.habilidade_avaliacao) +
                        obj.Sum(x => x.cordialidade_avaliacao) +
                        obj.Sum(x => x.solucionado_nota)
                    }).ToList();

                for (var i = 0; i < lista.Count(); i++)
                {
                    var ids = lista[i].tecnico_id;
                    var user = db.Usuario.Find(ids);
                    lista[i].tecnico_nome = user.nome;
                }

                if (sortOrder == 2)
                {
                    var asc = lista.OrderBy(s => s.soma);
                    return View(asc.ToList());
                }
                else
                {
                    var desc = lista.OrderByDescending(s => s.soma);
                    return View(desc.ToList());
                }
            }

            return View();
        }

    }
}