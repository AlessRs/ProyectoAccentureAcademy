let selec = document.querySelector(".seleccionado");
let asideForm = document.getElementById("asideForm");
let col = document.querySelector(".busqueda-avanzada-contenido").children.length+1
validar(document.querySelector("#Autor"));
validar(document.querySelector("#Editorial"));
columnasAdaptables(document.querySelector("table"))

document.getElementById("busqueda-avanzada-boton").addEventListener("click", e => {
    document.querySelector(".busqueda-avanzada-contenido").classList.toggle("drop-downed");
});
document.querySelectorAll("tr").forEach((fila) => {
    fila.addEventListener("click", (e) => {
        selec.classList.remove("seleccionado");
        selec = e.currentTarget
        selec.classList.add("seleccionado");
        for (let i = 0; i < col; i++) {
            asideForm[i].value = selec.children[i].innerText;
        }
    });
});

function validar(input) {
    input.addEventListener("change", (e) => {
        let req = new XMLHttpRequest;
        req.open("POST", `/Home/Val${input.name}`);
        req.onreadystatechange= ()=>{
            if (req.readyState == 4) {
                if (req.responseText == "false") {
                    input.classList.add("invalido");
                    let errorTag = document.getElementById(`error${input.name}`);
                    errorTag.innerText = "No se encuentra el elemento. Se creara un registro nuevo"
                    
                    input.addEventListener("mouseenter",()=>{
                        errorTag.style.display = "block";
                        input.addEventListener("mousemove", (e) => {
                            errorTag.style.left = 20 + e.pageX + "px";
                            errorTag.style.top = e.pageY + "px";
                        })
                    })
                    input.addEventListener("mouseleave",(e)=>{
                        errorTag.style.display = "none";
                        console.log("leaving")
                    })
                }
                else {
                    input.classList.remove("invalido");   
                }
            }
        }
        let parameters = new FormData();
        parameters.append("values", input.value);
        req.send(parameters);
    });
}
function columnasAdaptables(table) {
    let fila = table.getElementsByTagName('tr')[0],
    columnas = fila ? fila.children : undefined;
    if (!columnas) return;
    for (let columna of columnas){
        let div = crearBarra(table.offsetHeight);
        columna.appendChild(div);
        columna.style.position = 'relative';
        listeners(div);
    }
}
function crearBarra(altura){
    let div = document.createElement('div');
    div.classList.add("barra");
    div.style.height = altura+'px';
    return div;
}
function listeners(div){
    let posX,columna,proxColumna,columnaWidth,proxColumnaWidth;
    div.addEventListener('mousedown', function (e) {
        columna = e.target.parentElement;
        proxColumna = columna.nextElementSibling;
        posX = e.pageX;
        columnaWidth = columna.offsetWidth;
        if (proxColumna)
            proxColumnaWidth = proxColumna.offsetWidth;
    });

    document.addEventListener('mousemove', function (e) {
        if (columna) {
        let varX = e.pageX - posX;
        if (proxColumna)
            proxColumna.style.width = proxColumnaWidth - varX +'px';

        columna.style.width = columnaWidth + varX+'px';
        }
    });

    document.addEventListener('mouseup', function (e) { 
        columna = undefined;
        proxColumna = undefined;
        pageX = undefined;
        proxColumnaWidth = undefined;
        columnaWidth = undefined;
    });
}

function Buscar() {
    let input = document.getElementById("searchForm").querySelectorAll("input");
    window.open(`/Home/Buscar?PalabraClave=${input[0].value}&Titulo=${input[1].value}&Autor=${input[2].value}&Editorial=${input[3].value}&ISBN=${input[4].value}&Genero=${input[5].value}&Edicion=${input[6].value}`,"_self")
   
}
function Agregar() {
    sendFormPost("/Home/Agregar");
    let tr = document.createElement("tr");
    for (let i = 0; i < col; i++) {
        let td = document.createElement("td");
        td.innerText = asideForm[i].value;
        tr.appendChild(td);
    }
    document.querySelector("tbody").insertBefore(tr,selec);
}
function Editar() {
    sendFormPost("/Home/Editar");
    for (let i = 0; i < col; i++) {
        selec.children[i].innerText = asideForm[i].value; 
    }
}
function Borrar() {
    sendFormPost("/Home/Borrar");
    asideForm.reset();
    selec.remove();
}
function sendFormPost(url) {
    let req = new XMLHttpRequest;
    req.open("POST", url);
    req.addEventListener("readystatechange", (e) => {
        if (e.target.readyState == 4 && e.target.status != 200) {
            alert(req.responseText);
        }
    })
    req.send(new FormData(asideForm));
}