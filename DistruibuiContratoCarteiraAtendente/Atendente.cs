using System;
using System.Collections.Generic;
using System.Linq;

namespace DistruibuiContratoCarteiraAtendente
{
    public class Atendente
    {
        public Atendente(int empreendimento)
        {
            Empreendimento = empreendimento;
            Contratos = new List<Contrato>();
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public int Empreendimento { get; set; }
        public List<Contrato> Contratos { get; set; }

        public int TotalContratos(bool? escritura = null, int? contadorDistribuicao = null)
        {
            return Contratos.Count(x =>  x.Escritura == (escritura ?? x.Escritura) && x.ContadorDistribuicao == (contadorDistribuicao ?? x.ContadorDistribuicao));
        }
    }
}
