{% for (var i=0, file; file=o.files[i]; i++) { %}
    <tr class="template-download fade">
        <td>
            <input type="hidden" value="{%=file.pictureid%}" name="pictureid" />
            <span class="preview">
    {% if (file.thumbnailUrl) { %}
                <a href="{%=file.url%}" target="_blank" title="{%=file.name%}" download="{%=file.name%}" data-gallery><img src="{%=file.thumbnailUrl%}"></a>
                {% } %}
            </span>
        </td>
        <td>
            <p class="name">               
                <input type="text" class="form-control" name="Label" placeholder="Label" value="{%=file.label%}">
                <input type="text" class="form-control" name="PictureURL" value="{%=file.url%}" readonly>   
                             
                {% if (file.url) { %}
                <a href="{%=file.url%}" title="{%=file.name%}" class="fancybox-button" data-rel="fancybox-button" download="{%=file.name%}" {%=file.thumbnailUrl?'data-gallery':''%}>{%=file.name%}</a>
                {% } else { %}
                <span>{%=file.name%}</span>
                {% } %}
                
            </p>
                {% if (file.error) { %}
            <div><span class="label label-danger">出错</span> {%=file.error%}</div>
            {% } %}
        </td>
        <td>
            <span class="size">@*{%=o.formatFileSize(file.size)%}*@
                <input type="number" class="form-control" name="SortOrder" value="{%=file.sortorder%}">
            </span>
        </td>
        <td>
                {% if (file.deleteUrl) { %}
            <button class="btn red delete btn-sm btndelete" data-type="{%=file.deleteType%}" data-url="{%=file.deleteUrl%}" {% if (file.deletewithcredentials) { %} data-xhr-fields='{"withCredentials":true}' {% } %}>
                <i class="fa fa-trash-o"></i>
                <span>删除</span>
            </button>
            <input type="checkbox" name="delete" value="1" class="toggle">
            {% } else { %}
            <button class="btn yellow cancel btn-sm">
                <i class="fa fa-ban"></i>
                <span>取消</span>
            </button>
            {% } %}
        </td>
    </tr>
            {% } %}