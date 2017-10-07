package maven_plugin;

import org.apache.maven.plugin.AbstractMojo;
import org.apache.maven.plugin.MojoExecutionException;
import org.apache.maven.plugin.MojoFailureException;

/**
 * @goal echo
 * @requiresProject false
 */
public class EchoMojo extends AbstractMojo
{
    /**
     * @parameter expression="${echo.message}" default-value="hey there!"
     */
    Object message;
    public void execute() throws MojoExecutionException, MojoFailureException {
        getLog().info(message.toString());
    }
}
