using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projet1_ASP.Models
{
    public class SearchModelUser
    {
        SiteContext db = new SiteContext();


        public List<List<Etudiant>> liste = new List<List<Etudiant>>();
        public List<string> listn = new List<string>();
        public List<string> listf = new List<string>();
        public List<string> listt = new List<string>();
        public List<string> lists = new List<string>();
        public List<string> listd = new List<string>();
        public List<string> listdt = new List<string>();
        public List<int> listr = new List<int>();
        public List<bool> listbool = new List<bool>();
        public List<string> listenc = new List<string>();

        public SearchModelUser()
        {
            var x = (from g in db.groupes
                     join t in db.types on g.id_tp equals t.id_type
                     join ed in db.encadrants on g.id_enc equals ed.Id
                     select new
                     {
                         type = t.nom_type,
                         grps = g.grp_id,
                         enc = ed
                     });

            foreach (var i in x)
            {
                listenc.Add(i.enc.nom+" "+i.enc.prenom);
                listt.Add(i.type);
                listr.Add(i.grps);
                File f = db.files.Where(p => p.groupe_Id == i.grps).SingleOrDefault();
                if (f != null)
                {
                    lists.Add(f.sujet);
                    listdt.Add(f.date_disp);
                    listd.Add(f.description);
                    listbool.Add(true);
                }
                else
                {
                    lists.Add("");
                    listdt.Add("");
                    listd.Add("");
                    listbool.Add(false);
                }
                List<Etudiant> listet = new List<Etudiant>();
                var y = (from e in db.etudiants
                         join m in db.GroupeMembres on e.cne equals m.id_et
                         where m.id_grp == i.grps
                         select new
                         {
                             etudiant = e,
                         });
                foreach (var j in y)
                {
                    listet.Add(j.etudiant);
                }
                liste.Add(listet);
                listf.Add(listet[0].Filiere.Nom_filiere);
                listn.Add(listet[0].Niveau.nom_Niveau);
            }
        }

        internal void searchBy(string rech, string word)
        {
            if (rech.Equals("sujet"))
            {
                for (int i = 0; i < lists.Count; i++)
                {
                    if (!(lists[i].ToLower().Contains(word.ToLower())))
                    {
                        lists.RemoveAt(i);
                        listd.RemoveAt(i);
                        listt.RemoveAt(i);
                        listdt.RemoveAt(i);
                        liste.RemoveAt(i);
                        listr.RemoveAt(i);
                        listn.RemoveAt(i);
                        listf.RemoveAt(i);
                        listbool.RemoveAt(i);
                        listenc.RemoveAt(i);
                        i--;
                    }
                }
            }
            else if (rech.Equals("description"))
            {
                for (int i = 0; i < lists.Count; i++)
                {
                    if (!(listd[i].ToLower().Contains(word.ToLower())))
                    {
                        lists.RemoveAt(i);
                        listd.RemoveAt(i);
                        listt.RemoveAt(i);
                        listdt.RemoveAt(i);
                        liste.RemoveAt(i);
                        listr.RemoveAt(i);
                        listn.RemoveAt(i);
                        listf.RemoveAt(i);
                        listbool.RemoveAt(i);
                        listenc.RemoveAt(i);
                        i--;
                    }
                }
            }
            else if (rech.Equals("type"))
            {
                for (int i = 0; i < lists.Count; i++)
                {
                    if (!(listt[i].ToLower().Contains(word.ToLower())))
                    {
                        lists.RemoveAt(i);
                        listd.RemoveAt(i);
                        listt.RemoveAt(i);
                        listdt.RemoveAt(i);
                        liste.RemoveAt(i);
                        listr.RemoveAt(i);
                        listn.RemoveAt(i);
                        listf.RemoveAt(i);
                        listbool.RemoveAt(i);
                        listenc.RemoveAt(i);
                        i--;
                    }
                }
            }
            else if (rech.Equals("filiere"))
            {
                for (int i = 0; i < lists.Count; i++)
                {
                    if (!(listf[i].ToLower().Contains(word.ToLower())))
                    {
                        lists.RemoveAt(i);
                        listd.RemoveAt(i);
                        listt.RemoveAt(i);
                        listdt.RemoveAt(i);
                        liste.RemoveAt(i);
                        listr.RemoveAt(i);
                        listn.RemoveAt(i);
                        listf.RemoveAt(i);
                        listbool.RemoveAt(i);
                        listenc.RemoveAt(i);
                        i--;
                    }
                }
            }
            else if (rech.Equals("etudiant"))
            {
                for (int i = 0; i < liste.Count; i++)
                {
                    string str = "";
                    foreach (Etudiant e in liste[i])
                    {
                        str += e.nom + " " + e.prenom + " ";
                    }
                    if (!(str.ToLower().Contains(word.ToLower())))
                    {
                        lists.RemoveAt(i);
                        listd.RemoveAt(i);
                        listt.RemoveAt(i);
                        listdt.RemoveAt(i);
                        liste.RemoveAt(i);
                        listr.RemoveAt(i);
                        listn.RemoveAt(i);
                        listf.RemoveAt(i);
                        listbool.RemoveAt(i);
                        listenc.RemoveAt(i);
                        i--;
                    }
                }
            }
            else if (rech.Equals("filiere"))
            {
                for (int i = 0; i < lists.Count; i++)
                {
                    if (!(listenc[i].ToLower().Contains(word.ToLower())))
                    {
                        lists.RemoveAt(i);
                        listd.RemoveAt(i);
                        listt.RemoveAt(i);
                        listdt.RemoveAt(i);
                        liste.RemoveAt(i);
                        listr.RemoveAt(i);
                        listn.RemoveAt(i);
                        listf.RemoveAt(i);
                        listbool.RemoveAt(i);
                        listenc.RemoveAt(i);
                        i--;
                    }
                }
            }
        }
    }
}
