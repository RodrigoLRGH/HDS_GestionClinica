import { useNavigate } from 'react-router';
import { Card, CardContent, CardTitle } from '../components/ui/card';
import { ArrowRight, Calendar, ClipboardList, Hospital } from 'lucide-react';

const HomePage = () => {
    const navigate = useNavigate();

    return (
        <>
            <div className="min-h-screen bg-gray-50 flex flex-col items-center justify-center px-4">
                <div className="text-center mb-10">
                    <h1 className="text-4xl font-bold text-gray-800 mb-2">
                        <Hospital className="mx-auto mb-4 " size={48} />
                        Sistema de Gestión Clínica
                    </h1>
                    <p className="text-gray-500 text-lg">
                        Administra citas médicas e historial clínico de pacientes
                    </p>
                </div>

                <div className="grid grid-cols-1 md:grid-cols-2 gap-6 w-full max-w-3xl">
                    <Card
                        onClick={() => navigate('/citas')}
                        className="bg-white rounded-xl shadow-md p-6 cursor-pointer hover:shadow-lg hover:scale-105 transition-all">
                        <Calendar className="mb-4 text-blue-600" size={32} />
                        <CardTitle className="text-xl font-bold text-gray-800 mb-1">Citas Médicas</CardTitle>
                        <CardContent className="text-gray-500 text-sm">
                            Registra, consulta, edita y cancela citas médicas. Filtra por paciente, fecha o estado.
                        </CardContent>
                        <span className="mt-4 inline-block text-blue-600 font-semibold text-sm">
                            Ir a Citas <ArrowRight className="inline-block ml-1" size={16} />
                        </span>
                    </Card>

                    <Card
                        onClick={() => navigate('/historial')}
                        className="bg-white rounded-xl shadow-md p-6 cursor-pointer hover:shadow-lg hover:scale-105 transition-all">
                        <ClipboardList className="mb-4 text-green-600" size={32} />
                        <CardTitle className="text-xl font-bold text-gray-800 mb-1">Historial Clínico</CardTitle>
                        <CardContent className="text-gray-500 text-sm">
                            Consulta el historial completo de evoluciones por paciente y agrega nuevos registros.
                        </CardContent>
                        <span className="mt-4 inline-block text-green-600 font-semibold text-sm">
                            Ir a Historial <ArrowRight className="inline-block ml-1" size={16} />
                        </span>
                    </Card>
                </div>
            </div>
        </>
    );
};

export default HomePage;