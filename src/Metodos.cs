using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OitoRainhasAlgoritmosGeneticos
{
    class Metodos
    {
        private static Random random = new Random();

        public Boolean encontrouSolucao = false;
        public String solucao = "";

        public int nRainhas = 8;

        public int maxInteracoes = 0;
        public int interacoes = 0;
        public int numInteracoesAux = 0;

        public float probabilidade_1 = 0.0f;
        public int numInteracoes_1 = 0;

        public float probabilidade_2 = 0.0f;
        public int numInteracoes_2 = 0;

        public float probabilidade_3 = 0.0f;
        public int numInteracoes_3 = 0;

        public float probabilidadeAtual = 0;

        public int melhorFitness = 0;
        public String melhorIndividuo = "";

        public int reset = 0;

        private List<String> populacao = new List<String>();
        private List<int> listFitness = new List<int>();

        public void reiniciar()
        {
            populacao.Clear();
            listFitness.Clear();

            encontrouSolucao = false;
            solucao = "";

            interacoes = 0;
            numInteracoesAux = 0;

            probabilidadeAtual = probabilidade_1;

            melhorFitness = 0;
            melhorIndividuo = "";
        }

        /* Inicia a busca pela solução das 8 rainhas a partir de um algoritmo genético */
        public void iniciarInteracoes()
        {
            // Sorteia a primeira população com 10 indivíduos.
            reiniciar();
            populacao = sortearPopulacao(populacao);

            // Percorre o número máximo de interações.
            for (interacoes = 0; interacoes < maxInteracoes; interacoes++)
            {
                // Seleciona o melhor indivíduo da população de acordo com o algoritmo genético.
                String melhorIndividuoPopulacao = algorimoGenetico(populacao);
                int fitness = getFitness(melhorIndividuoPopulacao);
                // Verifica se encontrou uma solução.
                if (fitness == nRainhas)
                {
                    solucao = melhorIndividuoPopulacao;
                    encontrouSolucao = true;
                    break;
                }

                //  Verifica se é o melhor fitness até o momento.
                else if (fitness > melhorFitness)
                {
                    melhorFitness = fitness;
                    melhorIndividuo = melhorIndividuoPopulacao;
                    probabilidadeAtual = probabilidade_1;
                    numInteracoesAux = 0;
                }

                // Condições de probabilidades de mutação a partir do número de interações escolhidas.
                else
                {
                    numInteracoesAux++;
                    if (numInteracoesAux > reset)
                    {
                        // Apaga a população atual e gera uma nova população.
                        reiniciar();
                        populacao = sortearPopulacao(populacao);
                        numInteracoesAux = 0;
                    }
                    else if (numInteracoesAux > numInteracoes_3)
                    {
                        probabilidadeAtual = probabilidade_3;
                    }
                    else if (numInteracoesAux > numInteracoes_2)
                    {
                        probabilidadeAtual = probabilidade_2;
                    }
                }
            }
        }

        /* Gera uma nova população com base na anterior e ao final retorna o melhor indivíduo */
        private String algorimoGenetico(List<String> populacao)
        {
            List<String> novaPopulacao = new List<string>();
            List<int> novosFitness = new List<int>();

            // Gera uma nova população a partir do cruzamento dos indivíduos da população anterior.
            while (novaPopulacao.Count < populacao.Count)
            {
                // Sorteia dois indivíduos da população.
                String individuoSorteado_1 = getSorteioIndividuo(populacao, "");
                String individuoSorteado_2 = getSorteioIndividuo(populacao, individuoSorteado_1);
                // Realiza o cruzamento.
                String novoIndividuo = getCrossover(individuoSorteado_1, individuoSorteado_2);
                int fitness = getFitness(novoIndividuo);
                // Sorteia um double e verifica a probabilidade para realizar a mutação.
                if ((float)random.NextDouble() <= probabilidadeAtual)
                {
                    if (fitness <= melhorFitness)
                    {
                        novoIndividuo = getMutate(novoIndividuo);
                    }
                }
                novaPopulacao.Add(novoIndividuo);
                novosFitness.Add(fitness);
            }

            // Agora adiciona os 2 melhores indivíduos da população anterior.
            int[] posMelhores = new int[] { 0, 1 };
            int[] fitnessMelhores = new int[] { listFitness[0], listFitness[1] };
            for (int i = 2; i < populacao.Count; i++)
            {
                if (fitnessMelhores[0] > fitnessMelhores[1])
                {
                    if (listFitness[i] > fitnessMelhores[0])
                    {
                        fitnessMelhores[1] = listFitness[i];
                        posMelhores[1] = i;
                    }
                }
                else
                {
                    if (listFitness[i] > fitnessMelhores[1])
                    {
                        fitnessMelhores[0] = listFitness[i];
                        posMelhores[0] = i;
                    }
                }
            }

            // Depois procura a posição do melhor fitness.
            // E ao mesmo tempo procura os 2 piores para remover.
            int fitness_melhor = novosFitness[0];
            int pos_melhor = 0;
            int[] fitnessPiores = new int[] { novosFitness[0], novosFitness[1] };
            int[] posPiores = new int[] { 0, 1 };
            for (int i = 1; i < novosFitness.Count; i++)
            {
                // Busca o melhor
                if (novosFitness[i] > fitness_melhor)
                {
                    fitness_melhor = novosFitness[i];
                    pos_melhor = i;
                }
                // Busca os piores.
                if (fitnessPiores[0] < fitnessPiores[1])
                {
                    if (novosFitness[i] < fitnessPiores[0])
                    {
                        fitnessPiores[0] = novosFitness[i];
                        posPiores[0] = i;
                    }
                }
                else
                {
                    if (novosFitness[i] < fitnessPiores[1])
                    {
                        fitnessPiores[1] = novosFitness[i];
                        posPiores[1] = i;
                    }
                }
            }
            // Pega o melhor indivíduo da nova população.
            String melhor = novaPopulacao[pos_melhor];
            // Remove os 2 piores (maior indice primeiro).
            if (posPiores[0] > posPiores[1])
                novaPopulacao.RemoveAt(posPiores[0]);
            else
                novaPopulacao.RemoveAt(posPiores[1]);
            // A população atual passa a ser a nova população.

            populacao = novaPopulacao;
            listFitness = novosFitness;
            // Retorna o melhor indivíduo.
            return melhor;
        }

        /* Retorna o número de rainhas sem ataque */
        private int getFitness(String individuo)
        {
            int[] rainhas = new int[nRainhas];

            // Zera o array que representa as rainhas.
            for (int i = 0; i < rainhas.Length; i++)
                rainhas[i] = 0;

            // Coloca 1 na posição das rainhas com ataque.
            for (int i = 0; i < individuo.Length - 1; i++)
            {
                for (int j = i + 1; j < individuo.Length; j++)
                {
                    int n1 = (int)Char.GetNumericValue(individuo[i]);
                    int n2 = (int)Char.GetNumericValue(individuo[j]);
                    int a1 = j - i;
                    int a2 = Math.Abs(n2 - n1);

                    if ((n1 == n2) || (a1 == a2))
                    {
                        rainhas[i] = 1;
                        rainhas[j] = 1;
                    }
                }
            }

            // Remove rainhas com ataque.
            for (int i = 0; i < rainhas.Length; i++)
                if (rainhas[i] == 1)
                    individuo = individuo.Replace(individuo[i], '-');
            individuo = individuo.Replace("-", "");

            return individuo.Length;
        }

        /* Retorna o cruzamento entre os dois indivíduos sorteados */
        private String getCrossover(String individuo_1, String individuo_2)
        {
            int divAleatorio = random.Next(0, nRainhas);
            string novoIndividuo = individuo_1.Substring(0, divAleatorio) + individuo_2.Substring(divAleatorio, nRainhas - divAleatorio);
            return novoIndividuo;
        }

        /* Retorna a mutação de um indivíduo */
        private String getMutate(String individuo)
        {
            int posicao = random.Next(0, individuo.Length); // Vai de 0 a 7 posições.
            int rainha = random.Next(1, individuo.Length + 1); // Vai de 1 a 8 rainhas.

            if (posicao == individuo.Length - 1)
                return individuo.Substring(0, individuo.Length - 1) + rainha;
            else
                return individuo.Substring(0, posicao) + rainha + individuo.Substring(posicao + 1, individuo.Length - posicao - 1);
        }

        /* Sorteia um indivíduo dentro da população diferente do indivíduo auxiliar */
        private String getSorteioIndividuo(List<String> populacao, String individuoAux)
        {
            String individuo = individuoAux;
            while (individuo.Equals(individuoAux))
            {
                int sorteio = random.Next(0, populacao.Count);
                individuo = populacao[sorteio];
            }
            return individuo;
        }

        /* Gera uma população de 10 indivíduos */
        private List<String> sortearPopulacao(List<String> populacao)
        {
            while (populacao.Count < 10) // Para população com 10 indivíduos.
            {
                String individuo = getIndividuoAleatorio(nRainhas);
                int fitness = getFitness(individuo);
                populacao.Add(individuo);
                listFitness.Add(fitness);
            }
            return populacao;
        }

        /* Gera um indivíduo com rainhas aleatórias representado por uma string de tamanho n */
        private String getIndividuoAleatorio(int tamanho)
        {
            String individuo = "";
            while (individuo.Length < tamanho)
                individuo += (random.Next(1, tamanho + 1));
            return individuo;
        }
    }
}
