import { useState } from 'react';
import { type Cita } from '../types/Cita/Cita';
import { type CrearCitaDTO } from '../types/Cita/CrearCitaDTO';
import { type ActualizarCitaDTO } from '../types/Cita/ActualizarCitaDTO';
import ListaCitas from '../components/citas/ListaCitas';
import FormularioCita from '../components/citas/FormularioCita';
import DetalleCita from '../components/citas/DetalleCita';
import { useCitas } from '../hooks/useCitas';

type Vista = 'lista' | 'crear' | 'editar' | 'detalle';

const CitasPage = () => {
    const [vista, setVista] = useState<Vista>('lista');
    const [citaSeleccionada, setCitaSeleccionada] = useState<Cita | undefined>();
    const { crear, actualizar } = useCitas();

    const handleGuardar = async (dto: CrearCitaDTO | ActualizarCitaDTO) => {
        let exito: boolean = false;
        if (vista === 'crear') {
            exito = await crear(dto as CrearCitaDTO);
        } else {
            exito = await actualizar((dto as ActualizarCitaDTO).id, dto as ActualizarCitaDTO);
        }
        if (exito)
            setVista('lista');
        return exito;
    };

    return (
        <>
            <div className="min-vh-100 bg-light p-3">
                {vista === 'lista' && (
                    <ListaCitas
                        onNueva={() => setVista('crear')}
                        onEditar={(cita) => { setCitaSeleccionada(cita); setVista('editar'); }}
                        onDetalle={(cita) => { setCitaSeleccionada(cita); setVista('detalle'); }} />
                )}
                {(vista === 'crear' || vista === 'editar') && (
                    <FormularioCita
                        citaEditar={vista === 'editar' ? citaSeleccionada : undefined}
                        onGuardar={handleGuardar}
                        onCancelar={() => setVista('lista')} />
                )}
                {vista === 'detalle' && citaSeleccionada && (
                    <DetalleCita
                        cita={citaSeleccionada}
                        onVolver={() => setVista('lista')}
                        onEditar={() => setVista('editar')} />
                )}
            </div>
        </>
    );
};

export default CitasPage;