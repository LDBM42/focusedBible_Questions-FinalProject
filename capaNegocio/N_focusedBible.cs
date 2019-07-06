﻿
using capaDatos;
using capaEntidad;
using System.Data;

namespace capaNegocio
{
    public class N_focusedBible
    {
        D_focusedBible objDato = new D_focusedBible();
        E_focusedBible preg = new E_focusedBible();

        public DataTable N_listado(E_focusedBible preg)
        {
            return objDato.D_listado(preg);
        }

        public DataTable N_listadoPor_DificultadYCategoria(E_focusedBible preg)
        {
            return objDato.D_listadoPor_DificultadYCategoría(preg);
        }



        public DataTable N_listarCategorias()
        {
            return objDato.D_listarCategorias();
        }

        public DataTable N_listarLibros()
        {
            return objDato.D_listarLibros();
        }

        public DataTable N_listarCategoriasXTestamento(E_focusedBible preg)
        {
            return objDato.D_listarCategoriasXTestamento(preg);
        }

        public DataTable N_listarLibrosXCategoria(string [] catEvangelios_yOtros)
        {
            return objDato.D_listarLibrosXCategoria(catEvangelios_yOtros);
        }





        public void N_Insertar(E_focusedBible preg)
        {
            objDato.D_insertar(preg); //pasamos el objeto de la capa E_focusedBible como parametro al metodo D_insertar.
        }

        public int N_NumFilas_PorDificultadYCategoria(E_focusedBible preg)
        {
            return objDato.D_NumFilas_PorDificultadYCategoria(preg);
        }


    }

}
