interface Props {
    mensaje: string;
    onCerrar?: () => void;
}

const ErrorAlert = ({ mensaje, onCerrar }: Props) => {
    return (
        <>
            <div className="alert alert-danger d-flex align-items-start gap-2 mb-4" role="alert">
                <i className="bi bi-exclamation-triangle-fill mt-1 flex-shrink-0"></i>
                <span className="flex-grow-1 small">{mensaje}</span>
                {onCerrar && (
                    <button
                        onClick={onCerrar}
                        className="btn-close ms-auto"
                        aria-label="Cerrar"
                    />
                )}
            </div>
        </>
    );
};

export default ErrorAlert;