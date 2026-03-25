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
                    
                    // Check for YouTube
                    if (project.videoUrl.includes("youtube.com") || project.videoUrl.includes("youtu.be")) {
                        let videoId = "";
                        if (project.videoUrl.includes("v=")) {
                            videoId = project.videoUrl.split("v=")[1].split("&")[0];
                        } else if (project.videoUrl.includes("youtu.be/")) {
                            videoId = project.videoUrl.split("youtu.be/")[1].split("?")[0];
                        }

                        if (videoId) {
                            videoHtml += `
                                <div class="ratio ratio-16x9" style="max-width: 600px;">
                                    <iframe src="https://www.youtube.com/embed/${videoId}" title="YouTube video" allowfullscreen></iframe>
                                </div>`;
                        }
                    } else {
                        // Standard video tag (works for resolved GitHub assets too)
                        videoHtml += `
                            <video controls style="max-width: 100%; max-height: 400px;">
                                <source src="${project.videoUrl}">
                                Your browser does not support the video tag.
                            </video>`;
                    }
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