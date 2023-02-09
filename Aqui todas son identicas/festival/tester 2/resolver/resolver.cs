namespace Weboo.Examen
{

    public class TestResolverEficiente
    {

    }

    public class TestResolver
    {
        static int[] sol = new int[0];
        static int best;

        public static int[] MenorCantidadEquipos(bool[,] amigos)
        {
            sol = new int[amigos.GetLength(0)];
            for (int i = 0; i < sol.Length; i++)
                sol[i] = i;
            best = amigos.GetLength(0);
            AsignarEquipo(amigos, 0, 0, new int[amigos.GetLength(0)]);
            return sol;
        }

        public static void AsignarEquipo(bool[,] amigos, int index, int total_equipos, int[] current)
        {
            if (total_equipos >= best)
                return;
            if (index == sol.Length)
            {
                best = Math.Min(total_equipos, best);
                sol = current.ToArray();
                return;
            }

            bool[] mask_equipos = new bool[total_equipos + 1];

            SeleccionarPosiblesEquipos(mask_equipos, amigos, current, index);
            for (int i = 1; i <= total_equipos; i++)
            {
                if (mask_equipos[i])
                    continue;
                current[index] = i;
                AsignarEquipo(amigos, index + 1, total_equipos, current);
                current[index] = 0;
            }
            current[index] = total_equipos + 1;
            AsignarEquipo(amigos, index + 1, total_equipos + 1, current);
            current[index] = 0;
        }

        public static void SeleccionarPosiblesEquipos(bool[] mask_equipos, bool[,] amigos, int[] equipos_asignados, int index)
        {
            for (int j = 0; j < index; j++)
            {
                if (amigos[index, j] && equipos_asignados[j] != 0)
                    mask_equipos[equipos_asignados[j]] = true;
            }
        }

    }
}
