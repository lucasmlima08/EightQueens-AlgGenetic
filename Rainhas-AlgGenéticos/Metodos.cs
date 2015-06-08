using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainhas_AlgGenéticos
{
    class Metodos
    {
        private static Random random = new Random();

        public static Boolean encontrouSolucao = false;
        public static String solucao = "";

        public static int nRainhas = 8;

        public static int maxInteracoes = 0;
        public static int interacoes = 0;
        public static int numInteracoesAux = 0;

        public static float probabilidade_1 = 0.0f;
        public static int numInteracoes_1 = 0;

        public static float probabilidade_2 = 0.0f;
        public static int numInteracoes_2 = 0;

        public static float probabilidade_3 = 0.0f;
        public static int numInteracoes_3 = 0;

        public static float probabilidadeAtual = probabilidade_1;

        public static int melhorFitness = 0;
        public static String melhorIndividuo = "";

        public static int reset = 0;

        /* Inicia a busca pela solução das 8 rainhas a partir de um algoritmo genético */
        public static void iniciarInteracoes()
        {
            // Sorteia a primeira população com 10 indivíduos.
            List<String> populacao = new List<String>();
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
                        populacao.Clear();
                        populacao = sortearPopulacao(populacao);
                        probabilidadeAtual = probabilidade_1;
                        melhorFitness = 0;
                        melhorIndividuo = "";
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
        private static String algorimoGenetico(List<String> populacao)
        {
            List<String> novaPopulacao = new List<string>();

            // Gera uma nova população a partir do cruzamento dos indivíduos da população anterior.
            while (novaPopulacao.Count < populacao.Count)
            {
                // Sorteia dois indivíduos da população.
                String individuoSorteado_1 = getSorteioIndividuo(populacao, "");
                String individuoSorteado_2 = getSorteioIndividuo(populacao, individuoSorteado_1);
                // Realiza o cruzamento.
                String novoIndividuo = getCrossover(individuoSorteado_1, individuoSorteado_2);
                // Sorteia um double e verifica a probabilidade para realizar a mutação.
                if ((float)random.NextDouble() <= probabilidadeAtual)
                {
                    int fitness = getFitness(novoIndividuo);
                    if (fitness <= melhorFitness)
                    {
                        novoIndividuo = getMutate(novoIndividuo);
                    }
                }
                novaPopulacao.Add(novoIndividuo);
            }

            // Agora adiciona os 2 melhores indivíduos da população anterior.
            // Cria o array dos dois melhores e suas posições.
            int[] posMelhores = new int[]{0,1};
            int[] fitnessMelhores = new int[]{getFitness(populacao[0]),getFitness(populacao[1])};
            // Em seguida vai trocando sempre que encontra um melhor do que os dois.
            for (int i = 2; i < populacao.Count; i++)
            {
                int fitness = getFitness(populacao[i]);
                if (fitnessMelhores[0] > fitnessMelhores[1])
                {
                    if (fitness > fitnessMelhores[0])
                    {
                        fitnessMelhores[1] = fitness;
                        posMelhores[1] = i;
                    }
                }
                else
                {
                    if (fitness > fitnessMelhores[1])
                    {
                        fitnessMelhores[0] = fitness;
                        posMelhores[0] = i;
                    }
                }
            }

            // Agora, enfim, seleciona o melhor da nova população.
            // Primeiro guarda os fitness da nova população.
            int[] arrayFitness = new int[novaPopulacao.Count];
            for (int i = 0; i < novaPopulacao.Count; i++)
                arrayFitness[i] = getFitness(novaPopulacao[i]);
            // Depois compara os fitness para escolher o melhor.
            int fitness_melhor = arrayFitness[0];
            int pos_melhor = 0;
            for (int i = 1; i < arrayFitness.Length; i++)
            {
                if (arrayFitness[i] > fitness_melhor)
                {
                    fitness_melhor = arrayFitness[i];
                    pos_melhor = i;
                }
            }

            String melhor = novaPopulacao[pos_melhor];
            return melhor;
        }

        /* Retorna o número de rainhas sem ataque */
        private static int getFitness(String individuo)
        {
            int[] rainhas = new int[] { 0, 0, 0, 0, 0, 0, 0, 0 }; // Problema das 8 rainhas.

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
                    individuo = individuo.Replace(individuo[i],'-');
            individuo = individuo.Replace("-", "");

            return individuo.Length;
        }

        /* Retorna o cruzamento entre os dois indivíduos sorteados */
        private static String getCrossover(String individuo_1, String individuo_2)
        {
            int divAleatorio = random.Next(nRainhas);
            string novoIndividuo = individuo_1.Substring(0, divAleatorio) + individuo_2.Substring(divAleatorio, nRainhas - divAleatorio);
            return novoIndividuo;
        }

        /* Retorna a mutação de um indivíduo */
        private static String getMutate(String individuo)
        {
            int posicao = random.Next(individuo.Length); // Vai de 0 a 7 posições.
            int rainha = random.Next(individuo.Length) + 1; // Vai de 1 a 8 rainhas.
            if (posicao == individuo.Length - 1)
            {
                return individuo.Substring(0, individuo.Length - 1) + rainha;
            }
            else
            {
                return individuo.Substring(0, posicao) + rainha + individuo.Substring(posicao + 1, individuo.Length - posicao - 1);
            }
        }

        /* Sorteia um indivíduo dentro da população diferente do indivíduo auxiliar */
        private static String getSorteioIndividuo(List<String> populacao, String individuoAux)
        {
            String individuo = individuoAux;
            while (individuo.Equals(individuoAux))
            {
                int sorteio = random.Next(populacao.Count);
                individuo = populacao[sorteio];
            }
            return individuo;
        }

        /* Gera uma população de 10 indivíduos */
        private static List<String> sortearPopulacao(List<String> populacao)
        {
            while (populacao.Count < 10) // Para população com 10 indivíduos.
            {
                String individuo = getIndividuoAleatorio(nRainhas);
                populacao.Add(individuo);
            }
            return populacao;
        }

        /* Gera um indivíduo com rainhas aleatórias representado por uma string de tamanho n */
        private static String getIndividuoAleatorio(int tamanho)
        {
            String individuo = "";
            while (individuo.Length < tamanho)
                individuo += (random.Next(tamanho) + 1);
            return individuo;
        }
    }
}
