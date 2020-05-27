using System;

namespace DistruibuiContratoCarteiraAtendente
{
    public class Contrato
    {
        public Contrato(int empreendimento, bool escritura = false)
        {
            Empreendimento = empreendimento;
            Escritura = escritura;
            Id = Guid.NewGuid();

        }

        public Guid Id { get; set; }
        public int Empreendimento { get; set; }
        public bool Escritura { get; set; }
        public bool Distrubuido { get; set; }

        public int ContadorDistribuicao { get; set; }

        public void SetDistribuido(int contadorDistribuicao)
        {
            Distrubuido = true;
            ContadorDistribuicao = contadorDistribuicao;
        }
    }
}
