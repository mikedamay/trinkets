package jar;

import java.io.InputStream;
import java.util.Properties;

/**
 * Hello world!
 *
 */
public class App 
{
    public static void main( String[] args )
    {
        new App();
    }
    private App() {
        try {
            Properties prop = new Properties();
            InputStream in = getClass().getResourceAsStream("/app.properties");
            prop.load(in);
            in.close();
            System.out.println( "Hello " + prop.getProperty("app.name") + "!" );
        }
        catch ( Exception ex ) {
            ex.printStackTrace();
        }
    }
}
