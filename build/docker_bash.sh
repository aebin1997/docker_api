#!/bin/bash

#eco "Hello Aebin"

args=("$@")

dockerfile_location="/Users/aebin/Repository/study/docker_api/Application/Dockerfile"

case ${args[0]} in
  "Local") 
  server_environment="local"
  configuration="Debug"
  ;;
  "Development") 
  server_environment="dev"
  configuration="Debug"
  ;;
  "Staging")
  server_environment="stage"
  configuration="Release"
   ;;
  "Production")
  server_environment="prod"
  configuration="Release"
  ;;
esac

company_name="aebin"
service_name="docker_api"
service_port=7134
docker_image_name="${company_name}/${service_name}"
docker_image_tag=$(date +"%Y%m%d%H%M")

container_name="aebin-docker-api"

echo "server environment: ${server_environment}" 
echo "configuration: ${configuration}"
echo "service port: ${service_port}"
echo "docker image name: ${docker_image_name}"
echo "docker image tag: ${docker_image_tag}"
echo "docker container name: ${docker_container_name}"     

echo "=> Wait for 5 seconds.."
sleep 5s 

echo "=> Build the image.."
docker build --build-arg configuration=${configuration} --tag ${docker_image_name}:${docker_image_tag} -f ./Application/Dockerfile . 

#echo "=> On-build remove the temporary container.."
#onbuild image rm -f ${docker_image_name}

#echo "=> Remove the previous container.."
docker rm -f ${container_name}

echo "=> Run a new container.."
#docker container run -it --rm ${docker_container_name}/${docker_image_name}:${docker_image_tag}
docker run -d -p ${service_port}:${service_port} --name ${container_name} ${docker_image_name}:${docker_image_tag}

