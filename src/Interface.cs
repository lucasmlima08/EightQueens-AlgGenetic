using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OitoRainhasAlgoritmosGeneticos
{
    public partial class Interface : Form
    {
        public Interface()
        {
            InitializeComponent();
            construirMatriz();
        }

        private Label[] matriz;
        private Metodos metodos = new Metodos();

        private void construirMatriz()
        {
            matriz = new[] { n11, n12, n13, n14, n15, n16, n17, n18, 
                             n21, n22, n23, n24, n25, n26, n27, n28,
                             n31, n32, n33, n34, n35, n36, n37, n38, 
                             n41, n42, n43, n44, n45, n46, n47, n48, 
                             n51, n52, n53, n54, n55, n56, n57, n58, 
                             n61, n62, n63, n64, n65, n66, n67, n68, 
                             n71, n72, n73, n74, n75, n76, n77, n78, 
                             n81, n82, n83, n84, n85, n86, n87, n88 };
        }

        private void construirSolucao(String rainha)
        {
            int posRainha = 0;
            int posMatriz = 0;
            for (int i = 0; i < rainha.Length; i++)
            {
                posRainha = (int)Char.GetNumericValue(rainha[i]);
                posMatriz = ((posRainha - 1) * 8) + i;
                matriz[posMatriz].Text = "X";
            }
        }

        private void resetarMatrizSolucao(String caracter)
        {
            for (int i = 0; i < matriz.Length; i++)
            {
                matriz[i].Text = caracter;
            }
        }

        private Boolean condicoesDeAceitacao()
        {
            try
            {
                metodos.probabilidade_1 = float.Parse(prob1.Text, System.Globalization.CultureInfo.InvariantCulture);
                metodos.numInteracoes_1 = Convert.ToInt32(inter1.Text);

                metodos.probabilidade_2 = float.Parse(prob2.Text, System.Globalization.CultureInfo.InvariantCulture);
                metodos.numInteracoes_2 = Convert.ToInt32(inter2.Text);

                metodos.probabilidade_3 = float.Parse(prob3.Text, System.Globalization.CultureInfo.InvariantCulture);
                metodos.numInteracoes_3 = Convert.ToInt32(inter3.Text);

                metodos.reset = Convert.ToInt32(reset.Text);

                metodos.maxInteracoes = Convert.ToInt32(maxInteracoes.Text);

                metodos.interacoes = 0;
                metodos.numInteracoesAux = 0;
                metodos.melhorFitness = 0;
                metodos.solucao = "";
                metodos.encontrouSolucao = false;

                if ((0 <= metodos.probabilidade_1) && (metodos.probabilidade_3 < 1) &&
                    (metodos.probabilidade_1 <= metodos.probabilidade_2) && (metodos.probabilidade_2 <= metodos.probabilidade_3) &&
                    (metodos.numInteracoes_1 <= metodos.numInteracoes_2) && (metodos.numInteracoes_2 <= metodos.numInteracoes_3) &&
                    (-1 < metodos.numInteracoes_1) && (metodos.numInteracoes_3 < metodos.reset) && (metodos.reset < metodos.maxInteracoes))
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("Por Favor, Siga Essas Regras: \n\n" +
                                    "Probabilidade 1 < Probabilidade 2 < Probabilidade 3 \n\n" +
                                    "0.0 < Probabilidade 1 e Pobabilidade 3 < 1.0 \n\n" +
                                    "0 < Interações 1 < Interações 2 < Interações 3 \n\n" +
                                    "Interações 3 < Reset < Máximo de Interações \n\n Obrigado..",
                                    "Não permitido!",
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("" + e.Message, "Opa! Houve um erro..", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return false;
        }

        private void solucao_Click(object sender, EventArgs e)
        {
            if (condicoesDeAceitacao())
            {
                metodos.iniciarInteracoes();

                if (metodos.encontrouSolucao == true)
                {
                    resetarMatrizSolucao("-");
                    construirSolucao(metodos.solucao);
                    interacoesFinal.Text = "" + metodos.interacoes;
                    solucaoEncontrada.Text = metodos.solucao;
                }
                else
                {
                    resetarMatrizSolucao("?");
                    interacoesFinal.Text = "" + metodos.maxInteracoes;
                    solucaoEncontrada.Text = "Nenhuma";
                }
            }
        }

        private void Limpar_Click(object sender, EventArgs e)
        {
            prob1.Text = "0.10";
            inter1.Text = "0";
            prob2.Text = "0.30";
            inter2.Text = "1000";
            prob3.Text = "0.50";
            inter3.Text = "2000";
            reset.Text = "5000";
            maxInteracoes.Text = "100000";
            interacoesFinal.Text = "";
            solucaoEncontrada.Text = "";
            resetarMatrizSolucao("-");
        }

    }
}
