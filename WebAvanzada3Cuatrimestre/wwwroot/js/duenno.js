(() => {

    const Duenno = {
        tabla: null,

        init() {
            this.inicializarTabla();
            this.registrarEventos();
        },

        /**
         * Inicializa la tabla de duennos. Los datos se cargan por AJAX desde el controlador.
         * Las columnas de texto usan render.text() para escapar HTML y evitar XSS.
         */
        inicializarTabla() {
            this.tabla = new DataTable('#tblDuenno', {
                ajax: {
                    url: '/Duenno/ObtenerDuennos',
                    dataSrc: '',
                    type: 'GET'
                },
                columns: [
                    { data: 'id' },
                    { data: 'nombre', render: DataTable.render.text() },
                    { data: 'apellido1', render: DataTable.render.text() },
                    { data: 'apellido2', render: DataTable.render.text() },
                    { data: 'edad' },
                    {
                        data: null,
                        title: 'Acciones',
                        className: 'dt-actions-cell',
                        orderable: false,
                        searchable: false,
                        render: (data, type, row) => {
                            return `<div class="table-actions">
                                        <button type="button" class="btn-action btn-action-edit btn-editar" data-id="${row.id}" aria-label="Editar dueño" title="Editar">
                                            <i class="bi bi-pencil-fill" aria-hidden="true"></i>
                                        </button>
                                        <button type="button" class="btn-action btn-action-delete btn-eliminar" data-id="${row.id}" aria-label="Eliminar dueño" title="Eliminar">
                                            <i class="bi bi-trash3-fill" aria-hidden="true"></i>
                                        </button>
                                    </div>`;
                        }
                    }
                ],
                language: {
                    url: 'https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
                }
            });
        },

        /**
         * Registra los eventos de los botones de los modales y de las acciones de la tabla.
         */
        registrarEventos() {
            $('#btnGuardarDuenno').on('click', () => Duenno.guardarDuenno());
            $('#btnEditarDuenno').on('click', () => Duenno.editarDuenno());

            // Delegacion de eventos: las filas se generan dinamicamente por DataTables
            $('#tblDuenno').on('click', '.btn-eliminar', function () {
                Duenno.eliminarDuenno($(this).data('id'));
            });
            $('#tblDuenno').on('click', '.btn-editar', function () {
                Duenno.cargarDuenno($(this).data('id'));
            });

            // Limpia los formularios al cerrar los modales para no arrastrar datos
            $('#modalCrearDuenno').on('hidden.bs.modal', () => $('#formCrearDuenno')[0].reset());
            $('#modalEditarDuenno').on('hidden.bs.modal', () => $('#formEditarDuenno')[0].reset());
        },

        /**
         * Envia el formulario de creacion al servidor.
         */
        guardarDuenno() {
            Duenno.enviarFormulario($('#formCrearDuenno'), $('#modalCrearDuenno'));
        },

        /**
         * Envia el formulario de edicion al servidor.
         */
        editarDuenno() {
            Duenno.enviarFormulario($('#formEditarDuenno'), $('#modalEditarDuenno'));
        },

        /**
         * Flujo comun de crear y editar: valida en el navegador, envia el formulario
         * y muestra el resultado de las reglas de negocio.
         * @param {jQuery} form Formulario a enviar.
         * @param {jQuery} modal Modal que contiene el formulario.
         */
        enviarFormulario(form, modal) {
            if (!form[0].reportValidity()) {
                return;
            }
            

            $.ajax({
                url: form.attr('action'),
                type: 'POST',
                data: form.serialize(),
                success: function (respuesta) { // Las reglas de negocio son procesos correctos, se informan con esCorrecto, no con excepciones
                    if (respuesta.esCorrecto) {
                        Swal.fire({
                            title: "¡Listo!",
                            text: respuesta.mensaje,
                            icon: "success"
                        });
                        modal.modal('hide');
                        Duenno.tabla.ajax.reload();
                    }
                    else {
                        Swal.fire({
                            icon: "error",
                            title: "Oops...",
                            text: respuesta.mensaje
                        });
                    }
                },
                error: function (xhr) { // Errores de validacion (400) o no controlados
                    Duenno.mostrarError(xhr);
                }
            });
        },

        /**
         * Pide confirmacion y elimina el duenno. Los carros asociados se eliminan en cascada.
         * @param {number} id Id del duenno.
         */
        eliminarDuenno(id) {
            Swal.fire({
                title: "¿Estás seguro?",
                text: "Se eliminará el dueño y todos sus carros. No podrás revertir esta operación.",
                icon: "warning",
                showCancelButton: true,
                confirmButtonText: "Sí, eliminar",
                cancelButtonText: "Cancelar"
            }).then((result) => {
                if (!result.isConfirmed) {
                    return;
                }

                $.ajax({
                    url: `/Duenno/EliminarDuenno?id=${encodeURIComponent(id)}`,
                    type: 'DELETE',
                    success: function (respuesta) {
                        if (respuesta.esCorrecto) {
                            Swal.fire({
                                title: "¡Listo!",
                                text: respuesta.mensaje,
                                icon: "success"
                            });
                            Duenno.tabla.ajax.reload();
                        }
                        else {
                            Swal.fire({
                                icon: "error",
                                title: "Oops...",
                                text: respuesta.mensaje
                            });
                        }
                    },
                    error: function (xhr) {
                        Duenno.mostrarError(xhr);
                    }
                });
            });
        },

        /**
         * Obtiene el duenno por id, llena el formulario de edicion y abre el modal.
         * @param {number} id Id del duenno.
         */
        cargarDuenno(id) {
            $.get(`/Duenno/ObtenerDuennoPorId?id=${encodeURIComponent(id)}`)
                .done(function (resultado) {
                    if (resultado == null) {
                        Swal.fire({
                            icon: "error",
                            title: "Oops...",
                            text: "No se encontró el dueño seleccionado."
                        });
                        return;
                    }

                    $('#formEditarDuenno #Id').val(resultado.id);
                    $('#formEditarDuenno #Nombre').val(resultado.nombre);
                    $('#formEditarDuenno #Apellido1').val(resultado.apellido1);
                    $('#formEditarDuenno #Apellido2').val(resultado.apellido2);
                    $('#formEditarDuenno #Edad').val(resultado.edad);

                    $('#modalEditarDuenno').modal('show');
                })
                .fail(function (xhr) {
                    Duenno.mostrarError(xhr);
                });
        },

        /**
         * Muestra un error de la peticion. Si es un 400 con ModelState, lista los mensajes de validacion.
         * @param {jqXHR} xhr Respuesta fallida de la peticion.
         */
        mostrarError(xhr) {
            let mensaje = "Ocurrió un error inesperado. Intenta de nuevo.";

            if (xhr.status === 400 && xhr.responseJSON) {
                const errores = Object.values(xhr.responseJSON.errors ?? xhr.responseJSON).flat();
                if (errores.length > 0) {
                    mensaje = errores.join('\n');
                }
            }

            Swal.fire({
                icon: "error",
                title: "Oops...",
                text: mensaje
            });
        }
    };

    $(document).ready(() => Duenno.init());

})();
