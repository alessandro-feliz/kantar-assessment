# Use Nginx to serve static files
FROM nginx:alpine

# Remove default nginx website
RUN rm -rf /usr/share/nginx/html/*

# Copy your static site to nginx public directory
COPY wwwroot /usr/share/nginx/html

EXPOSE 80