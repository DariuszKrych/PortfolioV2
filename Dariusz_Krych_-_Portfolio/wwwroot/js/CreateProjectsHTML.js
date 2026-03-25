document.addEventListener("DOMContentLoaded", function () {
    const container = document.getElementById("projects-container");
    const spinner = document.getElementById("loading-spinner");

    fetch(fetch_location)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            spinner.style.display = 'none';
            container.style.display = 'block';

            if (data.length === 0) {
                container.innerHTML = '<p class="text-center">No projects found.</p>';
                return;
            }

            let html = '';
            data.forEach(project => {
                let videoHtml = '';
                if (project.videoUrl) {
                    videoHtml = '<div class="mt-3 mb-3">';
                    
                    // Standard video tag (works for resolved GitHub assets too)
                    videoHtml += `
                        <video controls style="display: block; margin: 0 auto; max-width: 100%; max-height: 500px;">
                            <source src="${project.videoUrl}">
                            Your browser does not support the video tag.
                        </video>`;
                    videoHtml += '</div>';
                }

                html += `
                    <div style="border: 1px solid #ccc; padding: 15px; margin-bottom: 20px; border-radius: 8px;">
                        <h2>${project.title}</h2>
                        <p>${project.description}</p>
                        ${videoHtml}
                        <a href="${project.gitHubLink}" target="_blank">View Code on GitHub</a>
                    </div>
                `;
            });

            container.innerHTML = html;
        })
        .catch(error => {
            console.error('Error fetching projects:', error);
            spinner.style.display = 'none';
            container.style.display = 'block';
            container.innerHTML = '<p class="text-danger text-center">Failed to load projects. Please try again later.</p>';
        });
});