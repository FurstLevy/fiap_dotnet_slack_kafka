package br.com.fiap69aoj.chamados.clietnt.model;

import java.net.URI;
import java.util.HashMap;
import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.web.client.RestTemplate;
import org.springframework.web.util.UriComponentsBuilder;

import java.util.Optional;

@Service
public class UsuarioClient {

	private static final String URL_USUARIO_CLIENT = "http://usuarios:8181/usuarios/{idAcesso}";

	@Autowired
	private RestTemplate restTemplate;

	public Optional<Usuario> obterDadosUsuario(String idAcessoUsuario) {

		Map<String, String> urlParam = new HashMap<String, String>();
		urlParam.put("idAcesso", idAcessoUsuario);

		URI targetUrl = UriComponentsBuilder.fromHttpUrl(URL_USUARIO_CLIENT).buildAndExpand(urlParam).toUri();
		Optional<Usuario> retorno = Optional.ofNullable(restTemplate.getForObject(targetUrl, Usuario.class));

		return retorno;
	}

}
