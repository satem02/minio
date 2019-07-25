FROM nginx
COPY minio.conf /etc/nginx/conf.d/minio.conf
COPY nginx.conf /etc/nginx/nginx.conf
RUN [ "mv", "/etc/nginx/conf.d/default.conf", "/etc/nginx/conf.d/default.conf_" ]