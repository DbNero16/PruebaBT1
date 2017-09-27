using PruebaBT1.OBD2;

namespace PruebaBT1.Droid.OBD2
{
    interface IConnection
    {
        DataResponse ConsultParameters( Parameters.PID pid);

        DataResponse  ConsultParameters();
        DataResponse  DiagnosticCar();

        void Initialization();
    }
}